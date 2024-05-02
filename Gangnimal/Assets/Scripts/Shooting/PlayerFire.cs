using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject firePosition;
    public GameObject bombFactory;


    public float throwPower = 15f;

    PlayerInfo playerInfo;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            throwPower = PowerGage.instance.powerValue * 30f;

            /*playerInfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
            
            for(int i = 0; i < 3; i++)
            {
                if(playerInfo.hasWeapons[i] == true)
                {

                }

            }*/

            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            Vector3 newDirection = Quaternion.AngleAxis(-30, transform.right) * transform.forward;
            rb.AddForce(newDirection*throwPower, ForceMode.Impulse);
        }
    }
}
