using UnityEngine;

public enum ItemType {
    Health,
    Mana
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/Item")]
public class ItemData : ScriptableObject
{
    public int itemId;
    public int itemName;
    public ItemType type;
    public float value;
}
