using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    private PlayerMove playerMove;

    private string MoveSpeedname = "MoveSpeed";
    private int MoveSpeedHash;
    private string JumpName = "Jump";
    private int JumpHash;

    private void Awake()
    {
        if (animator == null)
        {
            Debug.Log("Animator component is missing!");
        }
        playerMove = GetComponent<PlayerMove>();

        playerMove.MoveSpeedChanged += SetMoveSpeed;
        playerMove.Jumpforce += SetJump;

        MoveSpeedHash = Animator.StringToHash(MoveSpeedname);
        JumpHash = Animator.StringToHash(JumpName);
    }
    public void SetMoveSpeed(float speed)
    { 
        animator.SetFloat(MoveSpeedHash, speed);
    }
    public void SetJump()
    { 
        animator.SetTrigger(JumpHash);
    }
}
