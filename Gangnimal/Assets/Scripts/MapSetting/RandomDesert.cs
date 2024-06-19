using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDesert : MonoBehaviour
{
    public GameObject[] objects;
    public int spawnNumber;
    public int[] spawnposition;
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
                //Specify the area in which the item is to be randomly spawned for Desert Map
                Vector3 randomSpawnPosition = new Vector3(Random.Range(-80, -10), 7, Random.Range(0, 40));
                GameObject spawnObject =Instantiate(objects[i], randomSpawnPosition, Quaternion.identity);
                spawnObject.transform.SetParent(gameObject.transform);
            }
        }
    }
}
