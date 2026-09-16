using System.Collections;
using UnityEngine;

public class HeroHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("Respawn")]
    [SerializeField] private float respawnDelay = 3f;

    [Header("Death UI")]
    [SerializeField] private GameObject deathText;

    private int currentHealth;
    private bool isDead = false;

    private Unit unit;
    private HeroInventory heroInventory;
    private HeroMovement heroMovement;
    private HeroCombat heroCombat;
    private HeroAbility heroAbility;

    private Vector3 spawnPosition;

    public int MaxHealth
    {
        get { return maxHealth; }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    public bool IsDead
    {
        get { return isDead; }
    }

    void Start()
    {
        unit = GetComponent<Unit>();

        // Load base max health from UnitData.
        if (unit != null && unit.Data != null)
        {
            maxHealth = unit.MaxHealth;

            Debug.Log(
                unit.UnitName +
                " max health loaded from UnitData: " +
                maxHealth
            );
        }
        else
        {
            Debug.LogWarning(
                gameObject.name +
                " has no UnitData. Using HeroHealth fallback HP: " +
                maxHealth
            );
        }

        currentHealth = maxHealth;

        heroInventory = GetComponent<HeroInventory>();
        heroMovement = GetComponent<HeroMovement>();
        heroCombat = GetComponent<HeroCombat>();
        heroAbility = GetComponent<HeroAbility>();

        spawnPosition = transform.position;

        if (deathText != null)
        {
            deathText.SetActive(false);
        }

        Debug.Log(
            "Hero HP: " +
            currentHealth +
            "/" +
            maxHealth
        );
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        int armor = 0;

        if (heroInventory != null)
        {
            armor = heroInventory.EquippedArmorBonus;
        }

        int finalDamage = damage - armor;

        if (finalDamage < 1)
        {
            finalDamage = 1;
        }

        currentHealth -= finalDamage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        Debug.Log(
            "Hero took " +
            finalDamage +
            " damage. Armor reduced " +
            damage +
            " incoming damage by " +
            armor +
            "."
        );

        Debug.Log(
            "Hero HP: " +
            currentHealth +
            "/" +
            maxHealth
        );

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;

        if (!isDead)
        {
            currentHealth += amount;
        }

        Debug.Log(
            "Hero max HP increased by " +
            amount +
            "."
        );

        Debug.Log(
            "Hero HP: " +
            currentHealth +
            "/" +
            maxHealth
        );
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("Hero died.");

        SetHeroControl(false);

        if (deathText != null)
        {
            deathText.SetActive(true);
        }

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        transform.position = spawnPosition;

        currentHealth = maxHealth;
        isDead = false;

        if (deathText != null)
        {
            deathText.SetActive(false);
        }

        SetHeroControl(true);

        Debug.Log("Hero respawned.");

        Debug.Log(
            "Hero HP: " +
            currentHealth +
            "/" +
            maxHealth
        );
    }

    private void SetHeroControl(bool enabled)
    {
        if (heroMovement != null)
        {
            heroMovement.enabled = enabled;
        }

        if (heroCombat != null)
        {
            heroCombat.enabled = enabled;
        }

        if (heroAbility != null)
        {
            heroAbility.enabled = enabled;
        }
    }
}