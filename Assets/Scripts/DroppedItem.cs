
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [Header("Pickup")]
    [SerializeField] private float pickupDistance = 1.5f;

    private ItemData itemData;
    private bool isPickedUp;

    void Start()
    {
        itemData = GetComponent<ItemData>();

        if (itemData == null)
        {
            Debug.LogWarning(
                "DroppedItem has no ItemData component."
            );
        }
    }

    void Update()
    {
        if (isPickedUp || itemData == null)
            return;

        HeroMovement[] heroes =
            FindObjectsByType<HeroMovement>(
                FindObjectsSortMode.None
            );

        HeroInventory closestInventory = null;
        float closestDistance = float.MaxValue;

        foreach (HeroMovement hero in heroes)
        {
            HeroInventory inventory =
                hero.GetComponent<HeroInventory>();

            if (inventory == null)
                continue;

            if (inventory.CurrentItems >= inventory.MaxItems)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                hero.transform.position
            );

            if (distance <= pickupDistance &&
                distance < closestDistance)
            {
                closestDistance = distance;
                closestInventory = inventory;
            }
        }

        if (closestInventory != null)
        {
            TryPickUp(closestInventory);
        }
    }

    private void TryPickUp(HeroInventory inventory)
    {
        if (isPickedUp)
            return;

        bool itemAdded = inventory.TryAddItem(itemData);

        if (!itemAdded)
            return;

        isPickedUp = true;

        Debug.Log(
            inventory.gameObject.name +
            " picked up " +
            itemData.ItemName +
            "."
        );

        Destroy(gameObject);
    }
}
