using UnityEngine;

public class ItemData : MonoBehaviour
{
    public enum ItemType
    {
        Weapon,
        Helmet,
        Chest,
        Boots
    }

    [Header("Item")]
    [SerializeField] private string itemName = "Iron Sword";
    [SerializeField] private ItemType itemType = ItemType.Weapon;

    [Header("Stats")]
    [SerializeField] private int damageBonus = 5;
    [SerializeField] private int armorBonus = 0;

    public string ItemName
    {
        get { return itemName; }
    }

    public ItemType Type
    {
        get { return itemType; }
    }

    public int DamageBonus
    {
        get { return damageBonus; }
    }

    public int ArmorBonus
    {
        get { return armorBonus; }
    }
}