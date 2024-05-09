using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerGauge : MonoBehaviour
{
    private float clickTime = 0;
    public float powerValue = 0;
    private bool isClick = false;
    private float maxClickTime = 1f;
    private bool timeUp = true;
    static public PowerGauge instance;
    public Transform fire;

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
                fire.Rotate(clickTime*-40*Time.deltaTime,0,0);
            }
            else
            {
                clickTime -= Time.deltaTime;
                fire.Rotate(clickTime*40*Time.deltaTime,0,0);
            }
            //Debug.Log(clickTime);
        }
        else
        {

            
            StartCoroutine(WaitSecond());
            fire.Rotate(clickTime*30*Time.deltaTime,0,0);
            StartCoroutine(Wait());
        }

        if(powerSlider != null)
        {
            powerValue = clickTime / maxClickTime;
            powerSlider.value = powerValue;
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);
        Vector3 currentRotation = fire.transform.rotation.eulerAngles;
        fire.transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
    }
    IEnumerator WaitSecond()
    {
        yield return new WaitForSeconds(0.5f);
        clickTime = 0;
    }

}
