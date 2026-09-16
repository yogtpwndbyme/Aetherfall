using UnityEngine;

[CreateAssetMenu(
    fileName = "NewAbilityData",
    menuName = "Aetherfall/Ability Data"
)]
public class AbilityData : ScriptableObject
{
    [Header("Identity")]
    public string abilityName = "New Ability";

    [TextArea(2, 4)]
    public string description = "";

    [Header("Visuals")]
    public Sprite icon;

    [Header("Combat")]
    public int damage = 40;
    public float range = 6f;

    [Header("Resource")]
    public int manaCost = 20;

    [Header("Cooldown")]
    public float cooldown = 5f;
}