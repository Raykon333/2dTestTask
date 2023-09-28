using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [SerializeField] Inventory Inventory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickupItem pickup;
        if (collision.TryGetComponent(out pickup))
        {
            var success = Inventory.TryAddItem(new InventoryItem { Name = pickup.Name }, pickup.Amount);
            if (success)
                Destroy(pickup.gameObject);
        }
    }
}
