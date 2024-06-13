using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤 인스턴스

    public bool isGameOver = false; // 게임이 안끝난 상태 , true가 게임이 끝난상태 
    private bool isAlive=true;// 누군가 오버이지만 자신은 살아있다면 이긴거임

    //public GameObject gameOverPannel; // 게임오버 패널
    public GameObject winPannel=null; // 게임오버 패널
    public GameObject losePannel=null;
    private TextMeshProUGUI overText; // 승패를 알려줄 텍스트 
    
    // 각종 사운드 
    public AudioClip jumpSound;
    public AudioClip deathSound;
    public AudioClip getHPSound;
    public AudioClip getShieldSound;
    public AudioClip explosionSound1;
    public AudioClip explosionSound2;
    public AudioClip explosionSound3;
    public AudioClip[] characterSounds; // 캐릭터 선택창 사운드 배열 
    [HideInInspector] public AudioSource audioSource; // 오디오 소스
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

        LoadAudioSettings();// 설정창에서 설정한 볼륨값을 가져온다.
        LoadMouseSettings();// 설정값에서 설정한 마우스 감도값을 가져온다. 

        
    }
    void Start()
    {
        //isGameOver=false; // 이것을 다시 해준이유는 GameManager가 don't destroy로 있기때문에 메인메뉴로 돌아가면 다시 업데이트를 해줘야 한다. 
        //isAlive=true; // 위와 같은 이유
        //if(SceneManager.GetActiveScene().name =="ForestScene" || SceneManager.GetActiveScene().name =="Winter" || SceneManager.GetActiveScene().name =="Desert")
        //{// 전투씬인 경우에만 over 패널을 쓰기 때문에 조건문을 넣어줌. 
        //    winPannel = GameObject.Find("Canvas").transform.GetChild(6).gameObject;
        //    losePannel = GameObject.Find("Canvas").transform.GetChild(7).gameObject;
        //    if (winPannel != null && losePannel!=null)
        //    {
        //        winPannel.SetActive(false); // 만약에 찾았다면 처음에는 이 패널이 꺼져있어야 한다.
        //        losePannel.SetActive(false);
        //    }
        //    isGameOver=false;//혹시 모르니 한번 더 설정 해줌.
        //    isAlive=true;// 막 시작했으니 캐릭터는 전부 살아있어야 하기 때문에 설정 해줌 
        //}
    }
    // 게임 오버 처리
    public void GameOver()
    {
        if (!isGameOver) // 게임오버 중복실행을 막기위해 딱 한번만 실행되어야함. 
        {
            Cursor.visible = true; // 게임이 끝난 순간 버튼을 클릭하기 위해 커서를 보이게 한다. 
            Cursor.lockState = CursorLockMode.Confined;// 잠금을 푼다.
            isGameOver = true; // 게임오버 true
            
            
            if(isAlive)// 게임오버 패널이 켜져있지만 살아있다면 
            {
                winPannel.SetActive(true);
               
            }
            else
            {
                losePannel.SetActive(true);
            
            }
        }
    }
    public void InitializeGameOverPanel()// 혹시라도 못찾을 경우를 대비해 다른 오브젝트에서도 게임 패널을 찾아준다. 
    {

        isGameOver = false; // 이것을 다시 해준이유는 GameManager가 don't destroy로 있기때문에 메인메뉴로 돌아가면 다시 업데이트를 해줘야 한다. 
        isAlive = true; // 위와 같은 이유
        if (SceneManager.GetActiveScene().name == "ForestScene" || SceneManager.GetActiveScene().name == "Winter" || SceneManager.GetActiveScene().name == "Desert")
        {// 전투씬인 경우에만 over 패널을 쓰기 때문에 조건문을 넣어줌. 
            winPannel = GameObject.Find("Canvas").transform.GetChild(7).gameObject;
            losePannel = GameObject.Find("Canvas").transform.GetChild(6).gameObject;
            if (winPannel != null && losePannel != null)
            {
                winPannel.SetActive(false); // 만약에 찾았다면 처음에는 이 패널이 꺼져있어야 한다.
                losePannel.SetActive(false);
            }
            isGameOver = false;//혹시 모르니 한번 더 설정 해줌.
            isAlive = true;// 막 시작했으니 캐릭터는 전부 살아있어야 하기 때문에 설정 해줌 


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

    public void SaveAudioSettings(float volume)//볼륨 세팅값을 재설정해준다. ex) 슬라이더로 볼륨 설정할때
    {
        PlayerPrefs.SetFloat("AudioVolume", volume);
        PlayerPrefs.Save();
        audioSource.volume = volume;
    }

    public void LoadAudioSettings()//이전 씬에서 가져온 볼륨값 정보들을 연동해준다. 
    {
        if (PlayerPrefs.HasKey("AudioVolume"))//PlayerPrefeb은 게임이 실행되고 씬이 바뀌더라도 정보들이 저장되게 해주는 것 
        { //hasKey는 내가 설정한 "AudioVolume" 이라는 변수가 있는지 없는지 검사해주는것 지금은 있다는 것!
            float volume = PlayerPrefs.GetFloat("AudioVolume"); //있다면 볼륨값을 가져와서 다른 씬에도 볼륨값을 재설정해준다.
            audioSource.volume = volume;
        }
        else // 없다면 1.0값을 준다. 
        {
            audioSource.volume = 1.0f;
        }
    }

    // 마우스 감도 설정 저장
    public void SaveMouseSettings(float sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);//마우스센스티비티라는 플레이어 프리펩 변수를 만들어 주는거
        PlayerPrefs.Save();// Save는 playerPrefebs의 설정된 값을 저장해준다. 
        mouseSensitivity = sensitivity;// 그리고 값을 설정해준다. 
    }

    
    public void LoadMouseSettings() // 마우스 감도 값 설정을 가져온다. 
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
    public void SetAlive(bool alive)
    {
        isAlive = alive;
    }

    
    public void PlayCharacterSound(int characterIndex) // 이건 캐릭터 선택할때 음성 띄어주는거! 
    {
        if (characterIndex >= 0 && characterIndex < characterSounds.Length)
        {
            audioSource.PlayOneShot(characterSounds[characterIndex]);
        }
    }
}