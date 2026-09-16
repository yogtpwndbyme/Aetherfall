using UnityEngine;
using UnityEngine.InputSystem;

public class HeroMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Combat")]
    [SerializeField] private float attackRange = 2f;

    private HeroSelection heroSelection;
    private HeroAbility heroAbility;
    private Unit unit;

    private Vector3 targetPosition;
    private bool hasMoveTarget = false;

    private GameObject attackTarget;

    // Ability movement
    private Transform abilityMoveTarget;
    private float abilityMoveRange;
    private bool isMovingForAbility = false;

    public GameObject AttackTarget
    {
        get { return attackTarget; }
    }

    public float AttackRange
    {
        get { return attackRange; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    void Start()
    {
        heroSelection = GetComponent<HeroSelection>();
        heroAbility = GetComponent<HeroAbility>();
        unit = GetComponent<Unit>();

        // Load movement and basic attack range from UnitData.
        if (unit != null && unit.Data != null)
        {
            moveSpeed = unit.MoveSpeed;
            attackRange = unit.AttackRange;

            Debug.Log(
                unit.UnitName +
                " movement/combat range loaded from UnitData. " +
                "Move Speed: " +
                moveSpeed +
                ", Attack Range: " +
                attackRange
            );
        }
        else
        {
            Debug.LogWarning(
                gameObject.name +
                " has no UnitData. Using HeroMovement fallback values. " +
                "Move Speed: " +
                moveSpeed +
                ", Attack Range: " +
                attackRange
            );
        }

        targetPosition = transform.position;
    }

    void Update()
    {
        HandleRightClickOrders();

        // Ability movement gets priority over normal movement
        // and normal attack chasing.
        if (isMovingForAbility)
        {
            MoveTowardAbilityTarget();
        }
        else if (attackTarget != null)
        {
            ChaseAttackTarget();
        }
        else
        {
            MoveHero();
        }
    }

    private void HandleRightClickOrders()
    {
        if (heroSelection == null || !heroSelection.IsSelected)
            return;

        if (Mouse.current == null)
            return;

        if (!Mouse.current.rightButton.wasPressedThisFrame)
            return;

        // If Q targeting or a pending Q order exists,
        // HeroAbility owns this right-click and uses it to cancel Q.
        if (
            heroAbility != null &&
            (
                heroAbility.IsTargetingAbility ||
                heroAbility.HasPendingAbilityTarget
            )
        )
        {
            return;
        }

        if (Camera.main == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            CreepHealth enemy =
                hit.transform.GetComponentInParent<CreepHealth>();

            if (enemy != null)
            {
                CancelAbilityMovement();

                attackTarget = enemy.gameObject;
                hasMoveTarget = false;

                Debug.Log(
                    "Attack order: " +
                    attackTarget.name
                );

                return;
            }

            if (hit.transform.name == "Ground")
            {
                CancelAbilityMovement();

                attackTarget = null;

                targetPosition = hit.point;
                targetPosition.y = transform.position.y;

                hasMoveTarget = true;

                Debug.Log("Move order.");
            }
        }
    }

    public void MoveToAbilityTarget(
        Transform target,
        float requiredRange
    )
    {
        if (target == null)
            return;

        abilityMoveTarget = target;
        abilityMoveRange = requiredRange;
        isMovingForAbility = true;

        // A Q order replaces previous movement/attack orders.
        attackTarget = null;
        hasMoveTarget = false;

        Debug.Log(
            "Moving toward ability target: " +
            target.name
        );
    }

    public void CancelAbilityMovement()
    {
        abilityMoveTarget = null;
        isMovingForAbility = false;
    }

    private void MoveTowardAbilityTarget()
    {
        if (abilityMoveTarget == null)
        {
            CancelAbilityMovement();
            return;
        }

        Vector3 direction =
            abilityMoveTarget.position -
            transform.position;

        direction.y = 0f;

        float distanceToTarget =
            direction.magnitude;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
        }

        if (distanceToTarget <= abilityMoveRange)
        {
            isMovingForAbility = false;
            return;
        }

        Vector3 abilityTargetPosition =
            abilityMoveTarget.position;

        abilityTargetPosition.y =
            transform.position.y;

        transform.position =
            Vector3.MoveTowards(
                transform.position,
                abilityTargetPosition,
                moveSpeed * Time.deltaTime
            );
    }

    private void ChaseAttackTarget()
    {
        if (attackTarget == null)
            return;

        Vector3 direction =
            attackTarget.transform.position -
            transform.position;

        direction.y = 0f;

        float distanceToTarget =
            direction.magnitude;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
        }

        if (distanceToTarget > attackRange)
        {
            Vector3 target =
                attackTarget.transform.position;

            target.y =
                transform.position.y;

            transform.position =
                Vector3.MoveTowards(
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

        Vector3 direction =
            targetPosition -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
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
            ) < 0.05f
        )
        {
            transform.position =
                targetPosition;

            hasMoveTarget = false;
        }
    }
}