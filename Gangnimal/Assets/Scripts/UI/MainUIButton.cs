using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIButton : MonoBehaviour
{

    public void StartButton()
    {
        SceneManager.LoadScene("ForestScene");
    }

    public void OptionButton()
    {
        GameObject.Find("Canvas").transform.Find("MainUI").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("OptionUI").gameObject.SetActive(true);
    }

    public void SignUpButton()
    {
        GameObject.Find("Canvas").transform.Find("MainUI").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("SignUpUI").gameObject.SetActive(true);
    }

    public void RecordButton()
    {
        GameObject.Find("Canvas").transform.Find("MainUI").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("RecordUI").gameObject.SetActive(true);
        FindObjectOfType<AccountManager>().FindMyBattleRecord();
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void BackButtonFromOption()
    {
        GameObject.Find("Canvas").transform.Find("OptionUI").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("MainUI").gameObject.SetActive(true);
    }

    public void BackButtonFromSignUp()
    {
        GameObject.Find("Canvas").transform.Find("SignUpUI").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("MainUI").gameObject.SetActive(true);
    }

    public void BackButtonFromRecord()
    {
        GameObject.Find("Canvas").transform.Find("RecordUI").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("MainUI").gameObject.SetActive(true);
    }
}
