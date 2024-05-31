using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawnerNew : MonoBehaviour
{
     public GameObject[] characterPrefabs;
     public Transform spawnPoint;

    // Start is called before the first frame update
    private void Awake() {
        
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        

       
        if (selectedIndex >= 0 && selectedIndex < characterPrefabs.Length)
        {
            GameObject newCharacter=Instantiate(characterPrefabs[selectedIndex]);
            newCharacter.transform.position = spawnPoint.position;

            Debug.Log("Character is " + selectedIndex);
        }
        else
        {
            Debug.Log(selectedIndex);
            Debug.LogError("잘못된 선택된 캐릭터 인덱스: " + selectedIndex);
        }

    }
    void Start()
    {
        GameManager.instance.InitializeGameOverPanel();
        if(GameManager.instance.GameOver)
        {
            Debug.Log("panel ");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
