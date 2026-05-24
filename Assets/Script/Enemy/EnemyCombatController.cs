using System;
using UnityEngine;

public class EnemyCombatController: MonoBehaviour, IDamagerble
{
    [SerializeField] private EnemyData enemyData;
    public float currentHp;

    public event Action damagedAction;

    private void Awake()
    {
        if (enemyData == null)         
        {
            Debug.Log("[EnemyState] EnemyData 참조 누락");
            return;
        }
    }
    private void Start()
    {
        currentHp = enemyData.maxHp;
    }

    public void Hit(float amount)
    {
        if (amount < 0) return;

        currentHp -= amount;
        damagedAction?.Invoke();
        if(currentHp <= 0) 
        {
            Dead();
        }
    }
    public void Dead()
    { 
        Destroy(gameObject);
    }
}
