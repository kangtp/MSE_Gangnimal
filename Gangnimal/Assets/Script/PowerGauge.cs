using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerGage : MonoBehaviour
{
    private float clickTime = 0;
    public float powerValue = 0;
    private bool isClick = false;
    private float maxClickTime = 1f;
    private bool timeUp = true;
    static public PowerGage instance;

    public Slider powerSlider;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isClick = true;
            Debug.Log("clicked");
        }

        if (Input.GetMouseButtonUp(0))
        {
            isClick = false;
            Debug.Log("Release");
        }

        if (isClick)
        {
            if(clickTime >= maxClickTime)
            {
                timeUp = false;
            }
            else if(clickTime <= 0)
            {
                timeUp = true;
            }
            if (timeUp)
            {
                clickTime += Time.deltaTime;
            }
            else
            {
                clickTime -= Time.deltaTime;
            }
            //Debug.Log(clickTime);
        }
        else
        {
            clickTime = 0;
        }

        if(powerSlider != null)
        {
            powerValue = clickTime / maxClickTime;
            powerSlider.value = powerValue;
        }
    }
}
