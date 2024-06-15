using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Netcode;
using Unity.Networking;
using Unity.VisualScripting;


public class PlayerInfo : NetworkBehaviour, SubjectInterface
{
    public int HP = 100;
    public float movingTime = 10.0f;
    public bool myTurn = true;
    private int allDamage;// 적에게 가한 데미지

    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] bullets;
    public GameObject[] items;

    private int shieldIndex = 5;
    private int healIndex = 6;

    public float changeDelay=2f;
    //private GameObject bullet;
    //public static PlayerInfo instance;
    PowerGage powerGage;


    //public gameObject myWeapon;

    public bool haveShield = false;

    bool detect;
    bool iDown;
    int weaponIndex = -1;

    GameObject nearObject;//Weapon => Item : Association
    
    //line
    [SerializeField]
    public LineRenderer lineRenderer;
    public int numofDot;
    public float timeInterval;
    public float maxTime;

    GameObject firePosition;
    //public GameObject bombFactory;

    public float throwPower;

    private List<Observerinterface> observers = new List<Observerinterface>();

    private string playerId;

    // Start is called before the first frame update
    private void Start()
    {
        FindObjectOfType<HealthUI>().RegisterObserver();
        StartCoroutine("awaitfirePos");
        StartCoroutine("awaitPowerGage");
    }

    private IEnumerator awaitfirePos()
    {
        yield return new WaitForSeconds(1.0f);
        firePosition = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.transform.GetChild(0).gameObject;
    }


    private IEnumerator awaitPowerGage()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (GameObject.Find("PowerManager").transform.GetChild(0).GetComponent<PowerGage>() != null)
            {
                powerGage = GameObject.Find("PowerManager").transform.GetChild(0).GetComponent<PowerGage>();
                if (powerGage == null)
                {
                    Debug.Log("powergage is no");
                }
                else
                {
                    StopCoroutine("awaitPowerGage");
                }
            }
        }
        GameManager.instance.isGameOver=false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsLocalPlayer) { return; }
        GetInput();
        Interaction();
        ShootingBullet();
    }

    
    [ClientRpc]
    public void ApplyDamageToClientRpc(int damage)
    {
        if (!IsServer)
        {
            if (haveShield)
            {
                damage -= 10;
                Debug.Log("shield!!! " + damage);
                haveShield = false;
                RequestNotVisibleItemServerRpc(shieldIndex);
                FindObjectOfType<ShieldUI>().ShieldOff();
            }
            
            HP -= damage;
            NotifyObservers();
            Debug.Log("Client HP is : " + HP);
        }
    }

    public void TakeDamage(int damage)
    {
        if (haveShield)
        {
            damage -= 10;
            haveShield = false;
            Debug.Log("shield!!! " + damage);
            items[0].SetActive(false);
            FindObjectOfType<ShieldUI>().ShieldOff();
            if (IsServer)
                RequestNotVisibleItemClientRpc(shieldIndex);
            if (!IsServer)
                RequestNotVisibleItemServerRpc(shieldIndex);
        }
        HP -= damage;
        NotifyObservers();
        Debug.Log("Server HP is : " + HP);
    }


    void destroyWeapon(int weaponIndex)
    {
        if (weaponIndex != -1 && hasWeapons[weaponIndex])
        {
            hasWeapons[weaponIndex] = false;
            weapons[weaponIndex].SetActive(false);
            if (IsServer) { RequestNotVisibleItemClientRpc(weaponIndex); }
            if (!IsServer) { RequestNotVisibleItemServerRpc(weaponIndex); }
        }
    }

    void ShootingBullet()
    {

        if (!IsLocalPlayer)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            DrawParabola();
        }

        if (Input.GetMouseButtonUp(0) && IsLocalPlayer)
        {

            if (weaponIndex != -1 && hasWeapons[weaponIndex])
                SpawnBulletServerRpc(weaponIndex, firePosition.transform.position,firePosition.transform.forward.normalized,throwPower,powerGage.powerValue);

            destroyWeapon(weaponIndex);

            lineRenderer.enabled = false;
        }
    }



    void DrawParabola()
    {
        lineRenderer.enabled = true;

        Vector3[] points = new Vector3[numofDot];
        Vector3 startPosition = firePosition.transform.position;
        Vector3 startVelocity = throwPower * firePosition.transform.forward;

        float timeInterval = maxTime / numofDot;
        for (int i = 0; i < numofDot; i++)
        {
            float t = i * timeInterval;
            points[i] = startPosition + startVelocity * t + 0.5f * Physics.gravity * t * t;
        }

        lineRenderer.positionCount = numofDot;
        lineRenderer.SetPositions(points);
    }


    /*
     Pick Up the items
     */
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            if (other.name == "Shield(Clone)")
            {
                GameManager.instance.PlayShieldSound();
                haveShield = true;
                items[0].SetActive(true);
                FindObjectOfType<ShieldUI>().ShieldOn();

                if (IsServer) { RequestVisibleItemClientRpc(shieldIndex); }
                if (!IsServer) { RequestVisibleItemServerRpc(shieldIndex); }
            }
            if (other.name == "Healpack(Clone)")
            {
                GameManager.instance.PlayHpSound();
                items[1].SetActive(true);
                if (IsServer) { RequestVisibleItemClientRpc(healIndex); }
                if (!IsServer) { RequestVisibleItemServerRpc(healIndex); }

                HP += 10;
                HP = Math.Clamp(HP, 0, 100);
                NotifyObservers();
            }
            other.gameObject.GetComponent<Item>().RequestDespawnServerRpc();

        }
    }

    void GetInput()
    {
        iDown = Input.GetKeyDown(KeyCode.E);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
        }
    }

    void Interaction()
    {
        if (!IsLocalPlayer)
        {
            return;
        }
        if (iDown && nearObject != null)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                if (item != null)
                {
                    item.RequestDespawnServerRpc();
                }
                weaponIndex = item.value;


                for (int i = 0; i < 3; i++)
                {
                    if (hasWeapons[i])
                    {
                        hasWeapons[i] = false;

                        weapons[i].SetActive(false);
                    }
                }
                
                hasWeapons[weaponIndex] = true;
                weapons[weaponIndex].SetActive(true);

                if (IsServer) { RequestVisibleItemClientRpc(weaponIndex); }
                if (!IsServer) { RequestVisibleItemServerRpc(weaponIndex); }
            }
        }
    }


    public void HealEffectOff()
    {
        items[1].SetActive(false);
    }

    
    
    [ServerRpc(RequireOwnership = false)]
    private void SpawnBulletServerRpc(int index, Vector3 position, Vector3 throw_D,float throw_p, float p_value)
    {
        GameObject InstantiatedBullet = Instantiate(bullets[index], position, Quaternion.identity);
        InstantiatedBullet.GetComponent<NetworkObject>().Spawn();
        Vector3 throwDirection = throw_D;
        InstantiatedBullet.GetComponent<Rigidbody>().AddForce(throwDirection * throw_p * p_value, ForceMode.Impulse);

    }

    [ClientRpc]
    private void SpawnBulletClientRpc(int index)
    {
        GameObject InstantiatedBullet = Instantiate(bullets[index], firePosition.transform.position, Quaternion.identity);
        InstantiatedBullet.GetComponent<NetworkObject>().Spawn();
        Vector3 throwDirection = firePosition.transform.forward.normalized;
        InstantiatedBullet.GetComponent<Rigidbody>().AddForce(throwDirection * throwPower * powerGage.powerValue, ForceMode.Impulse);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestVisibleItemServerRpc(int value)
    {
        if (value == shieldIndex)
        {
            items[0].SetActive(true);   //shield -> true
        }
        else if (value == healIndex)
        {
            items[1].SetActive(true);   //healing -> true
            Invoke("HealEffectOff", 1f);    //after 1 sec, healing effect off
            RequestHealingOffClientRpc();
        }
        else
        {
            weapons[value].SetActive(true);
        }
    }

    [ClientRpc]
    public void RequestVisibleItemClientRpc(int value)
    {
        if(value == shieldIndex)
        {
            items[0].SetActive(true);   //shield -> true
        }
        else if(value == healIndex)
        {
            items[1].SetActive(true);   //healing -> true
            Invoke("HealEffectOff", 2f);    //after 1 sec, healing effect off
        }
        else
        {
            weapons[value].SetActive(true);
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestNotVisibleItemServerRpc(int value)
    {
        Debug.Log("RequestNotVisibleItemServerRpc" + value);
        if (value == shieldIndex)
        {
            items[0].SetActive(false);   //shield -> true
            RequestNotVisibleItemClientRpc(value);
        }
        else if (value == healIndex)
        {
            items[1].SetActive(false);   //healing -> true
            RequestNotVisibleItemClientRpc(value);
        }
        else
        {
            weapons[value].SetActive(false);
        }
    }



    [ClientRpc]
    public void RequestNotVisibleItemClientRpc(int value)
    {
        Debug.Log("RequestNotVisibleItemClientRpc" + value);
        if (value == shieldIndex)
        {
            items[0].SetActive(false);   //shield -> true
        }
        else if (value == healIndex)
        {
            items[1].SetActive(false);   //healing -> true
        }
        else
        {
            weapons[value].SetActive(false);
        }
    }

    [ClientRpc]
    public void RequestHealingOffClientRpc()
    {
        Invoke("HealEffectOff", 2f);
    }

    public void RegisterObserver(Observerinterface observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(Observerinterface observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (Observerinterface observer in observers)
        {
            Debug.Log(observer.ToString());
            observer.InformationUpdate(HP);
        }
    }
}