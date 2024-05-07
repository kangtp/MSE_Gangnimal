using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject firePosition;
    public GameObject bombFactory;
    public PowerGage powerGage;
    public float throwPower;
    

    //line
    [SerializeField]
    public LineRenderer lineRenderer;
    public int numofDot;
    public float timeInterval;
    public float maxTime;

    private void Start() {
       
    }

   void Update()
    {
        ShootingBullet();
    }
   
    
    void DrawParabola()
    {
        lineRenderer.enabled=true;
        Vector3[] points = new Vector3[numofDot];
        Vector3 startPosition = firePosition.transform.position;
        Vector3 startVelocity = throwPower * firePosition.transform.forward;

        
        float timeInterval = maxTime / numofDot;
        for (int i = 0; i < numofDot; i++)
        {
            float t = i * timeInterval;
            points[i] = startPosition + startVelocity * t + 0.5f * Physics.gravity * t * t;
        }

        
        lineRenderer.positionCount = numofDot;
        lineRenderer.SetPositions(points);
        for(int i=0;i<numofDot;i++)
        {
            lineRenderer.SetPosition(i,points[i]);
        }
    }
    void ShootingBullet()
    {
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
    }


}