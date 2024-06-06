using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤 인스턴스

    public bool isGameOver = false; // 게임 오버 상태

    public GameObject gameOverPannel;

    public AudioClip jumpSound;
    public AudioClip deathSound;
    public AudioClip getHPSound;
    public AudioClip getShieldSound;
    public AudioClip explosionSound1;
    public AudioClip explosionSound2;
    public AudioClip explosionSound3;

    public AudioClip[] characterSounds; // 캐릭터 사운드 배열
    [HideInInspector] public AudioSource audioSource;

    [HideInInspector] public float mouseSensitivity = 1.0f; // 마우스 감도

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

        LoadAudioSettings();
        LoadMouseSettings();

        
    }
    void Start()
    {
        gameOverPannel=GameObject.Find("GameOver");
        if (gameOverPannel != null)
        {
            gameOverPannel.SetActive(false);
        }
        isGameOver=false;
    }
    // 게임 오버 처리
    public void GameOver()
    {
        
        if (!isGameOver)
        {
            isGameOver = true;
            gameOverPannel.SetActive(true);
            
            Debug.Log("Game Over!");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            PlayDeathSound();
        }
    }
    public void InitializeGameOverPanel()
    {
        gameOverPannel = GameObject.Find("GameOver");
        if (gameOverPannel != null)
        {
            gameOverPannel.SetActive(false);
        }
    }

    // 오디오 관련 메소드
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

    public void SaveAudioSettings(float volume)
    {
        PlayerPrefs.SetFloat("AudioVolume", volume);
        PlayerPrefs.Save();
        audioSource.volume = volume;
    }

    public void LoadAudioSettings()
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

    // 마우스 감도 설정 저장
    public void SaveMouseSettings(float sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
        PlayerPrefs.Save();
        mouseSensitivity = sensitivity;
    }

    
    public void LoadMouseSettings()
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

    
    public void PlayCharacterSound(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < characterSounds.Length)
        {
            audioSource.PlayOneShot(characterSounds[characterIndex]);
        }
    }
}
