using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitInfoPanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Identity")]
    [SerializeField] private Image unitPortrait;
    [SerializeField] private TMP_Text unitNameText;
    [SerializeField] private TMP_Text unitTitleText;

    [Header("XP")]
    [SerializeField] private Image xpBarFill;

    [Header("Health")]
    [SerializeField] private Image hpBarFill;
    [SerializeField] private TMP_Text unitHPText;

    [Header("Mana")]
    [SerializeField] private Image manaBarFill;
    [SerializeField] private TMP_Text unitManaText;

    [Header("Stats")]
    [SerializeField] private TMP_Text unitDamageText;
    [SerializeField] private TMP_Text unitArmorText;
    [SerializeField] private TMP_Text unitSpellPowerText;
    [SerializeField] private TMP_Text unitHealingText;

    [Header("Abilities")]
    [SerializeField] private Image spellSlotQ;

    private GameObject currentUnit;

    void Update()
    {
        if (currentUnit != null)
        {
            RefreshPanel();
        }
    }

    public void ShowUnit(GameObject unit)
    {
        currentUnit = unit;

        if (panel != null)
        {
            panel.SetActive(true);
        }

        RefreshPanel();
    }

    public void HidePanel()
    {
        currentUnit = null;

        if (panel != null)
        {
            panel.SetActive(false);
        }

        ClearAbilityIcons();
    }

    private void RefreshPanel()
    {
        if (currentUnit == null)
            return;

        // --------------------------------------------------
        // IDENTITY
        // --------------------------------------------------

        Unit unit =
            currentUnit.GetComponent<Unit>();

        if (unit != null && unit.Data != null)
        {
            if (unitNameText != null)
            {
                unitNameText.text =
                    unit.UnitName;
            }

            if (unitTitleText != null)
            {
                unitTitleText.text =
                    unit.Title;
            }

            if (unitPortrait != null)
            {
                unitPortrait.sprite =
                    unit.Portrait;

                unitPortrait.enabled =
                    unit.Portrait != null;
            }
        }
        else
        {
            UnitInfoData unitInfo =
                currentUnit.GetComponent<UnitInfoData>();

            if (unitInfo != null)
            {
                if (unitNameText != null)
                {
                    unitNameText.text =
                        unitInfo.DisplayName;
                }

                if (unitTitleText != null)
                {
                    unitTitleText.text =
                        unitInfo.Title;
                }

                if (unitPortrait != null)
                {
                    unitPortrait.sprite =
                        unitInfo.Portrait;

                    unitPortrait.enabled =
                        unitInfo.Portrait != null;
                }
            }
            else
            {
                if (unitNameText != null)
                {
                    unitNameText.text =
                        currentUnit.name;
                }

                if (unitTitleText != null)
                {
                    unitTitleText.text = "";
                }

                if (unitPortrait != null)
                {
                    unitPortrait.sprite = null;
                    unitPortrait.enabled = false;
                }
            }
        }

        // --------------------------------------------------
        // HERO
        // --------------------------------------------------

        HeroHealth heroHealth =
            currentUnit.GetComponent<HeroHealth>();

        if (heroHealth != null)
        {
            // --------------------------------------------------
            // HEALTH
            // --------------------------------------------------

            UpdateHealth(
                heroHealth.CurrentHealth,
                heroHealth.MaxHealth
            );

            // --------------------------------------------------
            // XP / LEVEL / TITLE
            // --------------------------------------------------

            HeroProgression heroProgression =
                currentUnit.GetComponent<HeroProgression>();

            if (heroProgression != null)
            {
                if (unitTitleText != null)
                {
                    string title = "";

                    if (unit != null && unit.Data != null)
                    {
                        title = unit.Title;
                    }

                    unitTitleText.text =
                        "Level " +
                        heroProgression.CurrentLevel +
                        "  |  " +
                        title;
                }

                if (xpBarFill != null)
                {
                    xpBarFill.fillAmount =
                        heroProgression.XPToNextLevel > 0
                        ? (float)heroProgression.CurrentXP /
                          heroProgression.XPToNextLevel
                        : 0f;
                }
            }
            else
            {
                if (xpBarFill != null)
                {
                    xpBarFill.fillAmount = 0f;
                }
            }

            // --------------------------------------------------
            // MANA
            // --------------------------------------------------

            HeroMana heroMana =
                currentUnit.GetComponent<HeroMana>();

            if (heroMana != null)
            {
                if (unitManaText != null)
                {
                    unitManaText.text =
                        heroMana.CurrentMana +
                        " / " +
                        heroMana.MaxMana;
                }

                if (manaBarFill != null)
                {
                    manaBarFill.fillAmount =
                        heroMana.MaxMana > 0
                        ? (float)heroMana.CurrentMana /
                          heroMana.MaxMana
                        : 0f;
                }
            }
            else
            {
                if (unitManaText != null)
                {
                    unitManaText.text = "";
                }

                if (manaBarFill != null)
                {
                    manaBarFill.fillAmount = 0f;
                }
            }

            // --------------------------------------------------
            // DAMAGE
            // --------------------------------------------------

            HeroCombat heroCombat =
                currentUnit.GetComponent<HeroCombat>();

            if (unitDamageText != null)
            {
                unitDamageText.text =
                    heroCombat != null
                    ? heroCombat.TotalDamage.ToString()
                    : "0";
            }

            // --------------------------------------------------
            // ARMOR
            // --------------------------------------------------

            HeroInventory heroInventory =
                currentUnit.GetComponent<HeroInventory>();

            if (unitArmorText != null)
            {
                unitArmorText.text =
                    heroInventory != null
                    ? heroInventory.EquippedArmorBonus.ToString()
                    : "0";
            }

            // --------------------------------------------------
            // SPELL POWER
            // --------------------------------------------------

            if (unitSpellPowerText != null)
            {
                unitSpellPowerText.text =
                    unit != null
                    ? unit.SpellPower.ToString()
                    : "0";
            }

            // --------------------------------------------------
            // HEALING
            // --------------------------------------------------

            if (unitHealingText != null)
            {
                unitHealingText.text =
                    unit != null
                    ? unit.HealingPower.ToString()
                    : "0";
            }

            // --------------------------------------------------
            // ABILITIES
            // --------------------------------------------------

            UpdateAbilityIcons();

            return;
        }

        // --------------------------------------------------
        // CREEP
        // --------------------------------------------------

        CreepHealth creepHealth =
            currentUnit.GetComponent<CreepHealth>();

        if (creepHealth != null)
        {
            UpdateHealth(
                creepHealth.CurrentHealth,
                creepHealth.MaxHealth
            );

            if (xpBarFill != null)
            {
                xpBarFill.fillAmount = 0f;
            }

            if (unitManaText != null)
            {
                unitManaText.text = "";
            }

            if (manaBarFill != null)
            {
                manaBarFill.fillAmount = 0f;
            }

            CreepCombat creepCombat =
                currentUnit.GetComponent<CreepCombat>();

            if (unitDamageText != null)
            {
                unitDamageText.text =
                    creepCombat != null
                    ? creepCombat.AttackDamage.ToString()
                    : "0";
            }

            if (unitArmorText != null)
            {
                unitArmorText.text = "0";
            }

            if (unitSpellPowerText != null)
            {
                unitSpellPowerText.text = "0";
            }

            if (unitHealingText != null)
            {
                unitHealingText.text = "0";
            }

            // Creeps currently have no abilities.
            ClearAbilityIcons();
        }
    }

    private void UpdateAbilityIcons()
    {
        if (spellSlotQ == null)
            return;

        HeroAbility heroAbility =
            currentUnit.GetComponent<HeroAbility>();

        if (
            heroAbility != null &&
            heroAbility.QAbility != null &&
            heroAbility.QAbility.icon != null
        )
        {
            spellSlotQ.sprite =
                heroAbility.QAbility.icon;

            spellSlotQ.color =
                Color.white;
        }
        else
        {
            ClearAbilityIcons();
        }
    }

    private void ClearAbilityIcons()
    {
        if (spellSlotQ != null)
        {
            spellSlotQ.sprite = null;

            spellSlotQ.color =
                new Color32(
                    48,
                    55,
                    64,
                    255
                );
        }
    }

    private void UpdateHealth(
        int currentHealth,
        int maxHealth
    )
    {
        if (unitHPText != null)
        {
            unitHPText.text =
                currentHealth +
                " / " +
                maxHealth;
        }

        if (hpBarFill != null)
        {
            hpBarFill.fillAmount =
                maxHealth > 0
                ? (float)currentHealth / maxHealth
                : 0f;
        }
    }
}