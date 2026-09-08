using UnityEngine;

public class AbilityProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 12f;

    private Transform target;
    private int damage;

    public void Launch(Transform newTarget, int newDamage)
    {
        target = newTarget;
        damage = newDamage;

        gameObject.SetActive(true);
    }

    void Update()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        CreepHealth creepHealth = target.GetComponent<CreepHealth>();

        if (creepHealth != null)
        {
            creepHealth.TakeDamage(damage);
        }

        gameObject.SetActive(false);
    }
}