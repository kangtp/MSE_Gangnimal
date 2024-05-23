using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤 인스턴스

    public bool isGameOver = false; // 게임 오버 상태

    public GameObject gameOverPannel;

    private void Awake()
    {
        gameOverPannel.SetActive(false);
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 게임 오버 처리
    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            gameOverPannel.SetActive(true);
            Debug.Log("Game Over!");
            Cursor.visible= true;
            Cursor.lockState=CursorLockMode.Confined;
        }
    }

    
}
