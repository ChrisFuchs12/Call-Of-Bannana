using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
 
public class PlayerController : NetworkBehaviour
{   
    //movement
    [Header("Base setup")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
 
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
 
    [HideInInspector]
    public bool canMove = true;

    public float amountOfJumps = 1;

    private float jumpsRemaining = 1;
 
    [SerializeField]
    private float cameraYOffset = 0.4f;
    public Camera playerCamera;
    public GameObject camHolder;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

 
    public override void OnStartClient()
    { 
        base.OnStartClient();
        if(base.IsOwner){
            playerCamera.enabled = true;
            characterController.enabled = true;
            playerCamera = Camera.main;
            playerCamera.transform.SetParent(transform);
        }
 
        if (!base.IsOwner)
        {
            playerCamera.enabled = false;
            characterController.enabled = false;
            return;
        }

    }
 
    void Start()
    {
        characterController = GetComponent<CharacterController>();
 
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
 
    void Update()
    {

        if(PlayerDamageDetector.health <= 0){
            walkingSpeed = 0f;
            runningSpeed = 0f;
            jumpSpeed = 0f;
        }

        bool isRunning = false;
 
        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);
 
        // We are grounded, so recalculate move direction based on axis
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
 
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
 
        if (Input.GetButtonDown("Jump") && canMove && jumpsRemaining >= 2)
        {
            moveDirection.y = jumpSpeed;
            jumpsRemaining = jumpsRemaining - 1;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (characterController.isGrounded){
            jumpsRemaining = amountOfJumps;
        }
 
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
 
        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
 
        // Player and Camera rotation
        if (canMove && playerCamera != null && base.IsOwner)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            ServerMoveCam();
        }

    }

    [ObserversRpc]
    void ServerMoveCam()
    {
        camHolder.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        //ObserverMoveCam();
    }

    
    
    [ServerRpc(RequireOwnership = false)]
    void ObserverMoveCam()
   {
        
   }
}
