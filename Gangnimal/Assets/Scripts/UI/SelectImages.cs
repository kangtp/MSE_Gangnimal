using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectImages : MonoBehaviour //이건 맵선택창때 뒤에 배경 보여주는 클래스 
{
    private List<GameObject> images; // 배경 이미지들
    private int select_index=0; // 초기값은 0 
    [SerializeField]
    private Sprite[] sprites; // 보여줄 이미지 스프라이트들
    private Image background; // 뒷배경화면
    public GameObject[] characterSelects;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("SelectedMapIndex", select_index);// 초기값 저장
        images =new List<GameObject>();// 중간에 띄어져있는 맵사진들
        foreach(Transform t in transform)// 이게임오브젝트의 자식들에 존재하니 넣어준다. 
        {
            images.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }
        background = GameObject.Find("BackGround").GetComponent<Image>();// 뒤에 배경사진

        
        images[select_index].SetActive(true); //초기에는 0번째 인덱스 보여줘야하니깐
        background.sprite = sprites[select_index];//배경화면
    }
    public void MapSelect(int index)// 맵 선택 버튼들에 들어가는 함수들 버튼마다 번호 존재 0 1 2 
    {
        if(index == select_index) // 버튼 중복클릭시 바로 종료해도 괜찮다. 
        {
            return;
        }
        if(index <0 || index>images.Count) // 혹시모를 오류 시 그냥 기존 값으로 가져가게함.
        {
            return;
        }
        images[select_index].SetActive(false); // 기존 이미지 끄고
        select_index=index;
        images[select_index].SetActive(true);// 새로운 이미지 보여주고 
        background.sprite = sprites[select_index];
        PlayerPrefs.SetInt("SelectedMapIndex", select_index); // 다음씬에 가져가기 위해 playerprefebs에 저장해준다. 
        PlayerPrefs.Save();
    }
  
}
