using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Netcode;
using Unity.Networking;
using Unity.VisualScripting;
using Unity.Services.Lobbies.Models;


public class PlayerInfo : NetworkBehaviour, SubjectInterface
{
    public int HP;
    public float movingTime;

    public GameObject[] weapons;
    public bool[] hasWeapons;

    public GameObject[] bullets;
    public GameObject[] items;

    private const int shieldIndex = 5;
    private const int healIndex = 6;

    public bool haveShield = false;

    private bool pickUp;
    private int weaponIndex = -1;

    private GameObject nearObject;  //Weapon => Item : Association
    private PowerGage powerGage;

    //to show Parabola
    [SerializeField]
    public LineRenderer lineRenderer;
    public int numofDot;
    public float timeInterval;
    public float maxTime;

    private GameObject firePosition;

    public float throwPower;
    private int maxHp;

    private List<Observerinterface> observers = new List<Observerinterface>();

    // Start is called before the first frame update
    private void Start()
    {
        FindObjectOfType<HealthUI>().RegisterObserver();

        //Run after player prefab is created
        StartCoroutine("awaitFirePos");
        StartCoroutine("awaitPowerGage");

        maxHp = HP;
    }

    private IEnumerator awaitFirePos()
    {
        yield return new WaitForSeconds(1.0f);
        firePosition = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.transform.GetChild(0).gameObject;
        StartCoroutine("awaitPowerGage");

    }

    private IEnumerator awaitPowerGage()
    {
        yield return new WaitForSeconds(1.0f);
        powerGage = GameObject.Find("PowerManager").transform.GetChild(0).GetComponent<PowerGage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsLocalPlayer)
            return;

        GetInput();
        Interaction();
        ShootingBullet();
        
    }

    //Function that damages the client player
    [ClientRpc]
    public void ApplyDamageToClientRpc(int damage)
    {
        //enter only client
        if (!IsServer)
        {
            //shield check
            if (haveShield)
            {
                damage -= 10;   //Reduced damage by 10
                haveShield = false;
                FindObjectOfType<ShieldUI>().ShieldOff();
                RequestNotVisibleItemServerRpc(shieldIndex);
            }
            
            HP -= damage;
            NotifyObservers();
            Debug.Log("Client HP is : " + HP);
        }
    }

    //Function that damages the Host player
    public void TakeDamage(int damage)
    {
        //shield check
        if (haveShield)
        {
            damage -= 10;
            haveShield = false;
            items[0].SetActive(false);
            FindObjectOfType<ShieldUI>().ShieldOff();

            //To make the shield effect invisible in server, client
            RequestNotVisibleItemClientRpc(shieldIndex);

        }
        HP -= damage;
        NotifyObservers();
        Debug.Log("Server HP is : " + HP);
    }


    //a function that makes the weapon in hand invisible
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

    //a function of firing a picked-up weapon
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


    //A function that shows the trajectory of the weapon being fired
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
                items[0].SetActive(true);   //shield effect on
                FindObjectOfType<ShieldUI>().ShieldOn();

                //to show the shield acquired
                if (IsServer) { RequestVisibleItemClientRpc(shieldIndex); }
                if (!IsServer) { RequestVisibleItemServerRpc(shieldIndex); }
            }
            if (other.name == "Healpack(Clone)")
            {
                GameManager.instance.PlayHpSound();
                items[1].SetActive(true);   //healing effect on

                //to show the healing acquired
                if (IsServer) { RequestVisibleItemClientRpc(healIndex); }
                if (!IsServer) { RequestVisibleItemServerRpc(healIndex); }

                HP += 10;
                HP = Math.Clamp(HP, 0, maxHp);
                NotifyObservers();
            }

            //Items acquired will disappear from the map
            other.gameObject.GetComponent<Item>().RequestDespawnServerRpc();

        }
    }


    void GetInput()
    {
        pickUp = Input.GetKeyDown(KeyCode.E);
    }

    //If the player is around the weapon, store that weapon in the nearObject variable.
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
        //If there's a weapon around and you press the e key
        if (pickUp && nearObject != null)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                if (item != null)
                {
                    //Items picked up will disappear from the map
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

                //show a weapon in one's hand
                if (IsServer) { RequestVisibleItemClientRpc(weaponIndex); }
                if (!IsServer) { RequestVisibleItemServerRpc(weaponIndex); }
            }
        }
    }


    //Function to turn off the heel effect (because the heel effect is infinite loop)
    public void HealEffectOff()
    {
        items[1].SetActive(false);
    }


    //A function that generates and shows the weapon that was fired
    [ServerRpc(RequireOwnership = false)]
    private void SpawnBulletServerRpc(int index, Vector3 position, Vector3 throw_D,float throw_p, float p_value)
    {
        GameObject InstantiatedBullet = Instantiate(bullets[index], position, Quaternion.identity);
        InstantiatedBullet.GetComponent<NetworkObject>().Spawn();
        Vector3 throwDirection = throw_D;
        InstantiatedBullet.GetComponent<Rigidbody>().AddForce(throwDirection * throw_p * p_value, ForceMode.Impulse);

    }

    //Make the item visible on the server
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
            Invoke("HealEffectOff", 2f);    //after 2 sec, healing effect off
            RequestHealingOffClientRpc();
        }
        else
        {
            weapons[value].SetActive(true);
        }
    }

    //Make the item visible on the client
    [ClientRpc]
    public void RequestVisibleItemClientRpc(int value)
    {
        if (value == shieldIndex)
        {
            items[0].SetActive(true);   //shield -> true
        }
        else if(value == healIndex)
        {
            items[1].SetActive(true);   //healing -> true
            Invoke("HealEffectOff", 2f);    //after 2 sec, healing effect off
        }
        else
        {
            weapons[value].SetActive(true);
        }
        
    }

    //Make the item invisible on the server
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


    //Make the item invisible on the client
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

    //Remove healing effects from the client.
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

    //Subscribers will be notified when the player's physical strength changes.
    public void NotifyObservers()
    {
        foreach (Observerinterface observer in observers)
        {
            Debug.Log(observer.ToString());
            observer.InformationUpdate(HP);
        }
    }
}