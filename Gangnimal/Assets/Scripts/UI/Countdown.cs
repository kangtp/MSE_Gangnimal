using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [SerializeField] float setTime = 15.0f;
    [SerializeField] Text countdownText;
    private PlayerInfo playerinfo;

    // Start is called before the first frame update
    void Start()
    {
        countdownText.text = setTime.ToString();
        playerinfo = FindObjectOfType<PlayerInfo>();


    }

    // Update is called once per frame
    void Update()
    {   
        //timer reset
        if(!playerinfo.myTurn && setTime <= 0){
            setTime = 15.0f;
        }

        if(playerinfo != null && playerinfo.myTurn){
            if(setTime > 0){
                setTime -= Time.deltaTime;
                countdownText.text = Mathf.Round(setTime).ToString();
            }
            else if(setTime <= 0){
                playerinfo.myTurn = false;
                setTime = 0;
                countdownText.text = "Time Over!";
            }

        }
    }
}
