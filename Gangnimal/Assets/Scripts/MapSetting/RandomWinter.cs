using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWinter : MonoBehaviour
{
    public GameObject[] objects;
    public int spawnNumber;
    // Start is called before the first frame update
    void Start()
    {
        SpawnItem();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void SpawnItem()
    {
        for(int i = 0; i < 5; i++)
        {
            for (int j = 0; j < spawnNumber; j++) 
            {
                //Specify the area in which the item is to be randomly spawned for Winter Map
                Vector3 randomSpawnPosition = new Vector3(Random.Range(-50, 0), 13, Random.Range(-10, 40));
                Instantiate(objects[i], randomSpawnPosition, Quaternion.identity);
            }
        }
    }
}
