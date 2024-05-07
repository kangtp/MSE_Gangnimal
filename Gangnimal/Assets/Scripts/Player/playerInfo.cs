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

    //public gameObject myWeapon;

    public bool haveShield = false;

    bool iDown;
    int weaponIndex = -1;

    GameObject nearObject;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Interaction();
        destroyWeapon();
    }

    void destroyWeapon()
    {
        if (Input.GetMouseButtonUp(0) && hasWeapons[weaponIndex])
        {
            hasWeapons[weaponIndex] = false;
            weapons[weaponIndex].SetActive(false);
        }

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

        Debug.Log(nearObject.name);
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

                if (hasWeapons[0])
                {
                    hasWeapons[0] = false;
                    weapons[0].SetActive(false);
                }
                else if (hasWeapons[1])
                {
                    hasWeapons[1] = false;
                    weapons[1].SetActive(false);
                }
                else if (hasWeapons[2])
                {
                    hasWeapons[2] = false;
                    weapons[2].SetActive(false);
                }

                hasWeapons[weaponIndex] = true;
                weapons[weaponIndex].SetActive(true);

                Destroy(nearObject);
            }
        }
    }


}
