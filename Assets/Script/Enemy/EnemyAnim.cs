using UnityEngine;

public class EnemyAnim : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    private PlayerMove playerMove;
    private EnemyCombatController enemyCombatController;

    private string damageName = "Damage";
    private int damageHash;

    private void Awake()
    {
        if (animator == null)
        {
            Debug.Log("Animator component is missing!");
        }
        enemyCombatController = GetComponent<EnemyCombatController>();

        damageHash = Animator.StringToHash(damageName);
    }
    private void OnEnable()
    {
        enemyCombatController.damagedAction += SetDamage;
    }
    private void OnDisable()
    {
        enemyCombatController.damagedAction -= SetDamage;
    }
    public void SetDamage()
    {
        animator.SetTrigger(damageHash);
    }

}
