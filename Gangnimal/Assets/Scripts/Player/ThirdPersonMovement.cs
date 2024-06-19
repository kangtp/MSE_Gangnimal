using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.Networking;

// ThirdPersonMovement class handles the player's movement in a 3D environment using Unity's CharacterController
public class ThirdPersonMovement : NetworkBehaviour
{
    // Smooth turn duration for character rotation
    float turnTime = 0.1f; 
    float turnVelocity; // Velocity used for smoothing rotation
    public Animator anim; // Animator component for controlling animations
    bool sprinting; // Flag to check if player is sprinting
    CharacterController controller; // CharacterController component for handling movement
    Vector2 movement; // Stores movement input
    public float walkSpeed; // Speed for walking
    public float sprintSpeed; // Speed for sprinting
    float truespeed; // Current speed of the player (walk or sprint)
    public float turnspeed; // Speed of rotation
    bool oneDie = false; // Flag to ensure death logic runs once
    public float jumpHeight; // Jump height
    public float gravity; // Gravity affecting the player
    bool isGrounded; // Flag to check if the player is on the ground
    Vector3 velocity; // Velocity of the player for vertical movement

    PlayerInfo playerInfo; // Component holding player info like HP

    Transform HostSpawnPoint; // Spawn point for the host
    Transform ClientSpawnPoint; // Spawn point for the client

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        truespeed = walkSpeed; // Set the initial speed to walking speed
        controller = GetComponent<CharacterController>(); // Get the CharacterController component
        // anim = GetComponentInChildren<Animator>(); // (Commented) Get the Animator component
        playerInfo = GetComponentInChildren<PlayerInfo>(); // Get the PlayerInfo component
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
        StartCoroutine(awaitSpawn()); // Start the coroutine to handle player spawning
    }

    // Coroutine to wait and find the camera after a delay
    private IEnumerator awaitFindCamera()
    {
        yield return new WaitForSeconds(5.0f); // Wait for 5 seconds
    }

    private bool isSpawned = false; // Flag to check if the player has spawned

    private string Mapname; // Name of the map
    private int mapcode; // Code corresponding to the map

    // Coroutine to handle player spawning based on the selected map
    private IEnumerator awaitSpawn()
    {
        // Determine the map based on the selected map index
        switch (PlayerPrefs.GetString("SelectedMapIndex"))
        {
            case "0":
            {
                Mapname = "ForestMap";
                mapcode = 0;
            }
            break;

            case "1":
            {
                Mapname = "DesertMap";
                mapcode = 1;
            }
            break;

            case "2":
            {
                Mapname = "WinterMap";
                mapcode = 2;
            }
            break;
            default:
            break;
        }

        // Loop until the player is spawned
        while (!isSpawned)
        {
            yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds before checking again
            if (IsHost) // Check if the player is the host
            {
                // Find the host spawn point based on the selected map
                Transform hostSpawnTransform = GameObject.Find("Map").transform.GetChild(mapcode).GetChild(0).transform;
                if (hostSpawnTransform != null)
                {
                    HostSpawnPoint = hostSpawnTransform; // Set the host spawn point
                    this.gameObject.transform.position = HostSpawnPoint.position; // Move the player to the spawn point
                    Debug.Log("host!!" + this.gameObject.transform.position); // Log the spawn position
                    isSpawned = true; // Set the flag to true indicating the player is spawned
                }
            }
            else if (!IsHost)
            {
                // Find the client spawn point based on the selected map
                Transform clientSpawnTransform = GameObject.Find("Map").transform.GetChild(mapcode).GetChild(1).transform;
                if (clientSpawnTransform != null)
                {
                    ClientSpawnPoint = clientSpawnTransform; // Set the client spawn point
                    this.gameObject.transform.position = ClientSpawnPoint.position; // Move the player to the spawn point
                    Debug.Log("Client!!" + this.gameObject.transform.position); // Log the spawn position
                    isSpawned = true; // Set the flag to true indicating the player is spawned
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.InitializeGameOverPanel(); // Initialize the game over panel from the GameManager
    }

    // Update is called once per frame
    void Update()
    {
        // If Tab is released, decrease the player's HP by 100 (for testing)
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            playerInfo.HP -= 100;
        }

        // If player is alive and owns the object, allow movement
        if (playerInfo.HP > 0 && IsOwner)
        {
            PlayerMove3rd(); // Call the method to handle player movement
        }
        else if (playerInfo.HP <= 0 && !oneDie) // Check if the player has died and death logic hasn't run yet
        {
            if (IsServer) // If the player is the server, update the battle record and notify clients
            {
                FindObjectOfType<AccountManager>().UpdateBattleRecord("lose");
                WhenDeadHostClientRpc();
            }
            else if (!IsServer) // If the player is not the server, request to kill the player on the server
            {
                RequestKillClient_ServerRpc();
            }
            oneDie = true; // Set the flag to true indicating the death logic has run
        }
    }

    // Handles the third-person movement logic
    public void PlayerMove3rd()
    {
        CheckGroundStatus(); // Check if the player is grounded

        // If the player is grounded and velocity is negative, reset the velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        // Check if the LeftShift key is pressed for sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            truespeed = sprintSpeed; // Set the speed to sprint speed
            sprinting = true; // Set the sprinting flag to true
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            truespeed = walkSpeed; // Set the speed to walking speed
            sprinting = false; // Set the sprinting flag to false
        }

        // Reset the animator's local position and rotation
        anim.transform.localPosition = Vector3.zero;
        anim.transform.localEulerAngles = Vector3.zero;

        // Get the movement input
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized; // Normalize the direction vector

        // If there is any movement input
        if (direction.magnitude >= 0.1f)
        {
            // Calculate the target angle based on the camera's orientation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime); // Smoothly rotate towards the target angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // Apply the rotation to the player
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // Calculate the move direction
            controller.Move(moveDirection.normalized * truespeed * Time.deltaTime); // Move the player

            // Update the animation speed based on sprinting state
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
            anim.SetFloat("Speed", 0); // Set the animation speed to 0 if there is no movement
        }

        // Handle jumping logic if the player is alive
        if (playerInfo.HP > 0)
        {
            Jump();
        }

        // Apply gravity to the player
        if (velocity.y > -20)
        {
            velocity.y += (gravity * 10) * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime); // Apply the velocity to move the player
    }

    // Checks if the player is on the ground
    private void CheckGroundStatus()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.2f, 1 << 3); // Use a sphere to check for ground contact
        anim.SetBool("isGround", isGrounded); // Update the animator with the grounded status
    }

    // Handles the jumping logic
    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded) // Check if the jump button is pressed and the player is grounded
        {
            velocity.y = Mathf.Sqrt((jumpHeight * 10) * -2f * gravity); // Calculate the jump velocity
            GameManager.instance.PlayJumpSound(); // Play the jump sound from the GameManager
        }
    }

    // Client-side method to handle the player's death for the host
    [ClientRpc]
    private void WhenDeadHostClientRpc()
    {
        if (!IsServer) // If the player is not the server, update the battle record to win
        {
            FindObjectOfType<AccountManager>().UpdateBattleRecord("win");
        }

        // Ensure the death animation is played only once
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            if (playerInfo.HP <= 0)
            {
                GameManager.instance.SetAlive(false); // Update the alive status in the GameManager
                anim.SetTrigger("Death"); // Trigger the death animation
            }

            GameManager.instance.GameOver(); // Call the game over logic from the GameManager
            // Log if the game over panels are missing
            if (GameManager.instance.losePannel == null && GameManager.instance.winPannel == null)
            {
                Debug.Log("없어!");
            }
            Debug.Log("게임오버패널 켜져야지!");
        }
    }

    // Server-side method to request the player kill action
    [ServerRpc]
    private void RequestKillClient_ServerRpc()
    {
        FindObjectOfType<AccountManager>().UpdateBattleRecord("win"); // Update the battle record to win

        KillClientRpc(); // Call the client-side method to handle death
    }

    // Client-side method to handle the player's death
    [ClientRpc]
    private void KillClientRpc()
    {
        if (!IsServer) // If the player is not the server, update the battle record to lose
        {
            FindObjectOfType<AccountManager>().UpdateBattleRecord("lose");
        }

        // Ensure the death animation is played only once
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Falling Idle"))
            {
                anim.Play("idle"); // Play idle animation if currently falling idle
            }
            if (playerInfo.HP <= 0)
            {
                GameManager.instance.SetAlive(false); // Update the alive status in the GameManager
                anim.SetTrigger("Death"); // Trigger the death animation
            }
            GameManager.instance.GameOver(); // Call the game over logic from the GameManager
            // Log if the game over panels are missing
            if (GameManager.instance.losePannel == null && GameManager.instance.winPannel == null)
            {
                Debug.Log("없어!");
            }
            Debug.Log("게임오버패널 켜져야지!");
        }
    }

    // Draws gizmos in the editor to visualize the ground check sphere
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set the gizmo color to red
        Gizmos.DrawWireSphere(transform.position, 0.2f); // Draw a wire sphere at the player's position
    }
}
