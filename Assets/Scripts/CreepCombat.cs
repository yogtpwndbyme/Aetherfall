using UnityEngine;

public class CreepCombat : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackRange = 3.5f;
    [SerializeField] private float attackCooldown = 1.5f;

    private HeroHealth targetHero;
    private float attackTimer;

    public int AttackDamage
    {
        get { return attackDamage; }
    }

    void Update()
    {
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        FindNearestHero();

        if (targetHero == null)
            return;

        Vector3 creepPosition = transform.position;
        Vector3 heroPosition = targetHero.transform.position;

        creepPosition.y = 0f;
        heroPosition.y = 0f;

        float distanceToHero = Vector3.Distance(
            creepPosition,
            heroPosition
        );

        if (distanceToHero <= attackRange)
        {
            TryAttack();
        }
    }

    private void FindNearestHero()
    {
        HeroHealth[] heroes =
            FindObjectsByType<HeroHealth>(
                FindObjectsSortMode.None
            );

        HeroHealth nearestHero = null;
        float nearestDistance = Mathf.Infinity;

        foreach (HeroHealth hero in heroes)
        {
            if (hero.IsDead)
                continue;

            Vector3 direction =
                hero.transform.position - transform.position;

            direction.y = 0f;

            float distance = direction.sqrMagnitude;

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestHero = hero;
            }
        }

        targetHero = nearestHero;
    }

    private void TryAttack()
    {
        if (attackTimer > 0f)
            return;

        if (targetHero == null)
            return;

        targetHero.TakeDamage(attackDamage);

        Debug.Log(
            name +
            " attacked " +
            targetHero.name +
            " for " +
            attackDamage +
            " incoming damage."
        );

        attackTimer = attackCooldown;
    }
}