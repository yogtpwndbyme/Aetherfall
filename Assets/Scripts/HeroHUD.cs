
using UnityEngine;
using TMPro;

public class HeroHUD : MonoBehaviour
{
    private HeroProgression heroProgression;
    private HeroHealth heroHealth;
    private HeroInventory heroInventory;

    private HeroSelection currentHero;

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
        UpdateSelectedHero();
        UpdateHUD();
    }

    private void UpdateSelectedHero()
    {
        HeroSelection selectedHero = null;

        if (UnitSelectionManager.Instance != null)
        {
            selectedHero = UnitSelectionManager.Instance
                .MainSelectedHero;
        }

        if (selectedHero == currentHero &&
            selectedHero != null)
            return;

        currentHero = selectedHero;

        if (currentHero == null)
        {
            heroProgression = null;
            heroHealth = null;
            heroInventory = null;
            return;
        }

        heroProgression =
            currentHero.GetComponent<HeroProgression>();

        heroHealth =
            currentHero.GetComponent<HeroHealth>();

        heroInventory =
            currentHero.GetComponent<HeroInventory>();
    }

    private void UpdateHUD()
    {
        if (levelText != null)
        {
            levelText.text = heroProgression != null
                ? "Level " + heroProgression.CurrentLevel
                : "Level -";
        }

        if (xpText != null)
        {
            xpText.text = heroProgression != null
                ? "XP " +
                  heroProgression.CurrentXP +
                  " / " +
                  heroProgression.XPToNextLevel
                : "XP -";
        }

        if (goldText != null)
        {
            goldText.text = heroProgression != null
                ? "Gold " + heroProgression.CurrentGold
                : "Gold -";
        }

        if (healthText != null)
        {
            healthText.text = heroHealth != null
                ? "HP " +
                  heroHealth.CurrentHealth +
                  " / " +
                  heroHealth.MaxHealth
                : "HP -";
        }

        if (inventoryText != null)
        {
            inventoryText.text = heroInventory != null
                ? "Inventory " +
                  heroInventory.CurrentItems +
                  " / " +
                  heroInventory.MaxItems
                : "Inventory -";
        }

        UpdateWeaponText();
        UpdateInventorySlots();
        UpdateEquipmentSlots();
    }

    private void UpdateWeaponText()
    {
        if (weaponText == null)
            return;

        if (heroInventory == null ||
            heroInventory.EquippedWeaponName == "None")
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

            itemNameTexts[i].text = heroInventory != null
                ? heroInventory.GetItemName(i)
                : "Empty";
        }
    }

    private void UpdateEquipmentSlots()
    {
        if (weaponSlotText != null)
        {
            weaponSlotText.text =
                heroInventory == null ||
                heroInventory.EquippedWeaponName == "None"
                ? "Weapon"
                : heroInventory.EquippedWeaponName;
        }

        if (helmetSlotText != null)
        {
            helmetSlotText.text =
                heroInventory == null ||
                heroInventory.EquippedHelmetName == "None"
                ? "Head"
                : heroInventory.EquippedHelmetName;
        }

        if (chestSlotText != null)
        {
            chestSlotText.text =
                heroInventory == null ||
                heroInventory.EquippedChestName == "None"
                ? "Chest"
                : heroInventory.EquippedChestName;
        }

        if (bootsSlotText != null)
        {
            bootsSlotText.text =
                heroInventory == null ||
                heroInventory.EquippedBootsName == "None"
                ? "Boots"
                : heroInventory.EquippedBootsName;
        }
    }
}
