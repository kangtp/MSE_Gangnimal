using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;


public class PlayerInfo : MonoBehaviour
{
    public int HP = 100;
    public float movingTime = 10.0f;
    public bool myTurn = true;
    

    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] bullets;
    public float changeDelay=2f;
    public bool haveShield = false;
    PowerGage powerGage;
    
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


    // Start is called before the first frame update
    private void Start()
    {
        firePosition = GameObject.Find("Fireposition");
        powerGage = GameObject.Find("Canvas").GetComponent<PowerGage>();
        if(powerGage ==null)
        {
            Debug.LogError("powergage is no");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Interaction();
        ShootingBullet();
    }

    
    public void TakeDamage(int damage)
    {
        HP -= damage;
        Debug.Log("HP is : " + HP);
        
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
        
        if(Input.GetMouseButton(0))
        {
            if(detect)
            {
                lineRenderer.startColor =Color.red;
                lineRenderer.endColor =Color.red;
                DrawParabola();
            }
            else
            {
                lineRenderer.startColor =Color.blue;
                lineRenderer.endColor =Color.blue;
                DrawParabola();
            }
           
        }

        if (Input.GetMouseButtonUp(0))
        {
            
            GameObject bomb=null;
            
            if (weaponIndex != -1 && hasWeapons[weaponIndex]) bomb = Instantiate(bullets[weaponIndex]);

            if(bomb != null)
            {
                bomb.transform.position = firePosition.transform.position;
                Rigidbody rb = bomb.GetComponent<Rigidbody>();
                Vector3 throwDirection = firePosition.transform.forward.normalized;
                rb.AddForce(throwDirection * throwPower * powerGage.powerValue, ForceMode.Impulse);
                lineRenderer.enabled = false;
                destroyWeapon();

            }
            lineRenderer.enabled=false;

        }

    }


    void DrawParabola()
    {
        detect=false;
        lineRenderer.enabled = true;
        Vector3[] points = new Vector3[numofDot];
        Vector3 startPosition = firePosition.transform.position;
        Vector3 startVelocity = throwPower * firePosition.transform.forward;
        
        float timeInterval = maxTime / numofDot;
       

        for (int i = 0; i < numofDot; i++)
        {
            float t = i * timeInterval;
            points[i] = startPosition + startVelocity * t + 0.5f * Physics.gravity * t * t;

            // 레이쏴서 플레이어 인식하면!
            if (Physics.Raycast(points[i], Vector3.forward, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Player")))
            {
                detect =true;
                Debug.Log("충돌한 물체: " + hit.collider.name);
                
                points[i] = hit.point;
                lineRenderer.positionCount = i + 1;
                lineRenderer.SetPositions(points);
                break;
                
            }
            lineRenderer.positionCount = numofDot;
            lineRenderer.SetPositions(points);
            
        }

    
    }
    




    void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Item")
        {
            if (other.name == "Shield(Clone)")
            {
                GameManager.instance.PlayShieldSound();
                haveShield = true;
            }
            if (other.name == "Healpack(Clone)")
            {
                GameManager.instance.PlayHpSound();
                HP += 10;
                HP = Math.Clamp(HP, 0, 100);
            }
            other.gameObject.SetActive(false);

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

                nearObject.SetActive(false);
            }
        }
    }

    IEnumerator WaitCoroutine(float t)
    {
        //Debug.Log("MySecondCoroutine;" + t);
        yield return new WaitForSeconds(t);
    }
}
