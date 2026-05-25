using UnityEngine;

public class Item : MonoBehaviour
{
    private BoxCollider collider;
    [SerializeField] private ItemData data;
    [SerializeField] private LayerMask targetLayer;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) == 0) return;

        PlayerCombatController target = other.GetComponent<PlayerCombatController>();

        switch (data.type)
        {
            case ItemType.Health :
                target.Health(data.value);
                break;
            case ItemType.Mana :
                Debug.Log("마나회복");
                break;            
        }        


        Destroy(collider.gameObject);
    }
}
