using UnityEngine;
using UnityEngine.InputSystem;

public class HeroAbility : MonoBehaviour
{
    [Header("Q Ability")]
    [SerializeField] private AbilityData qAbility;

    [Header("Projectile")]
    [SerializeField] private AbilityProjectile abilityProjectile;

    private HeroMovement heroMovement;
    private HeroMana heroMana;
    private HeroSelection heroSelection;
    private HeroProgression heroProgression;

    private float abilityTimer;
    private bool isTargetingAbility = false;

    // True during the frame where this ability
    // consumed the left mouse click.
    private bool abilityClickUsedThisFrame = false;

    // Enemy currently targeted by Q.
    private Transform pendingAbilityTarget;

    public bool IsTargetingAbility
    {
        get { return isTargetingAbility; }
    }

    public bool AbilityClickUsedThisFrame
    {
        get { return abilityClickUsedThisFrame; }
    }

    public bool HasPendingAbilityTarget
    {
        get { return pendingAbilityTarget != null; }
    }

    public Transform PendingAbilityTarget
    {
        get { return pendingAbilityTarget; }
    }

    public AbilityData QAbility
    {
        get { return qAbility; }
    }

    public float AbilityRange
    {
        get
        {
            return qAbility != null
                ? qAbility.range
                : 0f;
        }
    }

    public int ManaCost
    {
        get
        {
            return qAbility != null
                ? qAbility.manaCost
                : 0;
        }
    }

    public float CooldownRemaining
    {
        get { return Mathf.Max(abilityTimer, 0f); }
    }

    public bool IsOnCooldown
    {
        get { return abilityTimer > 0f; }
    }

    void Start()
    {
        heroMovement =
            GetComponent<HeroMovement>();

        heroMana =
            GetComponent<HeroMana>();

        heroSelection =
            GetComponent<HeroSelection>();

        heroProgression =
            GetComponent<HeroProgression>();

        if (qAbility == null)
        {
            Debug.LogWarning(
                gameObject.name +
                ": No Q AbilityData has been assigned."
            );
        }

        if (heroMana == null)
        {
            Debug.LogWarning(
                gameObject.name +
                " has no HeroMana component."
            );
        }

        if (heroSelection == null)
        {
            Debug.LogWarning(
                gameObject.name +
                " has no HeroSelection component."
            );
        }

        if (heroProgression == null)
        {
            Debug.LogWarning(
                gameObject.name +
                " has no HeroProgression component."
            );
        }
    }

    void Update()
    {
        abilityClickUsedThisFrame = false;

        if (abilityTimer > 0f)
        {
            abilityTimer -= Time.deltaTime;

            if (abilityTimer < 0f)
            {
                abilityTimer = 0f;
            }
        }

        // Only the selected hero may start Q.
        if (
            Keyboard.current != null &&
            Keyboard.current.qKey.wasPressedThisFrame
        )
        {
            if (
                heroSelection != null &&
                heroSelection.IsSelected
            )
            {
                StartTargetingAbility();
            }
        }

        // Q owns this left-click.
        if (
            isTargetingAbility &&
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame
        )
        {
            abilityClickUsedThisFrame = true;

            TrySelectAbilityTarget();
        }

        // Continue a pending cast while moving into range.
        if (pendingAbilityTarget != null)
        {
            UpdatePendingAbility();
        }

        // Right-click cancels Q.
        if (
            Mouse.current != null &&
            Mouse.current.rightButton.wasPressedThisFrame
        )
        {
            if (
                isTargetingAbility ||
                pendingAbilityTarget != null
            )
            {
                CancelAbilityOrder();
            }
        }
    }

    private void StartTargetingAbility()
    {
        if (
            heroSelection == null ||
            !heroSelection.IsSelected
        )
        {
            return;
        }

        if (qAbility == null)
        {
            Debug.LogWarning(
                gameObject.name +
                ": Cannot cast Q because no AbilityData is assigned."
            );

            return;
        }

        if (abilityTimer > 0f)
        {
            Debug.Log(
                gameObject.name +
                ": " +
                qAbility.abilityName +
                " is on cooldown."
            );

            return;
        }

        if (abilityProjectile == null)
        {
            Debug.LogWarning(
                gameObject.name +
                ": Ability Projectile has not been assigned."
            );

            return;
        }

        if (heroMana == null)
        {
            Debug.LogWarning(
                gameObject.name +
                ": Cannot cast Q because HeroMana is missing."
            );

            return;
        }

        if (!heroMana.HasEnoughMana(qAbility.manaCost))
        {
            Debug.Log(
                gameObject.name +
                ": Not enough mana to cast " +
                qAbility.abilityName +
                "."
            );

            return;
        }

        pendingAbilityTarget = null;

        if (heroMovement != null)
        {
            heroMovement.CancelAbilityMovement();
        }

        isTargetingAbility = true;

        Debug.Log(
            gameObject.name +
            ": " +
            qAbility.abilityName +
            " targeting mode active."
        );
    }

    private void TrySelectAbilityTarget()
    {
        if (Camera.main == null)
            return;

        if (Mouse.current == null)
            return;

        Ray ray =
            Camera.main.ScreenPointToRay(
                Mouse.current.position.ReadValue()
            );

        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log(
                "Q did not hit a valid target."
            );

            return;
        }

        CreepHealth enemy =
            hit.transform.GetComponentInParent<CreepHealth>();

        if (enemy == null)
        {
            Debug.Log(
                "Invalid Q target."
            );

            return;
        }

        pendingAbilityTarget =
            enemy.transform;

        isTargetingAbility = false;

        Debug.Log(
            gameObject.name +
            " selected Q target: " +
            pendingAbilityTarget.name
        );

        float distanceToTarget =
            GetDistanceToTarget(
                pendingAbilityTarget
            );

        if (distanceToTarget <= qAbility.range)
        {
            CastAbility();
        }
        else
        {
            if (heroMovement != null)
            {
                heroMovement.MoveToAbilityTarget(
                    pendingAbilityTarget,
                    qAbility.range
                );
            }

            Debug.Log(
                gameObject.name +
                ": Q target is out of range. " +
                "Moving into range."
            );
        }
    }

    private void UpdatePendingAbility()
    {
        if (pendingAbilityTarget == null)
            return;

        if (qAbility == null)
        {
            CancelAbilityOrder();
            return;
        }

        float distanceToTarget =
            GetDistanceToTarget(
                pendingAbilityTarget
            );

        if (distanceToTarget <= qAbility.range)
        {
            CastAbility();
        }
    }

    private float GetDistanceToTarget(
        Transform target
    )
    {
        Vector3 direction =
            target.position -
            transform.position;

        direction.y = 0f;

        return direction.magnitude;
    }

    private void CastAbility()
    {
        if (pendingAbilityTarget == null)
            return;

        if (qAbility == null)
            return;

        if (abilityProjectile == null)
            return;

        if (heroMana == null)
            return;

        // Mana may have changed while walking into range.
        if (!heroMana.HasEnoughMana(qAbility.manaCost))
        {
            Debug.Log(
                gameObject.name +
                ": Not enough mana to complete " +
                qAbility.abilityName +
                " cast."
            );

            CancelAbilityOrder();
            return;
        }

        if (heroMovement != null)
        {
            heroMovement.CancelAbilityMovement();
        }

        // Face target before casting.
        Vector3 direction =
            pendingAbilityTarget.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }

        // Spend mana only when Q actually fires.
        if (!heroMana.SpendMana(qAbility.manaCost))
        {
            CancelAbilityOrder();
            return;
        }

        abilityProjectile.transform.localPosition =
            new Vector3(
                0f,
                0.5f,
                0.8f
            );

        abilityProjectile.Launch(
            pendingAbilityTarget,
            qAbility.damage,
            heroProgression
        );

        Debug.Log(
            gameObject.name +
            " launched " +
            qAbility.abilityName +
            " at " +
            pendingAbilityTarget.name +
            " for " +
            qAbility.damage +
            " damage. Mana cost: " +
            qAbility.manaCost +
            "."
        );

        abilityTimer = qAbility.cooldown;
        pendingAbilityTarget = null;
        isTargetingAbility = false;
    }

    public void CancelAbilityOrder()
    {
        isTargetingAbility = false;
        pendingAbilityTarget = null;

        if (heroMovement != null)
        {
            heroMovement.CancelAbilityMovement();
        }

        Debug.Log(
            gameObject.name +
            ": Q ability order cancelled."
        );
    }
}