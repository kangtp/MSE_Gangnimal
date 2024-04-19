using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class PlayerInfo : MonoBehaviour
{
    public int myHP = 100;
    public float movingTime = 10.0f;
    public bool myTurn = true;

    //public gameObject myWeapon;

    public bool haveShield = false;
    


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Weapon")
        { 
            other.gameObject.SetActive(false); 

        }
        else if(other.tag == "Item")
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
            other.gameObject.SetActive(false);

        }

    }
}
