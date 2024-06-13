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


        if (playerinfo == null)
        {
            Debug.Log("PlayerInfo not created");
        }

        shield.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerinfo != null)
        {
            shield.gameObject.SetActive(playerinfo.haveShield);
        }
        else if (playerinfo == null)
        {
            
            shield.gameObject.SetActive(false);

        }

    }
}
