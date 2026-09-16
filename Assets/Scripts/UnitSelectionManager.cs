using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionManager : MonoBehaviour
{
    [Header("Selection Box")]
    [SerializeField] private RectTransform selectionBox;

    [Header("Unit Info")]
    [SerializeField] private UnitInfoPanel unitInfoPanel;

    [Header("Settings")]
    [SerializeField] private float dragThreshold = 10f;

    private Vector2 dragStartPosition;
    private bool isDragging;

    private bool abilityConsumedClick;

    void Update()
    {
        if (Mouse.current == null)
            return;

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            abilityConsumedClick =
                IsAbilityUsingLeftClick();

            if (!abilityConsumedClick)
            {
                BeginSelection(mousePosition);
            }
        }

        if (Mouse.current.leftButton.isPressed)
        {
            if (!abilityConsumedClick)
            {
                UpdateSelection(mousePosition);
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (abilityConsumedClick)
            {
                abilityConsumedClick = false;

                if (selectionBox != null)
                {
                    selectionBox.gameObject.SetActive(false);
                }

                isDragging = false;
                return;
            }

            EndSelection(mousePosition);
        }
    }

    private bool IsAbilityUsingLeftClick()
    {
        HeroSelection[] heroes =
            FindObjectsByType<HeroSelection>(
                FindObjectsSortMode.None
            );

        foreach (HeroSelection hero in heroes)
        {
            if (!hero.IsSelected)
                continue;

            HeroAbility ability =
                hero.GetComponent<HeroAbility>();

            if (ability == null)
                continue;

            if (
                ability.IsTargetingAbility ||
                ability.AbilityClickUsedThisFrame
            )
            {
                return true;
            }
        }

        return false;
    }

    private void BeginSelection(Vector2 mousePosition)
    {
        dragStartPosition = mousePosition;
        isDragging = false;

        if (selectionBox != null)
        {
            selectionBox.gameObject.SetActive(false);
        }
    }

    private void UpdateSelection(Vector2 mousePosition)
    {
        float distance = Vector2.Distance(
            dragStartPosition,
            mousePosition
        );

        if (distance < dragThreshold)
            return;

        isDragging = true;

        if (selectionBox == null)
            return;

        selectionBox.gameObject.SetActive(true);

        Vector2 boxStart = new Vector2(
            Mathf.Min(dragStartPosition.x, mousePosition.x),
            Mathf.Min(dragStartPosition.y, mousePosition.y)
        );

        Vector2 boxEnd = new Vector2(
            Mathf.Max(dragStartPosition.x, mousePosition.x),
            Mathf.Max(dragStartPosition.y, mousePosition.y)
        );

        selectionBox.position = boxStart;
        selectionBox.sizeDelta = boxEnd - boxStart;
    }

    private void EndSelection(Vector2 mousePosition)
    {
        if (selectionBox != null)
        {
            selectionBox.gameObject.SetActive(false);
        }

        if (isDragging)
        {
            SelectUnitsInsideBox(mousePosition);
        }
        else
        {
            HandleSingleClick(mousePosition);
        }

        isDragging = false;
    }

    private void HandleSingleClick(Vector2 mousePosition)
    {
        if (Camera.main == null)
            return;

        Ray ray =
            Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            HeroSelection clickedHero =
                hit.transform.GetComponentInParent<HeroSelection>();

            if (clickedHero != null)
            {
                DeselectAllHeroes();
                clickedHero.SetSelected(true);

                if (unitInfoPanel != null)
                {
                    unitInfoPanel.ShowUnit(
                        clickedHero.gameObject
                    );
                }

                return;
            }

            CreepHealth clickedCreep =
                hit.transform.GetComponentInParent<CreepHealth>();

            if (clickedCreep != null)
            {
                if (unitInfoPanel != null)
                {
                    unitInfoPanel.ShowUnit(
                        clickedCreep.gameObject
                    );
                }

                return;
            }
        }

        DeselectAllHeroes();

        if (unitInfoPanel != null)
        {
            unitInfoPanel.HidePanel();
        }
    }

    private void SelectUnitsInsideBox(Vector2 mousePosition)
    {
        HeroSelection[] heroes =
            FindObjectsByType<HeroSelection>(
                FindObjectsSortMode.None
            );

        Vector2 min = new Vector2(
            Mathf.Min(dragStartPosition.x, mousePosition.x),
            Mathf.Min(dragStartPosition.y, mousePosition.y)
        );

        Vector2 max = new Vector2(
            Mathf.Max(dragStartPosition.x, mousePosition.x),
            Mathf.Max(dragStartPosition.y, mousePosition.y)
        );

        HeroSelection firstSelectedHero = null;

        foreach (HeroSelection hero in heroes)
        {
            Vector3 screenPosition =
                Camera.main.WorldToScreenPoint(
                    hero.transform.position
                );

            bool insideBox =
                screenPosition.z > 0f &&
                screenPosition.x >= min.x &&
                screenPosition.x <= max.x &&
                screenPosition.y >= min.y &&
                screenPosition.y <= max.y;

            hero.SetSelected(insideBox);

            if (
                insideBox &&
                firstSelectedHero == null
            )
            {
                firstSelectedHero = hero;
            }
        }

        if (unitInfoPanel != null)
        {
            if (firstSelectedHero != null)
            {
                unitInfoPanel.ShowUnit(
                    firstSelectedHero.gameObject
                );
            }
            else
            {
                unitInfoPanel.HidePanel();
            }
        }
    }

    private void DeselectAllHeroes()
    {
        HeroSelection[] heroes =
            FindObjectsByType<HeroSelection>(
                FindObjectsSortMode.None
            );

        foreach (HeroSelection hero in heroes)
        {
            hero.SetSelected(false);
        }
    }
}