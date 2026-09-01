using UnityEngine;
using UnityEngine.InputSystem;

public class HeroMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private HeroSelection heroSelection;
    private Vector3 targetPosition;
    private bool hasTarget = false;

    void Start()
    {
        heroSelection = GetComponent<HeroSelection>();
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleRightClick();
        MoveHero();
    }

    private void HandleRightClick()
    {
        if (!heroSelection.IsSelected)
            return;

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.name == "Ground")
                {
                    targetPosition = hit.point;
                    targetPosition.y = transform.position.y;

                    hasTarget = true;
                }
            }
        }
    }

    private void MoveHero()
    {
        if (!hasTarget)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            transform.position = targetPosition;
            hasTarget = false;
        }
    }
}