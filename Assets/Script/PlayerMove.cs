using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerManger PlayerManger;
    [SerializeField] private Transform focusCamera;
    private Rigidbody playerRb;

    private Transform playerPosition;

    private Vector2 moveValue;
    private bool isGrounded;

    public float moveSpeed = 2f;
    public float sprintSpeed = 5f;
    public float maxturnAngle = 45f;
    public float jumpForce = 5f;

    private void Awake()
    {
        if (PlayerManger == null)
        { 
            Debug.Log("PlayerManger reference is missing!");
            return;
        }
        playerPosition = PlayerManger.GetComponent<Transform>();
        playerRb = PlayerManger.GetComponent<Rigidbody>();
        if (playerRb == null) 
        {
            Debug.Log("Rigidbody component is missing!");
            return;
        }
    }
    private void Update()
    {

        moveValue = PlayerManger.inputProvider.moveInputValue;
        Move();
    }
    private void FixedUpdate()
    {
        if (PlayerManger.inputProvider.jumpInputValue && isGrounded)
        {
            Jump();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") && !isGrounded)
        {
            isGrounded = true;
        }
    }

    private void Move()
    {        
        Vector3 CameraForward = focusCamera.forward;
        Vector3 CameraRight = focusCamera.right;

        CameraForward.y = 0;
        CameraRight.y = 0;

        CameraForward.Normalize();
        CameraRight.Normalize();

        Vector3 moveDir = CameraRight * moveValue.x + CameraForward * moveValue.y;
        float currentSpeed = PlayerManger.inputProvider.sprintInputValue ? sprintSpeed : moveSpeed;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            playerPosition.position += moveDir * currentSpeed * Time.deltaTime;

            bool isBackMove = moveValue.y <= 0f;

            if (!isBackMove)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                playerPosition.rotation = Quaternion.Slerp(playerPosition.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }


        float moveSpeedParam = moveValue.magnitude * currentSpeed;
        PlayerManger.playeranim.SetMoveSpeed(moveSpeedParam);
    }

    private void Jump()
    {
        isGrounded = false;
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        PlayerManger.playeranim.SetJump(PlayerManger.inputProvider.jumpInputValue);
        
        PlayerManger.inputProvider.ConsumeJumpInput();
    }
       
}