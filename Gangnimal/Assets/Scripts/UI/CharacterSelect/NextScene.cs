using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour // 씬넘어가는 함수들 모아넣은 클래스
{
    [SerializeField] List<GameObject> characters;
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
    public void Characters()
    {
        foreach(GameObject characgers in characters)
        {
            characgers.SetActive(true);
        }
        this.transform.gameObject.SetActive(false);
    }
    
    
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
  
}
