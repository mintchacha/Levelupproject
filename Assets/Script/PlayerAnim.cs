using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;    

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

        MoveSpeedHash = Animator.StringToHash(MoveSpeedname);
        JumpHash = Animator.StringToHash(JumpName);
    }
    public void SetMoveSpeed(float speed)
    { 
        animator.SetFloat(MoveSpeedHash, speed);
    }
    public void SetJump(bool isJumping)
    { 
        animator.SetBool(JumpHash, isJumping);
    }
}
