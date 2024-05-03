using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIButton : MonoBehaviour
{

    public void StartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OptionButton()
    {
        GameObject.Find("Canvas").transform.Find("MainUI").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("OptionUI").gameObject.SetActive(true);
    }
    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
