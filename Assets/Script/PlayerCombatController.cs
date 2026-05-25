using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class PlayerCombatController : MonoBehaviour, IDamagerble
{
    PlayerMove playerMove;
    InputPrivider inputProvider;    
    PlayerStatus status;    

    [SerializeField] Hitbox hitbox;    

    public event Action attackEvent;
    public event Action damagedAction;
    public event Action dieAction;

    public float currentHp;

    [Header("기본공격 세팅")]
    public float damage;
    public Vector3 attackRange;
    public float attackDistance;
    public float isAttackTime;
    public bool isAttacking;
    public LayerMask attackTarget;

    private void Awake()
    {
        inputProvider = GetComponent<InputPrivider>();
        playerMove = GetComponent<PlayerMove>();
        status = GetComponent<PlayerStatus>();

        if (hitbox == null)
        {
            Debug.Log("[PlayerCombatController] hitbox 참조 누락");
            return;
        }

    }
    private void Start()
    {
        currentHp = status.maxHp;
        inputProvider.attackAction.performed += Attack;
    }
    private void OnDisable()
    {
        inputProvider.attackAction.performed -= Attack;
    }

    private void Attack(InputAction.CallbackContext context) 
    {
        if (isAttacking) return;

        StartCoroutine(AttackCooldown());

        attackEvent?.Invoke();

        Vector3 spawnPosition = transform.position + transform.forward * attackDistance;

        Instantiate(hitbox, spawnPosition, Quaternion.identity);
        hitbox.Initialize(gameObject, attackRange, isAttackTime, attackTarget, damage);
        playerMove.DashForWard(1.5f);
    }
    public IEnumerator AttackCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(isAttackTime);
        isAttacking = false;
    }
    public void Hit(float amount)
    {
        if (amount < 0) return;

        currentHp -= amount;
        damagedAction?.Invoke();
        Debug.Log("아픔");
        if (currentHp <= 0)
        {
            Dead();
        }
    }
    public void Dead()
    {
        dieAction?.Invoke();
    }

    public void Health(float amount) 
    {
        currentHp = Mathf.Clamp(currentHp + amount, 0, status.maxHp);        
    }
}
