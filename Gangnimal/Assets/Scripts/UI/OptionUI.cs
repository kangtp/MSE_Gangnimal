using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider sensitivitySlider;

    private void Start()
    {
        
        if (GameManager.instance != null)
        {
            volumeSlider.value = GameManager.instance.audioSource.volume;
            sensitivitySlider.value = GameManager.instance.mouseSensitivity;
        }
        else
        {
            volumeSlider.value = 1.0f;
            sensitivitySlider.value = 1.0f;
        }

        volumeSlider.onValueChanged.AddListener(SetVolume);
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
    }

    public void SetVolume(float volume)
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.SaveAudioSettings(volume);
        }
    }

    public void SetSensitivity(float sensitivity)
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.SaveMouseSettings(sensitivity);
        }
    }

    public void BackButton()
    {
        GameObject.Find("Canvas").transform.Find("OptionUI").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("MainUI").gameObject.SetActive(true);
    }
}
