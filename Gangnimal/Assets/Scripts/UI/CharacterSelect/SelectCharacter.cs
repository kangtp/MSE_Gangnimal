using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SelectCharacter : MonoBehaviour //캐릭터 선택씬 
{
    private List<GameObject> models; // 화면에 띄어져 있는 캐릭터 모델들 
    private int select_index; // 선택 번호 1. 곰 2. 말 3. 토끼
    private GameObject[] explainPannels; // 캐릭터 설명란
    
    // Start is called before the first frame update
    void Awake()
    {
        
    }
    void Start()
    {
        select_index=0;//초기값은 곰으로 설정
        PlayerPrefs.SetInt("SelectedCharacterIndex", select_index); // 초기값 저장
        models =new List<GameObject>(); // public으로 받아오기 싫어서 찾아줄려고 리스트 만듬. 
        foreach(Transform t in transform) // 이 게임오브젝트의 자식들이 캐릭터 모델들 
        {// 찾아주기 위해 전부 씬에선 true임 그리고 모델 리스트에 넣어주고 false시킴.
            models.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }
        models[select_index].SetActive(true); // 그리고 선택 모델들만 true
        explainPannels = GameObject.FindGameObjectsWithTag("Explain"); // 캐릭터 설명 텍스트들 배열로 받아옴. 
        explainPannels = ChangeSepuence(explainPannels); // 태그로 받아오면 순서 뒤죽박죽되서 정렬하기 위한 함수 
        foreach(GameObject pannel in explainPannels) // 이것도 같은 이치 아닌 것들은 꺼준다. 
        {
            pannel.SetActive(false);
        }
        explainPannels[select_index].SetActive(true);        
    }
    public void Select(int index) // 캐릭터 선택 버튼을 누르면 그 이외의 다른 것들은 줘야함. 버튼마다 번호 배정해줌.
    {
        if(index == select_index) // 버튼 두번 누르면 그냥 바로 종료
        {
            return;
        }
        if(index <0 || index>models.Count)
        {
            return;
        } // 이건 혹시 모를 오류 시 바로 종료하기 위함. 
        models[select_index].SetActive(false); // 기존꺼는 안보이게 해야함 
        explainPannels[select_index].SetActive(false); // 같은 이치
        select_index=index;// 버튼 번호 값으로 select 변수를 바꿔준다.
        models[select_index].SetActive(true); // 선택된 애들 키기 
        explainPannels[select_index].SetActive(true);
        
        PlayerPrefs.SetInt("SelectedCharacterIndex", select_index);// 그리고 전투씬에서의 스폰을 위한 정보값 저장
        //GameManager.instance.PlayCharacterSound(select_index);// 버튼 클릭시 사운드 넣기
        PlayerPrefs.Save(); // 저장해주기
        
    }
    public GameObject[] ChangeSepuence(GameObject[] pannels) // 이건 findtag로 오브젝트들을 가져오면 순서가 뒤죽박죽되서 정렬하기 위한 함수
    {
        GameObject[] correctOrder = new GameObject[3];// 캐릭터 설명이 3가지이니 배열도 3
        foreach(GameObject p in pannels)// 다시 생각해보면 텍스트 하나만 만들고 캐릭터 명에 따라서 텍스트값만 바꿔줘도 되긴할 듯 근데 일단은 보류
        {
            if(p.name=="BearExplain")
            {
                correctOrder[0] = p;
            }
            else if(p.name=="HorseExplain")
            {
                correctOrder[1]=p;
            }
            else if(p.name=="RabbitExplain")
            {
                correctOrder[2]=p;
            }
        }
       
       return correctOrder;
    }    
    
}
