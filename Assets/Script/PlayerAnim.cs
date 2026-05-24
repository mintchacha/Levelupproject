using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    private PlayerMove playerMove;
    private PlayerCombatController playerCombatController;

    private string MoveSpeedname = "MoveSpeed";
    private int MoveSpeedHash;
    private string JumpName = "Jump";
    private int JumpHash;
    private string AttackName = "Attack";
    private int AttackHash;
    private string damageName = "Damage";
    private int damageHash;
    private void Awake()
    {
        if (animator == null)
        {
            Debug.Log("Animator component is missing!");
        }
        playerMove = GetComponent<PlayerMove>();
        playerCombatController = GetComponent<PlayerCombatController>();

        MoveSpeedHash = Animator.StringToHash(MoveSpeedname);
        JumpHash = Animator.StringToHash(JumpName);
        AttackHash = Animator.StringToHash(AttackName);
        damageHash = Animator.StringToHash(damageName);
    }
    private void OnEnable()
    {
        playerMove.MoveSpeedChanged += SetMoveSpeed;
        playerMove.Jumpforce += SetJump;
        playerCombatController.attackEvent += SetAttack;
        playerCombatController.damagedAction += SetDamage;
    }
    private void OnDisable()
    {
        playerMove.MoveSpeedChanged -= SetMoveSpeed;
        playerMove.Jumpforce -= SetJump;
        playerCombatController.attackEvent -= SetAttack;
        playerCombatController.damagedAction += SetDamage;
    }
    public void SetMoveSpeed(float speed)
    { 
        animator.SetFloat(MoveSpeedHash, speed);
    }
    public void SetJump()
    { 
        animator.SetTrigger(JumpHash);
    }
    public void SetAttack()
    {
        animator.SetTrigger(AttackHash);
    }
    public void SetDamage()
    {
        animator.SetTrigger(damageHash);
    }

}
