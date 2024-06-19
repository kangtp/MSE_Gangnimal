using UnityEngine;
using UnityEngine.UI;

public class LoadingSlider : MonoBehaviour
{
    public Slider slider; // Loading Slider
    public float loadTime = 5f; // Wating time
    public Button hostButton; // host Button
    private float timer = 0f;
    bool clicked = false;//Prevent Twice Click

    void Start()
    {
        // Initialize
        slider.value = 0f;

        //테스트용

    }

    void Update()
    {
        timer += Time.deltaTime;//Count time

        slider.value = Mathf.Clamp01(timer / loadTime); // Slider Gage up 

        if (timer > 1f && !clicked) //justOnce 
        {
            if (PlayerPrefs.GetInt("RelayInfo") == 0)
            {
                
                clicked = true;
            }
        }
        if (slider.value >= 1f) // when Slide is full
        {

            OnLoadingComplete();

        }
    }

    void OnLoadingComplete() // Set active false canvas
    {
        
        gameObject.SetActive(false);
    }
}
