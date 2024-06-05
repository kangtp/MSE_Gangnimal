using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;


public class PlayerInfo : NetworkBehaviour
{
    public int HP = 100;
    public float movingTime = 10.0f;
    public bool myTurn = true;

    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] bullets;
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


    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("sival : " + gameObject.transform.GetChild(0).gameObject.name);
        firePosition =  NetworkManager.Singleton.LocalClient.PlayerObject.gameObject.transform.GetChild(0).gameObject;
        //StartCoroutine("awaitPowerGage");
    }

    private IEnumerator awaitPowerGage()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (GameObject.Find("PowerGageManage").GetComponent<PowerGage>() != null)
            {
                powerGage = GameObject.Find("PowerGageManage").GetComponent<PowerGage>();
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
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Interaction();
        ShootingBullet();
    }

    //�߻� �� ���� ����
    public void TakeDamage(int damage)
    {
        HP -= damage;
        Debug.Log("HP is : " + HP);
        if (HP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player has died.");
    }


    void destroyWeapon()
    {
        if (weaponIndex != -1 && hasWeapons[weaponIndex])
        {
            //WaitCoroutine(2.0f);

            hasWeapons[weaponIndex] = false;
            weapons[weaponIndex].SetActive(false);
        }

    }

    void ShootingBullet()
    {

        if (Input.GetMouseButton(0))
        {
            DrawParabola();
        }

        if (Input.GetMouseButtonUp(0))
        {

            GameObject bomb = null;

            if (weaponIndex != -1 && hasWeapons[weaponIndex]) bomb = Instantiate(bullets[weaponIndex]);

            if (bomb != null)
            {
                bomb.transform.position = firePosition.transform.position;
                Rigidbody rb = bomb.GetComponent<Rigidbody>();
                Vector3 throwDirection = firePosition.transform.forward.normalized;
                rb.AddForce(throwDirection * throwPower * powerGage.powerValue, ForceMode.Impulse);
                lineRenderer.enabled = false;


                destroyWeapon();

            }
            lineRenderer.enabled = false;

        }

    }


    void DrawParabola()
    {
        Debug.Log(NetworkManager.Singleton.LocalClientId);
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




    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Item")
        {
            if (other.name == "Shield(Clone)")
            {
                haveShield = true;
            }
            if (other.name == "Healpack(Clone)")
            {
                HP += 10;
                HP = Math.Clamp(HP, 0, 100);
            }
            Destroy(other.gameObject);

        }

    }

    void GetInput()
    {

        iDown = Input.GetKeyDown(KeyCode.E);


        //Debug.Log(iDown);

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
        if (iDown && nearObject != null)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
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

                Destroy(nearObject);
            }
        }
    }

    IEnumerator WaitCoroutine(float t)
    {
        //Debug.Log("MySecondCoroutine;" + t);
        yield return new WaitForSeconds(t);
    }
}
