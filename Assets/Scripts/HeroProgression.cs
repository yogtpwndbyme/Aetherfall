using UnityEngine;

public class HeroProgression : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int maxLevel = 25;

    [Header("XP")]
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int xpToNextLevel = 100;
    [SerializeField] private int xpIncreasePerLevel = 50;

    [Header("Gold")]
    [SerializeField] private int currentGold = 0;

    [Header("Level Up Rewards")]
    [SerializeField] private int healthGainPerLevel = 10;

    private HeroHealth heroHealth;

    public int CurrentLevel
    {
        get { return currentLevel; }
    }

    public int CurrentXP
    {
        get { return currentXP; }
    }

    public int XPToNextLevel
    {
        get { return xpToNextLevel; }
    }

    public int CurrentGold
    {
        get { return currentGold; }
    }

    void Start()
    {
        heroHealth = GetComponent<HeroHealth>();
    }

    public void AddXP(int amount)
    {
        if (currentLevel >= maxLevel)
            return;

        currentXP += amount;

        Debug.Log("Hero gained " + amount + " XP.");

        CheckForLevelUp();

        Debug.Log(
            "Hero Level: " + currentLevel +
            " | XP: " + currentXP + "/" + xpToNextLevel
        );
    }

    public void AddGold(int amount)
    {
        currentGold += amount;

        Debug.Log("Hero gained " + amount + " Gold.");
        Debug.Log("Hero Gold: " + currentGold);
    }

    private void CheckForLevelUp()
    {
        while (currentXP >= xpToNextLevel && currentLevel < maxLevel)
        {
            currentXP -= xpToNextLevel;
            currentLevel++;

            Debug.Log("LEVEL UP! Hero reached Level " + currentLevel + "!");

            if (heroHealth != null)
            {
                heroHealth.IncreaseMaxHealth(healthGainPerLevel);
            }

            xpToNextLevel += xpIncreasePerLevel;
        }

        if (currentLevel >= maxLevel)
        {
            currentLevel = maxLevel;
            currentXP = 0;

            Debug.Log("Hero has reached MAX LEVEL!");
        }
    }
}