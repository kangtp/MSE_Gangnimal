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

    // Start is called before the first frame update
    void Start()
    {
        currentTime = endTime;
    }

    // Update is called once per frame
    void Update()
    {
        Timer_Show();
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
            currentTime = 0;
            
            startButton.onClick.Invoke();
        }

        timerText.text = Mathf.CeilToInt(currentTime).ToString();
    }
}
