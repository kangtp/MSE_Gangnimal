using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldUI : MonoBehaviour
{
    public GameObject shield;

    void Start()
    {
        GameObject.Find("Canvas").transform.GetChild(5).gameObject.SetActive(true);
        shield = GameObject.Find("StateUI").transform.GetChild(2).gameObject;
    }

    //Turn on the shield image in UI
    public void ShieldOn()
    {
        shield.SetActive(true);
    }

    //Turn off the shield image in UI
    public void ShieldOff()
    {
        shield.SetActive(false);
    }
}
