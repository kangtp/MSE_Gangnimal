using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, SubjectInterface
{
    public int HP = 100;
    public float movingTime = 10.0f;
    public bool myTurn = true;
    private int allDamage;// 적에게 가한 데미지

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

    private List<Observerinterface> observers = new List<Observerinterface>();

    // Start is called before the first frame update
    private void Start()
    {
        firePosition = GameObject.Find("Fireposition");
        powerGage = GameObject.Find("Canvas").GetComponent<PowerGage>();
        if (powerGage == null)
        {
            Debug.LogError("powergage is no");
        }
        GameManager.instance.isGameOver=false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("rhkdus"+GameManager.instance.isGameOver);
        if (!GameManager.instance.isGameOver)
        {
            GetInput();
            Interaction();
            ShootingBullet();
        }
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        NotifyObservers();
        Debug.Log("HP is : " + HP);
    }

    void destroyWeapon()
    {
        if (weaponIndex != -1 && hasWeapons[weaponIndex])
        {
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
                NotifyObservers();
            }
            other.gameObject.SetActive(false);
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

                nearObject.SetActive(false);
            }
        }
    }

    IEnumerator WaitCoroutine(float t)
    {
        yield return new WaitForSeconds(t);
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
            observer.InformationUpdate(HP);
        }
    }
}
