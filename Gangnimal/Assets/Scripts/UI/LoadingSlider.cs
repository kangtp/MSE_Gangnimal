using UnityEngine;
using UnityEngine.UI;

public class LoadingSlider : MonoBehaviour
{
    public Slider slider; // 로딩 슬라이드바
    public float loadTime = 5f; // 대기시간
    public Button hostButton;
    private float timer = 0f;
    bool clicked=false;//중복클릭 방지용으로 false

    void Start()
    {
        // 초기화
        slider.value = 0f;
        
        //테스트용
       
    }

    void Update()
    {
        timer += Time.deltaTime;
      
        slider.value = Mathf.Clamp01(timer / loadTime);

        if(timer>3f &&!clicked) // 딱 1번만 
        {
            hostButton.onClick.Invoke();
            clicked=true;
        }
        if (slider.value >= 1f)
        {

            OnLoadingComplete();
            
        }
    }

    void OnLoadingComplete()
    {
        // 캔바스 끄기
        gameObject.SetActive(false);
    }
}
