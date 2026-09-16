using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Unit Data")]
    [SerializeField] private UnitData unitData;

    public UnitData Data
    {
        get { return unitData; }
    }

    public string UnitName
    {
        get
        {
            return unitData != null
                ? unitData.unitName
                : gameObject.name;
        }
    }

    public string Title
    {
        get
        {
            return unitData != null
                ? unitData.title
                : "";
        }
    }

    public UnitData.UnitRole Role
    {
        get
        {
            return unitData != null
                ? unitData.role
                : UnitData.UnitRole.Bruiser;
        }
    }

    public int MaxHealth
    {
        get
        {
            return unitData != null
                ? unitData.maxHealth
                : 100;
        }
    }

    public int MaxMana
    {
        get
        {
            return unitData != null
                ? unitData.maxMana
                : 100;
        }
    }

    public float MoveSpeed
    {
        get
        {
            return unitData != null
                ? unitData.moveSpeed
                : 5f;
        }
    }

    public int AttackDamage
    {
        get
        {
            return unitData != null
                ? unitData.attackDamage
                : 10;
        }
    }

    public float AttackRange
    {
        get
        {
            return unitData != null
                ? unitData.attackRange
                : 2f;
        }
    }

    public float AttackCooldown
    {
        get
        {
            return unitData != null
                ? unitData.attackCooldown
                : 1f;
        }
    }

    public int SpellPower
    {
        get
        {
            return unitData != null
                ? unitData.spellPower
                : 0;
        }
    }

    public int HealingPower
    {
        get
        {
            return unitData != null
                ? unitData.healingPower
                : 0;
        }
    }

    public Sprite Portrait
    {
        get
        {
            return unitData != null
                ? unitData.portrait
                : null;
        }
    }

    void Start()
    {
        if (unitData == null)
        {
            Debug.LogWarning(
                gameObject.name +
                " does not have UnitData assigned."
            );

            return;
        }

        Debug.Log(
            "Loaded UnitData for " +
            unitData.unitName +
            " - " +
            unitData.title +
            " (" +
            unitData.role +
            ")"
        );
    }
}