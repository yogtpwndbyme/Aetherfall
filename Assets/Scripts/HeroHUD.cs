using UnityEngine;
using TMPro;

public class HeroHUD : MonoBehaviour
{
    [Header("Hero")]
    [SerializeField] private HeroProgression heroProgression;
    [SerializeField] private HeroHealth heroHealth;

    [Header("HUD Text")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text goldText;

    void Update()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (heroProgression != null)
        {
            levelText.text = "Level " + heroProgression.CurrentLevel;

            xpText.text =
                "XP " +
                heroProgression.CurrentXP +
                " / " +
                heroProgression.XPToNextLevel;

            goldText.text = "Gold " + heroProgression.CurrentGold;
        }

        if (heroHealth != null)
        {
            healthText.text =
                "HP " +
                heroHealth.CurrentHealth +
                " / " +
                heroHealth.MaxHealth;
        }
    }
}