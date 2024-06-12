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
            Debug.LogError("PlayerInfo를 찾을 수 없습니다.");
        }

        if (shield == null)
        {
            Debug.LogError("Shield 이미지가 연결되지 않았습니다.");
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
            playerinfo = FindObjectOfType<PlayerInfo>();
            shield.gameObject.SetActive(false);

        }
    }
}
