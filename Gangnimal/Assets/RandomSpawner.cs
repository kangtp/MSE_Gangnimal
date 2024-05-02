using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject[] objects;


    public int spawnNumber;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            for (int j = 0; j < spawnNumber; j++) //한 오브젝트 당 생성할 개수
            {
                Vector3 randomSpawnPosition = new Vector3(Random.Range(-10, 30), 3, Random.Range(0, 50));
                Instantiate(objects[i], randomSpawnPosition, Quaternion.identity);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
}
