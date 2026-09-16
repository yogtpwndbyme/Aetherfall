using UnityEngine;
using TMPro;

public class HeroHUD : MonoBehaviour
{
    [Header("Hero")]
    [SerializeField] private HeroProgression heroProgression;
    [SerializeField] private HeroHealth heroHealth;
    [SerializeField] private HeroInventory heroInventory;

    [Header("HUD Text")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text inventoryText;
    [SerializeField] private TMP_Text weaponText;

    [Header("Inventory Slots")]
    [SerializeField] private TMP_Text[] itemNameTexts;

    [Header("Equipment")]
    [SerializeField] private TMP_Text weaponSlotText;
    [SerializeField] private TMP_Text helmetSlotText;
    [SerializeField] private TMP_Text chestSlotText;
    [SerializeField] private TMP_Text bootsSlotText;

    void Update()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (heroProgression != null)
        {
            if (levelText != null)
            {
                levelText.text =
                    "Level " +
                    heroProgression.CurrentLevel;
            }

            if (xpText != null)
            {
                xpText.text =
                    "XP " +
                    heroProgression.CurrentXP +
                    " / " +
                    heroProgression.XPToNextLevel;
            }

            if (goldText != null)
            {
                goldText.text =
                    "Gold " +
                    heroProgression.CurrentGold;
            }
        }

        if (heroHealth != null && healthText != null)
        {
            healthText.text =
                "HP " +
                heroHealth.CurrentHealth +
                " / " +
                heroHealth.MaxHealth;
        }

        if (heroInventory != null)
        {
            if (inventoryText != null)
            {
                inventoryText.text =
                    "Inventory " +
                    heroInventory.CurrentItems +
                    " / " +
                    heroInventory.MaxItems;
            }

            UpdateWeaponText();
            UpdateInventorySlots();
            UpdateEquipmentSlots();
        }
    }

    private void UpdateWeaponText()
    {
        if (weaponText == null)
            return;

        if (heroInventory.EquippedWeaponName == "None")
        {
            weaponText.text = "Weapon: None";
        }
        else
        {
            weaponText.text =
                "Weapon: " +
                heroInventory.EquippedWeaponName +
                " (+" +
                heroInventory.EquippedDamageBonus +
                " Damage)";
        }
    }

    private void UpdateInventorySlots()
    {
        if (itemNameTexts == null)
            return;

        for (int i = 0; i < itemNameTexts.Length; i++)
        {
            if (itemNameTexts[i] == null)
                continue;

            itemNameTexts[i].text =
                heroInventory.GetItemName(i);
        }
    }

    private void UpdateEquipmentSlots()
    {
        if (weaponSlotText != null)
        {
            weaponSlotText.text =
                heroInventory.EquippedWeaponName == "None"
                ? "Weapon"
                : heroInventory.EquippedWeaponName;
        }

        if (helmetSlotText != null)
        {
            helmetSlotText.text =
                heroInventory.EquippedHelmetName == "None"
                ? "Head"
                : heroInventory.EquippedHelmetName;
        }

        if (chestSlotText != null)
        {
            chestSlotText.text =
                heroInventory.EquippedChestName == "None"
                ? "Chest"
                : heroInventory.EquippedChestName;
        }

        if (bootsSlotText != null)
        {
            bootsSlotText.text =
                heroInventory.EquippedBootsName == "None"
                ? "Boots"
                : heroInventory.EquippedBootsName;
        }
    }
}