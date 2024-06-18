using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour // Classe that collect button functions
{
    [SerializeField] List<GameObject> characters; // When CharacterSelect , just set false character object
    void Start()
    {
        if(gameObject.name == "MapSelectCanvas")
        {
                foreach(GameObject gameObjectcharacter in characters)
                {
                    gameObjectcharacter.SetActive(false);
                }
            
        }
       
    }
    
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
    public void Characters() // Just SetActicve False because hide object
    {
        foreach(GameObject characgers in characters)
        {
            characgers.SetActive(true);
        }
        this.transform.gameObject.SetActive(false);
    }
    
    
    public void ToMainMenu()
    {
        TestRelay.Instance.EndGame();
    } 
    public void ToMainMenuToLooby()
    {
        
        SceneManager.LoadScene("ForestScene");

    }
    public void ToLoginScene()
    {
        SceneManager.LoadScene("LoginScene");
    }
}
