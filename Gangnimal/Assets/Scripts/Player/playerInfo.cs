using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class PlayerInfo : MonoBehaviour
{
    public int myHP = 100;
    public float movingTime = 10.0f;
    public bool myTurn = true;

    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] bullets;
    //public static PlayerInfo instance;
    public PowerGage powerGage;


    //public gameObject myWeapon;

    public bool haveShield = false;

    bool iDown;
    int weaponIndex = -1;

    GameObject nearObject;

    
    //line
    [SerializeField]
    public LineRenderer lineRenderer;
    public int numofDot;
    public float timeInterval;
    public float maxTime;

    public GameObject firePosition;
    //public GameObject bombFactory;


    public float throwPower;


    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Interaction();

        ShootingBullet();
    }

    //�߻� �� ���� ����
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
        DrawParabola();

        if (Input.GetMouseButtonUp(0))
        {
            
            GameObject bomb = null;
            
            if (weaponIndex != -1 && hasWeapons[weaponIndex]) bomb = Instantiate(bullets[weaponIndex]);


            //bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            Vector3 throwDirection = firePosition.transform.forward.normalized;
            rb.AddForce(throwDirection * throwPower * powerGage.powerValue, ForceMode.Impulse);
            Debug.Log(throwDirection * throwPower * powerGage.powerValue);
            lineRenderer.enabled = false;


            destroyWeapon();
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




    void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Item")
        {
            if (other.name == "Shield")
            {
                haveShield = true;
            }
            if (other.name == "Healpack")
            {
                myHP += 10;
                myHP = Math.Clamp(myHP, 0, 100);
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
        if(other.tag == "Weapon")
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
            if(nearObject.tag == "Weapon") {
                Item item = nearObject.GetComponent<Item> ();
                weaponIndex = item.value;


                for(int i = 0; i < 3; i++)
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
