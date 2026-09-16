using UnityEngine;
using UnityEngine.UI;

public class CreepHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Image healthBarFill;

    [Header("Rewards")]
    [SerializeField] private int xpReward = 25;
    [SerializeField] private int goldReward = 10;

    [Header("Loot")]
    [SerializeField] private GameObject itemDropPrefab;

    private int currentHealth;

    // Hero who most recently damaged this creep.
    private HeroProgression lastAttackingHero;

    public int MaxHealth
    {
        get { return maxHealth; }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();

        Debug.Log(
            "Creep HP: " +
            currentHealth +
            "/" +
            maxHealth
        );
    }

    public void TakeDamage(
        int damage,
        HeroProgression attackingHero = null
    )
    {
        if (attackingHero != null)
        {
            lastAttackingHero = attackingHero;
        }

        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHealthBar();

        Debug.Log(
            "Creep took " +
            damage +
            " damage."
        );

        Debug.Log(
            "Creep HP: " +
            currentHealth +
            "/" +
            maxHealth
        );

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill == null)
            return;

        float healthPercent =
            maxHealth > 0
            ? (float)currentHealth / maxHealth
            : 0f;

        healthBarFill.fillAmount =
            healthPercent;
    }

    private void Die()
    {
        Debug.Log("Creep died.");

        if (lastAttackingHero != null)
        {
            lastAttackingHero.AddXP(xpReward);
            lastAttackingHero.AddGold(goldReward);

            Debug.Log(
                lastAttackingHero.name +
                " received " +
                xpReward +
                " XP and " +
                goldReward +
                " Gold."
            );
        }
        else
        {
            Debug.LogWarning(
                "Creep died without a HeroProgression reward target."
            );
        }

        DropItem();

        Destroy(gameObject);
    }

    private void DropItem()
    {
        if (itemDropPrefab == null)
        {
            Debug.LogWarning(
                "Creep has no Item Drop Prefab assigned."
            );

            return;
        }

        Vector3 dropPosition =
            transform.position;

        dropPosition.y = 0.3f;

        Instantiate(
            itemDropPrefab,
            dropPosition,
            Quaternion.identity
        );

        Debug.Log(
            "Creep dropped an item."
        );
    }
}