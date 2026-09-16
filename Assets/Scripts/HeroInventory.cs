using System.Collections.Generic;
using UnityEngine;

public class HeroInventory : MonoBehaviour
{
    [System.Serializable]
    public class InventoryItem
    {
        public string itemName;
        public ItemData.ItemType itemType;
        public int damageBonus;
        public int armorBonus;

        public InventoryItem(
            string newItemName,
            ItemData.ItemType newItemType,
            int newDamageBonus,
            int newArmorBonus)
        {
            itemName = newItemName;
            itemType = newItemType;
            damageBonus = newDamageBonus;
            armorBonus = newArmorBonus;
        }
    }

    [Header("Inventory")]
    [SerializeField] private int maxItems = 18;

    private List<InventoryItem> items = new List<InventoryItem>();

    [Header("Equipment")]
    private InventoryItem equippedWeapon;
    private int equippedWeaponIndex = -1;

    private InventoryItem equippedHelmet;
    private int equippedHelmetIndex = -1;

    private InventoryItem equippedChest;
    private int equippedChestIndex = -1;

    private InventoryItem equippedBoots;
    private int equippedBootsIndex = -1;

    public int CurrentItems
    {
        get { return items.Count; }
    }

    public int MaxItems
    {
        get { return maxItems; }
    }

    public int EquippedDamageBonus
    {
        get
        {
            if (equippedWeapon == null)
                return 0;

            return equippedWeapon.damageBonus;
        }
    }

    public int EquippedArmorBonus
    {
        get
        {
            int totalArmor = 0;

            if (equippedHelmet != null)
                totalArmor += equippedHelmet.armorBonus;

            if (equippedChest != null)
                totalArmor += equippedChest.armorBonus;

            if (equippedBoots != null)
                totalArmor += equippedBoots.armorBonus;

            return totalArmor;
        }
    }

    public string EquippedWeaponName
    {
        get { return equippedWeapon == null ? "None" : equippedWeapon.itemName; }
    }

    public int EquippedWeaponIndex
    {
        get { return equippedWeaponIndex; }
    }

    public string EquippedHelmetName
    {
        get { return equippedHelmet == null ? "None" : equippedHelmet.itemName; }
    }

    public int EquippedHelmetIndex
    {
        get { return equippedHelmetIndex; }
    }

    public string EquippedChestName
    {
        get { return equippedChest == null ? "None" : equippedChest.itemName; }
    }

    public int EquippedChestIndex
    {
        get { return equippedChestIndex; }
    }

    public string EquippedBootsName
    {
        get { return equippedBoots == null ? "None" : equippedBoots.itemName; }
    }

    public int EquippedBootsIndex
    {
        get { return equippedBootsIndex; }
    }

    public bool TryAddItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning("Tried to add an item with no ItemData.");
            return false;
        }

        if (items.Count >= maxItems)
        {
            Debug.Log("Inventory is full.");
            return false;
        }

        InventoryItem newItem = new InventoryItem(
            item.ItemName,
            item.Type,
            item.DamageBonus,
            item.ArmorBonus
        );

        items.Add(newItem);

        Debug.Log(
            newItem.itemName +
            " added to inventory. " +
            items.Count +
            "/" +
            maxItems
        );

        return true;
    }

    public string GetItemName(int index)
    {
        if (index < 0 || index >= items.Count)
            return "Empty";

        return items[index].itemName;
    }

    public bool IsItemEquipped(int index)
    {
        return index == equippedWeaponIndex
            || index == equippedHelmetIndex
            || index == equippedChestIndex
            || index == equippedBootsIndex;
    }

    public void ToggleEquipItem(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            Debug.Log("That inventory slot is empty.");
            return;
        }

        InventoryItem item = items[index];

        if (item.itemType == ItemData.ItemType.Weapon)
        {
            ToggleEquipWeapon(index);
        }
        else if (item.itemType == ItemData.ItemType.Helmet)
        {
            ToggleEquipHelmet(index);
        }
        else if (item.itemType == ItemData.ItemType.Chest)
        {
            ToggleEquipChest(index);
        }
        else if (item.itemType == ItemData.ItemType.Boots)
        {
            ToggleEquipBoots(index);
        }
    }

    private void ToggleEquipWeapon(int index)
    {
        if (equippedWeaponIndex == index)
        {
            UnequipWeapon();
            return;
        }

        equippedWeapon = items[index];
        equippedWeaponIndex = index;

        Debug.Log(
            equippedWeapon.itemName +
            " equipped in Weapon Slot. Damage bonus: +" +
            equippedWeapon.damageBonus
        );
    }

    private void ToggleEquipHelmet(int index)
    {
        if (equippedHelmetIndex == index)
        {
            UnequipHelmet();
            return;
        }

        equippedHelmet = items[index];
        equippedHelmetIndex = index;

        Debug.Log(
            equippedHelmet.itemName +
            " equipped in Helmet Slot. Armor bonus: +" +
            equippedHelmet.armorBonus
        );
    }

    private void ToggleEquipChest(int index)
    {
        if (equippedChestIndex == index)
        {
            UnequipChest();
            return;
        }

        equippedChest = items[index];
        equippedChestIndex = index;

        Debug.Log(
            equippedChest.itemName +
            " equipped in Chest Slot. Armor bonus: +" +
            equippedChest.armorBonus
        );
    }

    private void ToggleEquipBoots(int index)
    {
        if (equippedBootsIndex == index)
        {
            UnequipBoots();
            return;
        }

        equippedBoots = items[index];
        equippedBootsIndex = index;

        Debug.Log(
            equippedBoots.itemName +
            " equipped in Boots Slot. Armor bonus: +" +
            equippedBoots.armorBonus
        );
    }

    public void UnequipWeapon()
    {
        if (equippedWeapon == null)
            return;

        Debug.Log(equippedWeapon.itemName + " unequipped.");

        equippedWeapon = null;
        equippedWeaponIndex = -1;
    }

    public void UnequipHelmet()
    {
        if (equippedHelmet == null)
            return;

        Debug.Log(equippedHelmet.itemName + " unequipped.");

        equippedHelmet = null;
        equippedHelmetIndex = -1;
    }

    public void UnequipChest()
    {
        if (equippedChest == null)
            return;

        Debug.Log(equippedChest.itemName + " unequipped.");

        equippedChest = null;
        equippedChestIndex = -1;
    }

    public void UnequipBoots()
    {
        if (equippedBoots == null)
            return;

        Debug.Log(equippedBoots.itemName + " unequipped.");

        equippedBoots = null;
        equippedBootsIndex = -1;
    }
}