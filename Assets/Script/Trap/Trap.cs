using System.Collections;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public LayerMask targetLayer;
    public float amount;
    public float effectTime;
    public bool isEffect;

    private WaitForSeconds wait;

    private void Awake()
    {
        wait = new WaitForSeconds(effectTime);
    }

    private void OnTriggerStay(Collider colider)
    {
        if (((1 << colider.gameObject.layer) & targetLayer) == 0) return;

        if (isEffect) return;

        StartCoroutine(Effect(colider.gameObject));


        IDamagerble target = colider.GetComponent<IDamagerble>();
        
        if (target is IDamagerble)
        {
            IDamagerble enemy = target as IDamagerble;
            target.Hit(amount);
        }
    }

    private IEnumerator Effect(GameObject target)
    {
        isEffect = true;
        PlayerStatus state = target.GetComponent<PlayerStatus>();        
        yield return wait;
        
        if (state.state != State.Die)
        { 
            isEffect = false;
        }

    }
}
