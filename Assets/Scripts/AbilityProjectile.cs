using UnityEngine;

public class AbilityProjectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float hitDistance = 0.15f;
    [SerializeField] private float targetHeightOffset = 0.5f;

    private Transform target;
    private int damage;

    // Hero who cast this projectile.
    private HeroProgression castingHero;

    private Transform originalParent;
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;

    private bool hasStoredOriginalTransform = false;

    void Awake()
    {
        StoreOriginalTransform();
    }

    private void StoreOriginalTransform()
    {
        if (hasStoredOriginalTransform)
            return;

        originalParent = transform.parent;
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;

        hasStoredOriginalTransform = true;
    }

    public void Launch(
        Transform newTarget,
        int newDamage,
        HeroProgression newCastingHero
    )
    {
        if (newTarget == null)
            return;

        StoreOriginalTransform();

        target = newTarget;
        damage = newDamage;
        castingHero = newCastingHero;

        // Detach from the casting hero so movement/rotation
        // no longer affects the projectile.
        transform.SetParent(null, true);

        gameObject.SetActive(true);

        FaceTarget();
    }

    void Update()
    {
        if (target == null)
        {
            ResetProjectile();
            return;
        }

        Vector3 targetPosition =
            GetTargetPosition();

        Vector3 direction =
            targetPosition - transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }

        transform.position =
            Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

        if (
            Vector3.Distance(
                transform.position,
                targetPosition
            ) <= hitDistance
        )
        {
            HitTarget();
        }
    }

    private Vector3 GetTargetPosition()
    {
        return target.position +
               Vector3.up * targetHeightOffset;
    }

    private void FaceTarget()
    {
        if (target == null)
            return;

        Vector3 direction =
            GetTargetPosition() -
            transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }
    }

    private void HitTarget()
    {
        if (target != null)
        {
            CreepHealth creepHealth =
                target.GetComponentInParent<CreepHealth>();

            if (creepHealth != null)
            {
                // Pass the casting hero to CreepHealth.
                // If this hit kills the creep,
                // that hero receives XP and Gold.
                creepHealth.TakeDamage(
                    damage,
                    castingHero
                );
            }
        }

        ResetProjectile();
    }

    private void ResetProjectile()
    {
        target = null;
        damage = 0;
        castingHero = null;

        // Return projectile to its original hero.
        transform.SetParent(
            originalParent,
            false
        );

        transform.localPosition =
            originalLocalPosition;

        transform.localRotation =
            originalLocalRotation;

        gameObject.SetActive(false);
    }
}