using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private int slotIndex = 0;
    [SerializeField] private HeroInventory heroInventory;

    [Header("Colors")]
    [SerializeField] private Color normalColor =
        new Color(0.145f, 0.165f, 0.2f, 1f);

    [SerializeField] private Color equippedColor =
        new Color(1f, 0.83f, 0.23f, 1f);

    private Button button;
    private Image slotImage;

    void Awake()
    {
        button = GetComponent<Button>();
        slotImage = GetComponent<Image>();

        if (button != null)
        {
            button.onClick.AddListener(HandleClick);
        }
    }

    void Update()
    {
        UpdateVisual();
    }

    private void HandleClick()
    {
        if (heroInventory == null)
        {
            Debug.LogWarning("InventorySlotUI has no HeroInventory assigned.");
            return;
        }

        heroInventory.ToggleEquipItem(slotIndex);
    }

    private void UpdateVisual()
    {
        if (slotImage == null || heroInventory == null)
            return;

        if (heroInventory.IsItemEquipped(slotIndex))
        {
            slotImage.color = equippedColor;
        }
        else
        {
            slotImage.color = normalColor;
        }
    }
}