using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerMove : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform focusCamera;
    private Rigidbody playerRb;
    private InputPrivider inputProvider;
    private PlayerAnim playeranim;

    private Transform playerPosition;
    public event Action<float> MoveSpeedChanged;
    public event Action Jumpforce;

    private Vector2 moveInputValue;
    private bool isSprint;
    Vector3 moveDir;

    private bool isGrounded;
    private float currentSpeed;
    private bool isJumpInput = false;

    public float moveSpeed = 2f;
    public float sprintSpeed = 5f;
    public float maxturnAngle = 45f;
    public float jumpForce = 5f;

    private void Awake()
    {
        playerPosition = GetComponent<Transform>();
        playerRb = GetComponent<Rigidbody>();
        inputProvider = GetComponent<InputPrivider>();
        playeranim = GetComponent<PlayerAnim>();

        inputProvider.moveAction.performed += Move;
        inputProvider.moveAction.canceled += Move;

        inputProvider.sprintAction.performed += Sprint;
        inputProvider.sprintAction.canceled += Sprint;

        inputProvider.jumpAction.performed += Jump;
    }
    private void FixedUpdate()
    {
        MoveAction();

        if (isJumpInput && isGrounded)
        {
            JumpAction();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") && !isGrounded)
        {
            isGrounded = true;
            isJumpInput = false;
        }
    }

    private void Move(InputAction.CallbackContext context)
    {
        moveInputValue = context.ReadValue<Vector2>();
    }
    private void Sprint(InputAction.CallbackContext context)
    {
        isSprint = context.ReadValueAsButton();
    }

    private void MoveAction()
    {
        Vector3 CameraForward = focusCamera.forward;
        Vector3 CameraRight = focusCamera.right;

        CameraForward.y = 0;
        CameraRight.y = 0;

        CameraForward.Normalize();
        CameraRight.Normalize();

        moveDir = CameraRight * moveInputValue.x + CameraForward * moveInputValue.y;

        currentSpeed = isSprint ? sprintSpeed : moveSpeed;

        Vector3  velocity = moveDir * currentSpeed;
        velocity.y = playerRb.linearVelocity.y;
        playerRb.linearVelocity = velocity;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            bool isBackMove = moveInputValue.y <= 0f;

            if (!isBackMove)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                playerPosition.rotation = Quaternion.Slerp(playerPosition.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        float moveSpeedParam = moveInputValue.sqrMagnitude * currentSpeed;        
        MoveSpeedChanged?.Invoke(moveSpeedParam);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        isJumpInput = context.ReadValueAsButton();
    }
    private void JumpAction()
    {
        isGrounded = false;
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        Jumpforce?.Invoke();
    }

}