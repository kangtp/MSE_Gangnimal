using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Transform cam;
    float turnTime=.1f;
    float turnVelocity;
    Animator anim;
    bool sprinting;
    CharacterController controller;
    Vector2 movement;
    public float walkSpeed;
    public float sprintSpeed;
    float truespeed;
    public float turnspeed;

    public float jumpHeight;
    public float gravity;
    bool isGrounded;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
      truespeed = walkSpeed;
      controller = GetComponent<CharacterController>();
      anim = GetComponentInChildren<Animator>();
      Cursor.lockState=CursorLockMode.Locked;
      Cursor.visible=false;

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove3rd();
    }
    public void PlayerMove3rd()
    {
        isGrounded = Physics.CheckSphere(transform.position,.1f,1);
        anim.SetBool("isGround",isGrounded);
        float h = turnspeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, h, 0);
        if(isGrounded&&velocity.y<0)
        {
            velocity.y= -2;
        }
        

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            truespeed=sprintSpeed;
            sprinting = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            truespeed = walkSpeed;
            sprinting= false;
        }
        anim.transform.localPosition = Vector3.zero;
        anim.transform.localEulerAngles = Vector3.zero;
        
        movement= new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 direction = new Vector3(movement.x,0,movement.y).normalized;

        if(direction.magnitude>=0.1f)
        {
            float targetAngle =Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg+cam.eulerAngles.y;
            float angle=Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,ref turnVelocity,turnTime);
            transform.rotation =Quaternion.Euler(0f,angle,0f);
            Vector3 moveDirection = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;
            controller.Move(moveDirection.normalized*truespeed*Time.deltaTime);
            if(sprinting ==true)
            {
                anim.SetFloat("Speed",2);
            }
            else
            {
                anim.SetFloat("Speed",1);
            }
        }
        else
        {
            anim.SetFloat("Speed",0);
        }
        if(Input.GetButtonDown("Jump")&& isGrounded)
        {
            velocity.y = Mathf.Sqrt((jumpHeight*10)*-2f*gravity);
        }
        if(velocity.y>-20)
        {
            velocity.y +=(gravity*10)*Time.deltaTime;
        }
        
        controller.Move(velocity*Time.deltaTime);
    }
}
