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

    

    // Start is called before the first frame update
    private void Start()
    {
        FindObjectOfType<HealthUI>().RegisterObserver();
        firePosition = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.transform.GetChild(0).gameObject;
        StartCoroutine("awaitPowerGage");

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
            if(IsServer)
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
    // Enable the LineRenderer to start drawing the parabola.
    lineRenderer.enabled = true;

    // Create an array to store the points of the parabola.
    Vector3[] points = new Vector3[numofDot];

    // Get the starting position and the initial velocity.
    Vector3 startPosition = firePosition.transform.position; // The position where the projectile is fired from.
    Vector3 startVelocity = throwPower * firePosition.transform.forward; // The initial velocity of the projectile based on throw power and direction.

    // Calculate the time interval between each point along the parabola.
    float timeInterval = maxTime / numofDot;
    
    // Calculate each point's position along the parabola.
    for (int i = 0; i < numofDot; i++)
    {
        float t = i * timeInterval; // The current time for the i-th point.
        // Calculate the position of the point at time t using the formula:
        // position = start position + initial velocity * time + 0.5 * gravity * time^2
        points[i] = startPosition + startVelocity * t + 0.5f * Physics.gravity * t * t;
    }

    // Set the number of positions in the LineRenderer and apply the calculated points.
    lineRenderer.positionCount = numofDot; // Set the number of points to be drawn.
    lineRenderer.SetPositions(points); // Update the LineRenderer with the calculated points.
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