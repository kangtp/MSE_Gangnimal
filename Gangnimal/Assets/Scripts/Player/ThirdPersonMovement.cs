using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.Networking;

public class ThirdPersonMovement : NetworkBehaviour
{
    float turnTime = 0.1f;
    float turnVelocity;
    public Animator anim;
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

    PlayerInfo playerInfo;

    private bool isDead = false;


    private void Awake()
    {
        truespeed = walkSpeed;
        controller = GetComponent<CharacterController>();
        //anim = GetComponentInChildren<Animator>();
        playerInfo = GetComponentInChildren<PlayerInfo>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private IEnumerator awaitFindCamera()
    {
        yield return new WaitForSeconds(5.0f);
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playerInfo.HP -= 100;
            if (playerInfo.HP <= 0 && !isDead)
            {
                isDead = true;
                anim.SetTrigger("Death");
                GameManager.instance.GameOver();
            }
        }

        if (playerInfo.HP > 0 && IsLocalPlayer)
        {
            PlayerMove3rd();
        }
        else
        {
            WhenDead();
        }
    }

    public void PlayerMove3rd()
    {
        CheckGroundStatus();
        float h = turnspeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, h, 0);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            truespeed = sprintSpeed;
            sprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            truespeed = walkSpeed;
            sprinting = false;
        }
        anim.transform.localPosition = Vector3.zero;
        anim.transform.localEulerAngles = Vector3.zero;

        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * truespeed * Time.deltaTime);
            if (sprinting)
            {
                anim.SetFloat("Speed", 2);
            }
            else
            {
                anim.SetFloat("Speed", 1);
            }
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }

        if (playerInfo.HP > 0)
        {
            Jump();
        }


        if (velocity.y > -20)
        {
            velocity.y += (gravity * 10) * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.2f, 1 << 3);
        anim.SetBool("isGround", isGrounded);
    }

    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt((jumpHeight * 10) * -2f * gravity);
            Debug.Log(isGrounded);
        }
    }

    private void WhenDead()
    {
        if (!isGrounded)
        {
            if (velocity.y > -20)
            {
                velocity.y += (gravity * 10) * Time.deltaTime;
            }
            controller.Move(velocity * Time.deltaTime);
        }
        else if (isGrounded && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            anim.SetTrigger("Death");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
