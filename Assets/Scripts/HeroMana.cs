using UnityEngine;

public class HeroMana : MonoBehaviour
{
    [Header("Mana")]
    [SerializeField] private int maxMana = 100;

    private int currentMana;

    private Unit unit;

    public int MaxMana
    {
        get { return maxMana; }
    }

    public int CurrentMana
    {
        get { return currentMana; }
    }

    void Start()
    {
        unit = GetComponent<Unit>();

        // Load base max mana from UnitData.
        if (unit != null && unit.Data != null)
        {
            maxMana = unit.MaxMana;

            Debug.Log(
                unit.UnitName +
                " max mana loaded from UnitData: " +
                maxMana
            );
        }
        else
        {
            Debug.LogWarning(
                gameObject.name +
                " has no UnitData. Using HeroMana fallback Mana: " +
                maxMana
            );
        }

        currentMana = maxMana;

        Debug.Log(
            gameObject.name +
            " Mana: " +
            currentMana +
            "/" +
            maxMana
        );
    }

    public bool HasEnoughMana(int amount)
    {
        return currentMana >= amount;
    }

    public bool SpendMana(int amount)
    {
        if (amount <= 0)
            return true;

        if (!HasEnoughMana(amount))
        {
            Debug.Log(
                gameObject.name +
                " does not have enough mana."
            );

            return false;
        }

        currentMana -= amount;

        if (currentMana < 0)
        {
            currentMana = 0;
        }

        Debug.Log(
            gameObject.name +
            " spent " +
            amount +
            " Mana. " +
            currentMana +
            "/" +
            maxMana
        );

        return true;
    }

    public void RestoreMana(int amount)
    {
        if (amount <= 0)
            return;

        currentMana += amount;

        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }

        Debug.Log(
            gameObject.name +
            " restored " +
            amount +
            " Mana. " +
            currentMana +
            "/" +
            maxMana
        );
    }

    public void RestoreFullMana()
    {
        currentMana = maxMana;

        Debug.Log(
            gameObject.name +
            " restored to full Mana: " +
            currentMana +
            "/" +
            maxMana
        );
    }
}