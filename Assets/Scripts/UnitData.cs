using UnityEngine;

[CreateAssetMenu(
    fileName = "NewUnitData",
    menuName = "Aetherfall/Unit Data"
)]
public class UnitData : ScriptableObject
{
    public enum UnitRole
    {
        Worker,
        Tank,
        Bruiser,
        Marksman,
        Mage
    }

    [Header("Identity")]
    public string unitName = "New Unit";
    public string title = "";
    public UnitRole role;

    [TextArea(2, 4)]
    public string description;

    [Header("Visuals")]
    public Sprite portrait;

    [Header("Health")]
    public int maxHealth = 100;

    [Header("Mana")]
    public int maxMana = 100;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Combat")]
    public int attackDamage = 10;
    public float attackRange = 2f;
    public float attackCooldown = 1f;

    [Header("Magic Stats")]
    public int spellPower = 0;
    public int healingPower = 0;
}