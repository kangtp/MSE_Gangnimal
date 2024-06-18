using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour // Timer class
{
    [SerializeField]
    private TextMeshProUGUI timerText; // timer text
    [SerializeField]
    private Button startButton; // start button
    public int endTime; // endtime
    private float currentTime;//current time

    private bool oneTime; // becasuse just click one time

    // Start is called before the first frame update
    void Start()
    {
        oneTime = false;
        currentTime = endTime;
        timerText.text = "waiting for user";
    }

    // Update is called once per frame
    void Update()
    {
        if (LobbyManager.Instance.IsLobbyHost()) 
        {
            if (LobbyManager.Instance.StartCondition() == false)// when client not come in room
            {
                timerText.text = "waiting for user";
            }
            else if(LobbyManager.Instance.StartCondition()) // when come client in room 
            {                
                Timer_Show(); //update timer
            }
        }
        else if (LobbyManager.Instance.IsLobbyHost() == false)
        {
            Timer_Show();
        }
    }
    void Timer_Show() //Show timer
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // update time
            if (currentTime < 5)
            {
                timerText.color = Color.red; // when current time is < 5 then color is red
            }
        }
        else // when time is 0 then start game
        {
            if (!oneTime && LobbyManager.Instance.IsLobbyHost())
            {
                currentTime = 0;
                LobbyManager.Instance.StartGame();
                oneTime = true;
            }
            //startButton.onClick.Invoke();
        }

        timerText.text = Mathf.CeilToInt(currentTime).ToString(); // update text
    }
}
