using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject firePosition;
    public GameObject bombFactory;

    public static PlayerFire instance;

    public float throwPower = 15f;

    private void Start() {
        instance = this;
    }

    public void Shooting(float power)
    {
<<<<<<< Updated upstream
        throwPower = power * 30f;
        GameObject bomb = Instantiate(bombFactory);
        bomb.transform.position = firePosition.transform.position;
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        Vector3 newDirection = Quaternion.AngleAxis(-30, transform.right) * transform.forward;
        rb.AddForce(newDirection*throwPower, ForceMode.Impulse);
=======
        DrawParabola();
        if (Input.GetMouseButtonUp(0))
        {
            
            
            
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            Vector3 throwDirection = firePosition.transform.forward.normalized;
            rb.AddForce(throwDirection * throwPower*powerGage.powerValue, ForceMode.Impulse);
            lineRenderer.enabled=false;
            
        }
        
>>>>>>> Stashed changes
    }
}
