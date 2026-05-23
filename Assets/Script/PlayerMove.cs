using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody playerRb;
    private InputPrivider inputProvider;

    private Transform playerPosition;
    public event Action<float> MoveSpeedChanged;
    public event Action Jumpforce;

    private Vector2 moveInputValue;
    private bool isSprint;
    private bool isAttack;

    private bool isGrounded;
    private float currentSpeed;
    private bool isJumpInput = false;

    [Header("마우스 회전 속도")]
    [SerializeField] private float aimSpeed = 0.05f;
    private float yaw;
    [Header("이동 속도")]
    public float moveSpeed = 2f;
    public float sprintSpeed = 5f;
    public float maxturnAngle = 45f;
    public float jumpForce = 5f;

    private void Awake()
    {
        playerPosition = GetComponent<Transform>();
        playerRb = GetComponent<Rigidbody>();
        inputProvider = GetComponent<InputPrivider>();

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
        Vector3 focus = inputProvider.lookInputValue;
        yaw += focus.x * aimSpeed;
        focus.y = 0f;
        focus.Normalize();

        if (isAttack)
        {
            Vector3 dash = transform.forward * 0.5f;
            playerRb.linearVelocity = dash;
            return;
        }

        currentSpeed = isSprint ? sprintSpeed : moveSpeed;

        //if (moveDir.sqrMagnitude > 0.01f)
        //{
            Quaternion targetRotation = Quaternion.Euler(0f, yaw, 0f);
            playerPosition.rotation = Quaternion.Slerp(playerPosition.rotation, targetRotation, Time.deltaTime * 10f);
        //}

        //Vector3 moveDir = new Vector3(transmoveInputValue.x, 0, moveInputValue.y);
        Vector3 moveDir = new Vector3(transform.forward.x * moveInputValue.y + transform.right.x * moveInputValue.x, 0,
            transform.forward.z * moveInputValue.y + transform.right.z * moveInputValue.x);

        Vector3 velocity = moveDir * currentSpeed;
        velocity.y = playerRb.linearVelocity.y;
        playerRb.linearVelocity = velocity;

        float moveSpeedParam = moveInputValue.sqrMagnitude * currentSpeed;        
        MoveSpeedChanged?.Invoke(moveSpeedParam);
    }

    public void DashForWard(float dashValue)
    {
        isAttack = true;
        StartCoroutine(ResetAttackState());
    }
    private IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(1f);
        isAttack = false;
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