using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PowerGage : MonoBehaviour
{
    private float clickTime = 0;//Click time while click
    public float powerValue = 0; // slider value
    private bool isClick = false;
    private float maxClickTime = 1f; // max click time
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
        powerSlider = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<Slider>();
    }

    IEnumerator WaitingForObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            if(NetworkManager.Singleton.LocalClient.PlayerObject != null)
            {
                player = NetworkManager.Singleton.LocalClient.PlayerObject;
                fire = player.transform.GetChild(0).GetComponent<Transform>();
                StopCoroutine("WaitingForObject");
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        ChargingGage();
    }

    IEnumerator Wait_Change()//The mouse is placed when it is launched to prevent it from becoming zero
    {
        yield return new WaitForSeconds(0.3f);
        if(fire != null)
        {
        Vector3 currentRotation = fire.rotation.eulerAngles;
        fire.transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
        }
    }
    IEnumerator ResetClickTime() 
    {
        yield return new WaitForSeconds(0.5f);
        clickTime = 0;
    }
    void ChargingGage()
    {
        // Check if the left mouse button is pressed down.
        if (Input.GetMouseButtonDown(0))
        {
            isClick = true; // Set the click state to true.
        }

        // Check if the left mouse button is released.
        if (Input.GetMouseButtonUp(0))
        {
            isClick = false; // Set the click state to false.
        }

        // If the mouse button is held down.
        if (isClick)
        {
            // Determine if the charge time has reached its maximum.
            if (clickTime >= maxClickTime)
            {
                timeUp = false; // Stop increasing the time.
            }
            // Determine if the charge time has reached zero.
            else if (clickTime <= 0)
            {
                timeUp = true; // Start increasing the time.
            }

            // If the charge time should increase.
            if (timeUp)
            {
                clickTime += Time.deltaTime; // Increase the click time.

                // Rotate the 'fire' object based on the increasing click time.
                fire.Rotate(clickTime * -60 * Time.deltaTime, 0, 0);
            }
            // If the charge time should decrease.
            else
            {
                clickTime -= Time.deltaTime; // Decrease the click time.

                // Rotate the 'fire' object based on the decreasing click time.
                fire.Rotate(clickTime * 60 * Time.deltaTime, 0, 0);
            }
            // Debugging statement to show the current click time.
            // Debug.Log(clickTime);
        }
        else // If the mouse button is released.
        {
            // Start coroutine to reset the click time.
            StartCoroutine(ResetClickTime());

            // If the 'fire' object is not null, apply a rotation.
            if (fire != null)
            {
                fire.Rotate(clickTime * 30 * Time.deltaTime, 0, 0);
            }

            // Start coroutine to wait and then change the state or perform an action.
            StartCoroutine(Wait_Change());
        }

        // If the power slider UI component exists.
        if (powerSlider != null)
        {
            powerValue = clickTime / maxClickTime; // Calculate the current power value as a ratio.
            powerSlider.value = powerValue; // Update the slider to reflect the current power value.
        }
    }

}