using UnityEngine;
using UnityEngine.InputSystem;

public class HeroAbility : MonoBehaviour
{
    [Header("Ability")]
    [SerializeField] private int abilityDamage = 40;
    [SerializeField] private float abilityRange = 6f;
    [SerializeField] private float abilityCooldown = 5f;

    [Header("Projectile")]
    [SerializeField] private AbilityProjectile abilityProjectile;

    private HeroMovement heroMovement;
    private float abilityTimer;
    private bool isTargetingAbility = false;
    private bool abilityClickUsedThisFrame = false;

    public bool IsTargetingAbility
    {
        get { return isTargetingAbility; }
    }

    public bool AbilityClickUsedThisFrame
    {
        get { return abilityClickUsedThisFrame; }
    }

    void Start()
    {
        heroMovement = GetComponent<HeroMovement>();
    }

    void Update()
    {
        abilityClickUsedThisFrame = false;

        if (abilityTimer > 0f)
        {
            abilityTimer -= Time.deltaTime;
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            StartTargetingAbility();
        }

        if (isTargetingAbility && Mouse.current.leftButton.wasPressedThisFrame)
        {
            TrySelectAbilityTarget();
        }

        if (isTargetingAbility && Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelTargetingAbility();
        }
    }

    private void StartTargetingAbility()
    {
        if (abilityTimer > 0f)
        {
            Debug.Log("Ability is on cooldown.");
            return;
        }

        if (abilityProjectile == null)
        {
            Debug.LogWarning("Ability Projectile has not been assigned.");
            return;
        }

        isTargetingAbility = true;

        Debug.Log("Q targeting mode active. Select an Enemy.");
    }

    private void TrySelectAbilityTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;

        if (!hit.transform.CompareTag("Enemy"))
        {
            Debug.Log("Invalid Q target.");
            return;
        }

        float distanceToTarget = Vector3.Distance(
            transform.position,
            hit.transform.position
        );

        if (distanceToTarget > abilityRange)
        {
            Debug.Log("Target is out of ability range.");
            return;
        }

        abilityProjectile.transform.localPosition =
            new Vector3(0f, 0.5f, 0.8f);

        abilityProjectile.Launch(
            hit.transform,
            abilityDamage
        );

        Debug.Log(
            "Hero launched Q ability at " +
            hit.transform.name +
            "."
        );

        abilityClickUsedThisFrame = true;
        abilityTimer = abilityCooldown;
        isTargetingAbility = false;
    }

    private void CancelTargetingAbility()
    {
        isTargetingAbility = false;

        Debug.Log("Q targeting cancelled.");
    }
}