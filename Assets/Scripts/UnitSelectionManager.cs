
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance
    {
        get;
        private set;
    }

    public HeroSelection MainSelectedHero
    {
        get;
        private set;
    }

    public HeroInventory MainSelectedInventory
    {
        get
        {
            if (MainSelectedHero == null)
                return null;

            return MainSelectedHero
                .GetComponent<HeroInventory>();
        }
    }

    [Header("Selection Box")]
    [SerializeField] private RectTransform selectionBox;

    [Header("Unit Info")]
    [SerializeField] private UnitInfoPanel unitInfoPanel;

    [Header("Settings")]
    [SerializeField] private float dragThreshold = 10f;

    private Vector2 dragStartPosition;
    private bool isDragging;
    private bool abilityConsumedClick;
    private bool uiConsumedClick;

    void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    void Update()
    {
        if (Mouse.current == null)
            return;

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            uiConsumedClick =
                EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject();

            if (uiConsumedClick)
                return;

            abilityConsumedClick =
                IsAbilityUsingLeftClick();

            if (!abilityConsumedClick)
            {
                BeginSelection(mousePosition);
            }
        }

        if (uiConsumedClick)
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                uiConsumedClick = false;
            }

            return;
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

                SetMainSelectedHero(clickedHero);

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
        if (Camera.main == null)
            return;

        HeroSelection[] heroes =
            FindObjectsByType<HeroSelection>(
                FindObjectsSortMode.InstanceID
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

        if (
            MainSelectedHero != null &&
            MainSelectedHero.IsSelected
        )
        {
            SetMainSelectedHero(MainSelectedHero);
        }
        else
        {
            SetMainSelectedHero(firstSelectedHero);
        }
    }

    public void SetMainSelectedHero(HeroSelection hero)
    {
        if (hero != null && !hero.IsSelected)
            return;

        MainSelectedHero = hero;

        if (unitInfoPanel == null)
            return;

        if (hero != null)
        {
            unitInfoPanel.ShowUnit(hero.gameObject);
        }
        else
        {
            unitInfoPanel.HidePanel();
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

        MainSelectedHero = null;
    }
}
