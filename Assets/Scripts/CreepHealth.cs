using UnityEngine;
using UnityEngine.UI;

public class CreepHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Image healthBarFill;

    [Header("Rewards")]
    [SerializeField] private int xpReward = 25;
    [SerializeField] private int goldReward = 10;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();

        Debug.Log("Creep HP: " + currentHealth + "/" + maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHealthBar();

        Debug.Log("Creep took " + damage + " damage.");
        Debug.Log("Creep HP: " + currentHealth + "/" + maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / maxHealth;
        healthBarFill.fillAmount = healthPercent;
    }

    private void Die()
    {
        Debug.Log("Creep died.");

        HeroProgression heroProgression = FindFirstObjectByType<HeroProgression>();

        if (heroProgression != null)
        {
            heroProgression.AddXP(xpReward);
            heroProgression.AddGold(goldReward);
        }

        Destroy(gameObject);
    }
}