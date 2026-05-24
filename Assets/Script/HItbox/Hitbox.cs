using UnityEngine;
using UnityEngine.UIElements;

public class Hitbox : MonoBehaviour
{
    BoxCollider colider;

    public Vector3 size;
    public float activeTime;
    public LayerMask targetMask;
    public float amount;
    public GameObject owner;

    private void OnEnable()
    {
        Destroy(gameObject, 0.3f);
    }
    private void OnTriggerEnter(Collider colider)
    {
        if (colider.gameObject == owner) return;

        IDamagerble target = colider.GetComponent<IDamagerble>();
        Debug.Log(target);
        if (target is IDamagerble)
        {
            IDamagerble enemy = target as IDamagerble;
            target.Hit(amount);
        }        
    }

    public void Initialize(GameObject owner, Vector3 size, float activeTime, LayerMask targetMask, float amount)
    {
        this.owner = owner;
        this.size = size;
        this.activeTime = activeTime;
        this.targetMask = targetMask;
        this.amount = amount;

        colider = GetComponent<BoxCollider>();
        colider.size = size;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.greenYellow;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
