using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private Button startButton;
    public int endTime;
    private float currentTime;

    private bool oneTime;

    // Start is called before the first frame update
    void Start()
    {
        oneTime = false;
        currentTime = endTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (LobbyManager.Instance.IsLobbyHost())
        {
            if (LobbyManager.Instance.StartCondition() == false)
            {
                timerText.text = "waiting for user";
            }
            else
            {
                Timer_Show();
            }
        }
        else if (LobbyManager.Instance.IsLobbyHost() == false)
        {
            Timer_Show();
        }
    }
    void Timer_Show()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 5)
            {
                timerText.color = Color.red;
            }
        }
        else
        {
            if (!oneTime && LobbyManager.Instance.IsLobbyHost())
            {
                currentTime = 0;
                LobbyManager.Instance.StartGame();
                oneTime = true;
            }
            //startButton.onClick.Invoke();
        }

        timerText.text = Mathf.CeilToInt(currentTime).ToString();
    }
}
