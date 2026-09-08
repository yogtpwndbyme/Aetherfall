using UnityEngine;

public class HeroHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;

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

        Debug.Log("Hero HP: " + currentHealth + "/" + maxHealth);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;

        Debug.Log("Hero max HP increased by " + amount + ".");
        Debug.Log("Hero HP: " + currentHealth + "/" + maxHealth);
    }
}