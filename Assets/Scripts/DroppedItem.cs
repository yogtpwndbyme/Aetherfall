using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [Header("Pickup")]
    [SerializeField] private float pickupDistance = 1.5f;

    private Transform hero;
    private HeroInventory heroInventory;
    private ItemData itemData;

    void Start()
    {
        HeroMovement heroMovement = FindFirstObjectByType<HeroMovement>();

        if (heroMovement != null)
        {
            hero = heroMovement.transform;
            heroInventory = heroMovement.GetComponent<HeroInventory>();
        }

        itemData = GetComponent<ItemData>();

        if (itemData == null)
        {
            Debug.LogWarning("DroppedItem has no ItemData component.");
        }
    }

    void Update()
    {
        if (hero == null)
            return;

        float distanceToHero = Vector3.Distance(
            transform.position,
            hero.position
        );

        if (distanceToHero <= pickupDistance)
        {
            TryPickUp();
        }
    }

    private void TryPickUp()
    {
        if (heroInventory == null)
        {
            Debug.LogWarning("Hero has no HeroInventory component.");
            return;
        }

        if (itemData == null)
        {
            Debug.LogWarning("This dropped item has no ItemData.");
            return;
        }

        bool itemAdded = heroInventory.TryAddItem(itemData);

        if (!itemAdded)
            return;

        Debug.Log(
            "Hero picked up " +
            itemData.ItemName +
            "."
        );

        Destroy(gameObject);
    }
}