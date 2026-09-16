using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;

    void Start()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        if (inventoryPanel == null)
            return;

        bool isOpen = inventoryPanel.activeSelf;

        inventoryPanel.SetActive(!isOpen);
    }
}