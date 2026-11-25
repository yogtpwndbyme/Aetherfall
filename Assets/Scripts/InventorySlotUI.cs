
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [Header("Inventory Slot")]
    [SerializeField] private int slotIndex = 0;

    [Header("Colors")]
    [SerializeField] private Color normalColor =
        new Color(0.145f, 0.165f, 0.2f, 1f);

    [SerializeField] private Color equippedColor =
        new Color(1f, 0.83f, 0.23f, 1f);

    private Button button;
    private Image slotImage;

    private HeroInventory CurrentInventory
    {
        get
        {
            if (UnitSelectionManager.Instance == null)
                return null;

            return UnitSelectionManager.Instance
                .MainSelectedInventory;
        }
    }

    void Awake()
    {
        button = GetComponent<Button>();
        slotImage = GetComponent<Image>();

        if (button != null)
        {
            button.onClick.AddListener(HandleClick);
        }
    }

    void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(HandleClick);
        }
    }

    void Update()
    {
        UpdateVisual();
    }

    private void HandleClick()
    {
        HeroInventory inventory = CurrentInventory;

        if (inventory == null)
            return;

        inventory.ToggleEquipItem(slotIndex);
    }

    private void UpdateVisual()
    {
        HeroInventory inventory = CurrentInventory;

        bool hasItem =
            inventory != null &&
            slotIndex >= 0 &&
            slotIndex < inventory.CurrentItems;

        if (button != null)
        {
            button.interactable = hasItem;
        }

        if (slotImage == null)
            return;

        bool isEquipped =
            hasItem &&
            inventory.IsItemEquipped(slotIndex);

        slotImage.color = isEquipped
            ? equippedColor
            : normalColor;
    }
}
