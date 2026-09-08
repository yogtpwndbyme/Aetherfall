using UnityEngine;
using UnityEngine.InputSystem;

public class HeroMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Combat")]
    [SerializeField] private float attackRange = 2f;

    private HeroSelection heroSelection;
    private HeroAbility heroAbility;

    private Vector3 targetPosition;
    private bool hasMoveTarget = false;

    private GameObject attackTarget;

    public GameObject AttackTarget
    {
        get { return attackTarget; }
    }

    public float AttackRange
    {
        get { return attackRange; }
    }

    void Start()
    {
        heroSelection = GetComponent<HeroSelection>();
        heroAbility = GetComponent<HeroAbility>();

        targetPosition = transform.position;
    }

    void Update()
    {
        HandleLeftClickAttack();
        HandleRightClickMovement();

        if (attackTarget != null)
        {
            ChaseAttackTarget();
        }
        else
        {
            MoveHero();
        }
    }

    private void HandleLeftClickAttack()
    {
        if (!heroSelection.IsSelected)
            return;

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        if (heroAbility != null)
        {
            if (heroAbility.IsTargetingAbility)
                return;

            if (heroAbility.AbilityClickUsedThisFrame)
                return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                attackTarget = hit.transform.gameObject;
                hasMoveTarget = false;

                Debug.Log("Attack target selected: " + attackTarget.name);
            }
        }
    }

    private void HandleRightClickMovement()
    {
        if (!heroSelection.IsSelected)
            return;

        if (!Mouse.current.rightButton.wasPressedThisFrame)
            return;

        if (heroAbility != null && heroAbility.IsTargetingAbility)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                attackTarget = hit.transform.gameObject;
                hasMoveTarget = false;

                Debug.Log("Attack target selected: " + attackTarget.name);
            }
            else if (hit.transform.name == "Ground")
            {
                attackTarget = null;

                targetPosition = hit.point;
                targetPosition.y = transform.position.y;

                hasMoveTarget = true;
            }
        }
    }

    private void ChaseAttackTarget()
    {
        Vector3 direction = attackTarget.transform.position - transform.position;
        direction.y = 0f;

        float distanceToTarget = direction.magnitude;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        if (distanceToTarget > attackRange)
        {
            Vector3 target = attackTarget.transform.position;
            target.y = transform.position.y;

            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime
            );
        }
    }

    private void MoveHero()
    {
        if (!hasMoveTarget)
            return;

        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            transform.position = targetPosition;
            hasMoveTarget = false;
        }
    }
}