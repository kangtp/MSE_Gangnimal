using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : MonoBehaviour
{
 
    public void BackButton()
    {
        GameObject.Find("Canvas").transform.Find("OptionUI").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("MainUI").gameObject.SetActive(true);
    }
}
