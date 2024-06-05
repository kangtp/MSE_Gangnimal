using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] Text healthText;
    private PlayerInfo playerinfo;

    void Start()
    {
        playerinfo = FindObjectOfType<PlayerInfo>();

        healthText.text = playerinfo.HP.ToString();


    }

    // Update is called once per frame
    void Update()
    {   
        if(playerinfo != null && playerinfo.HP >= 0){
            healthText.text = Mathf.Round(playerinfo.HP).ToString();
        }
    }
}
