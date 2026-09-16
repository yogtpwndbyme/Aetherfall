using UnityEngine;
using UnityEngine.UI;

public class BootsSlotUI : MonoBehaviour
{
    [SerializeField] private HeroInventory heroInventory;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(HandleClick);
        }
    }

    private void HandleClick()
    {
        if (heroInventory == null)
        {
            Debug.LogWarning("BootsSlotUI has no HeroInventory assigned.");
            return;
        }

        heroInventory.UnequipBoots();
    }
}