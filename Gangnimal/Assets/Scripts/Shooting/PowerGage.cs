using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PowerGage : MonoBehaviour
{
    private float clickTime = 0;
    public float powerValue = 0;
    private bool isClick = false;
    private float maxClickTime = 1f;
    private bool timeUp = true;
    Transform fire;
    Slider powerSlider;

    NetworkObject player;
    // Start is called before the first frame update

    private void Awake() {
        StartCoroutine("WaitingForObject");
    }
    void Start()
    {
        powerSlider = GameObject.Find("Canvas").GetComponent<Slider>();
    }

    IEnumerator WaitingForObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            if(NetworkManager.Singleton.LocalClient.PlayerObject != null)
            {
                player = NetworkManager.Singleton.LocalClient.PlayerObject;
                fire = player.transform.GetChild(2).GetComponent<Transform>();
                StopCoroutine("WaitingForObject");
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        ChargingGage();
    }

    IEnumerator Wait_Change()
    {
        yield return new WaitForSeconds(0.3f);
        Vector3 currentRotation = fire.rotation.eulerAngles;
        fire.transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
    }
    IEnumerator ResetClickTime()
    {
        yield return new WaitForSeconds(0.5f);
        clickTime = 0;
    }
    void ChargingGage()
    {
        if (Input.GetMouseButtonDown(0))

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
            if (clickTime >= maxClickTime)
            {
                timeUp = false;
            }
            else if (clickTime <= 0)
            {
                timeUp = true;
            }
            if (timeUp)
            {
                clickTime += Time.deltaTime;


                fire.Rotate(clickTime * -60 * Time.deltaTime, 0, 0);

            }
            else
            {
                clickTime -= Time.deltaTime;

                fire.Rotate(clickTime * 60 * Time.deltaTime, 0, 0);

            }
            //Debug.Log(clickTime);
        }
        else
        {
            StartCoroutine(ResetClickTime());

            if(fire!=null)
            {
                fire.Rotate(clickTime * 30 * Time.deltaTime, 0, 0);
            }

            StartCoroutine(Wait_Change());
        }

        if (powerSlider != null)
        {
            powerValue = clickTime / maxClickTime;
            powerSlider.value = powerValue;
        }
    }
}