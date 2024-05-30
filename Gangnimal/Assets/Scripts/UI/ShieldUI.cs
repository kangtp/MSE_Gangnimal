using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldUI : MonoBehaviour
{
    public Image shield;
    private PlayerInfo playerinfo;

    void Start()
    {
        playerinfo = FindObjectOfType<PlayerInfo>();

        shield.gameObject.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {   
        if(playerinfo != null && playerinfo.haveShield){
            shield.gameObject.SetActive(true);
        }
        else if(playerinfo != null && playerinfo.haveShield == false){
            shield.gameObject.SetActive(false);

        }
    }
}
