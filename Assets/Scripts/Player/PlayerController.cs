using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PlayerController : NetworkBehaviour
{
    [Header("Base setup")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;

    [SyncVar(OnChange = nameof(OnCameraRotationChanged))]
    private Vector2 networkCameraRotation = Vector2.zero;

    [HideInInspector]
    public bool canMove = true;

    public float amountOfJumps = 1;
    private float jumpsRemaining = 1;

    [SerializeField]
    private float cameraYOffset = 0.4f;
    public Camera playerCamera;
    public GameObject camHolder;

    public float dashTime = 100;
    private bool shouldRemoveDash = false;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnCameraRotationChanged(Vector2 oldValue, Vector2 newValue, bool asServer)
    {
        if (!base.IsOwner && playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(newValue.x, 0, 0);
            transform.rotation = Quaternion.Euler(0, newValue.y, 0);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
        {
            playerCamera.enabled = false;
        }

        characterController.enabled = true;
    }

    void Update()
    {
        if (!base.IsOwner) return;

        if (PlayerDamageDetector.health <= 0) return;

        // Movement
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButtonDown("Jump") && canMove && jumpsRemaining >= 2)
        {
            moveDirection.y = jumpSpeed;
            jumpsRemaining--;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (characterController.isGrounded)
        {
            jumpsRemaining = amountOfJumps;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKeyDown("q") && dashTime <= 100)
        {
            runningSpeed = 50f;
            shouldRemoveDash = true;
        }
        if (dashTime <= 0)
        {
            runningSpeed = 20f;
            shouldRemoveDash = false;
        }
        if (Input.GetKeyUp("q"))
        {
            runningSpeed = 20f;
            dashTime = 0;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        // Camera rotation
        if (canMove && playerCamera != null)
        {
            float rotationX = networkCameraRotation.x + -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            float rotationY = networkCameraRotation.y + Input.GetAxis("Mouse X") * lookSpeed;

            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation = Quaternion.Euler(0, rotationY, 0);

            // Update the networked camera rotation
            if (networkCameraRotation.x != rotationX || networkCameraRotation.y != rotationY)
            {
                networkCameraRotation = new Vector2(rotationX, rotationY);
            }
        }
    }
}
