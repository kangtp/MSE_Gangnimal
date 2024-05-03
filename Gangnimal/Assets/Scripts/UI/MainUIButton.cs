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
        //option window
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
