using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singletone instance

    public bool isGameOver = false; // Game over state true : game end false : gaming
    private bool isAlive=true;// player alive state

    //public GameObject gameOverPannel; // 게임오버 패널
    public GameObject winPannel=null; // gameover panel
    public GameObject losePannel=null;
   
    
    // Sound Effects 
    public AudioClip jumpSound;
    public AudioClip deathSound;
    public AudioClip getHPSound;
    public AudioClip getShieldSound;
    public AudioClip explosionSound1;
    public AudioClip explosionSound2;
    public AudioClip explosionSound3;
    public AudioClip[] characterSounds; 
    [HideInInspector] public AudioSource audioSource; // audio source
    [HideInInspector] public float mouseSensitivity = 1.0f; // mouse sensitivity

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        LoadAudioSettings();// load sound setting.
        LoadMouseSettings();// load mouse setting. 

        
    }
    void Start()
    {
       
    }
    //GameOver Fuction
    public void GameOver()
    {
        if (!isGameOver) // when player Dead 
        {
            Cursor.visible = true; // visible cursor
            Cursor.lockState = CursorLockMode.Confined;// unlocked cursor.
            isGameOver = true; // Game over is true game end
            
            
            if(isAlive)// When player alive 
            {
                winPannel.SetActive(true);//win
               
            }
            else
            {
                losePannel.SetActive(true);//false
            }
            TestRelay.Instance.EndGame();
        }
    }
    public void InitializeGameOverPanel()// Initialize panel because Don't be destroy load is just execute {start,awake} once
    {

        isGameOver = false; // initialize parmeters 
        isAlive = true; // initialize parmeters 
        if (SceneManager.GetActiveScene().name == "IngameScene")
        {// when in InGameScene , use over panel 
            winPannel = GameObject.Find("Canvas").transform.GetChild(7).gameObject;
            losePannel = GameObject.Find("Canvas").transform.GetChild(6).gameObject;
            if (winPannel != null && losePannel != null)
            {
                winPannel.SetActive(false); // when find then set active false
                losePannel.SetActive(false);
            }
            isGameOver = false;//Set twice as important variable value
            isAlive = true;// Set twice as important variable value 
        }
    }

    // Audio methods
    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound);
    }
    public void PlayShieldSound()
    {
        audioSource.PlayOneShot(getShieldSound);
    }
    public void PlayExplosionSound1()
    {
        audioSource.PlayOneShot(explosionSound1);
    }
    public void PlayExplosionSound2()
    {
        audioSource.PlayOneShot(explosionSound2);
    }
    public void PlayExplosionSound3()
    {
        audioSource.PlayOneShot(explosionSound3);
    }
    public void PlayHpSound()
    {
        audioSource.PlayOneShot(getHPSound);
    }

    public void SaveAudioSettings(float volume)//Setting volume by slider
    {
        PlayerPrefs.SetFloat("AudioVolume", volume);
        PlayerPrefs.Save();
        audioSource.volume = volume;
    }

    public void LoadAudioSettings()//apply setting audio
    {
        if (PlayerPrefs.HasKey("AudioVolume"))
        { 
            float volume = PlayerPrefs.GetFloat("AudioVolume");
            audioSource.volume = volume;
        }
        else 
        {
            audioSource.volume = 1.0f;
        }
    }

    // Save senstivity
    public void SaveMouseSettings(float sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
        PlayerPrefs.Save(); 
        mouseSensitivity = sensitivity;
    }

    
    public void LoadMouseSettings()  //Load Mouse sensitivity
    {
        if (PlayerPrefs.HasKey("MouseSensitivity"))
        {
            mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
        }
        else
        {
            mouseSensitivity = 1.0f; 
        }
    }
    public void SetAlive(bool alive) //Change alive state function
    {
        isAlive = alive;
    }

    
    public void PlayCharacterSound(int characterIndex) // sound method when Characet select
    {
        if (characterIndex >= 0 && characterIndex < characterSounds.Length)
        {
            audioSource.PlayOneShot(characterSounds[characterIndex]);
        }
    }
}