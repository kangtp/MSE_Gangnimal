using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public void ToNext()
    {
        switch (PlayerPrefs.GetInt("SelectedMapIndex"))
        {
            case 0 :
                SceneManager.LoadScene("ForestScene");
                break;
            case 1 :
                SceneManager.LoadScene("Desert");
                break;
            case 2 :
                SceneManager.LoadScene("Winter");
                break;
            
                
        }
    }
    public void ToCharacterSelect()
    {
        SceneManager.LoadScene("CharacterSelect");
    }
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
