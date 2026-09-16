using UnityEngine;
using UnityEngine.UI;

public class HelmetSlotUI : MonoBehaviour
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
            Debug.LogWarning("HelmetSlotUI has no HeroInventory assigned.");
            return;
        }

        heroInventory.UnequipHelmet();
    }
}