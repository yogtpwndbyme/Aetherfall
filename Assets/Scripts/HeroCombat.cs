using System.Collections;
using UnityEngine;

public class HeroCombat : MonoBehaviour
{
    [SerializeField] private int attackDamage = 25;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Attack Feedback")]
    [SerializeField] private Transform attackVisual;
    [SerializeField] private float punchDistance = 0.35f;
    [SerializeField] private float punchDuration = 0.15f;

    private HeroMovement heroMovement;
    private float attackTimer;

    private Vector3 attackVisualStartPosition;

    void Start()
    {
        heroMovement = GetComponent<HeroMovement>();

        if (attackVisual != null)
        {
            attackVisualStartPosition = attackVisual.localPosition;
        }
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;

        TryAttack();
    }

    private void TryAttack()
    {
        GameObject target = heroMovement.AttackTarget;

        if (target == null)
            return;

        float distanceToTarget = Vector3.Distance(
            transform.position,
            target.transform.position
        );

        if (distanceToTarget > heroMovement.AttackRange)
            return;

        if (attackTimer > 0f)
            return;

        CreepHealth creepHealth = target.GetComponent<CreepHealth>();

        if (creepHealth == null)
            return;

        creepHealth.TakeDamage(attackDamage);

        Debug.Log("Hero attacked Creep for " + attackDamage + " damage.");

        if (attackVisual != null)
        {
            StartCoroutine(PlayAttackFeedback());
        }

        attackTimer = attackCooldown;
    }

    private IEnumerator PlayAttackFeedback()
    {
        Vector3 forwardPosition =
            attackVisualStartPosition + Vector3.forward * punchDistance;

        float halfDuration = punchDuration / 2f;
        float timer = 0f;

        while (timer < halfDuration)
        {
            timer += Time.deltaTime;

            attackVisual.localPosition = Vector3.Lerp(
                attackVisualStartPosition,
                forwardPosition,
                timer / halfDuration
            );

            yield return null;
        }

        timer = 0f;

        while (timer < halfDuration)
        {
            timer += Time.deltaTime;

            attackVisual.localPosition = Vector3.Lerp(
                forwardPosition,
                attackVisualStartPosition,
                timer / halfDuration
            );

            yield return null;
        }

        attackVisual.localPosition = attackVisualStartPosition;
    }
}