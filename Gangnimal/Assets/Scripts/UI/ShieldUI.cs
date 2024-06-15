using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldUI : MonoBehaviour
{
    public GameObject shield;
    private PlayerInfo playerinfo;

    void Start()
    {
        playerinfo = FindObjectOfType<PlayerInfo>();
        GameObject.Find("Canvas").transform.GetChild(5).gameObject.SetActive(true);
        shield = GameObject.Find("StateUI").transform.GetChild(2).gameObject;

        if (playerinfo == null)
        {
            Debug.Log("PlayerInfo not yet");
        }

        if (shield == null)
        {
            Debug.Log("Shield not yet");
        }
    }
    public void ShieldOn()
    {
        shield.SetActive(true);
    }

    public void ShieldOff()
    {
        shield.SetActive(false);
    }
}
