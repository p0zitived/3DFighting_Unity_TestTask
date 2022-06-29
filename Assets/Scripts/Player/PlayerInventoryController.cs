using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField] Inventory_Controller inv;

    private List<Item> items_arround;

    private void Update()
    {
        items_arround = inv.CheckForItems();
        
        if (items_arround.Count > 0)
        {
            inv.ShowPickUp(true, items_arround[0].item);

            if (Input.GetKeyDown(KeyCode.E))
            {
                inv.PickUpItem(items_arround[0]);
                inv.ShowPickUp(true, items_arround[0].item);
            }
        } else
        {
            inv.ShowPickUp(false);
        }

        SlotSelection(1, 2, 3, 4, 5);
        DropingItem();
    }

    private void SlotSelection(params int[] keys)
    {
        foreach (int key in keys)
        {
            if (Input.GetKeyDown(key + ""))
            {
                inv.SelectSlot(key - 1);
                inv.RefreshInventoryUI();
            }
        }
    }
    private void DropingItem()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!inv.IsEmpty())
            {
                inv.DropItem(inv.GetSelectedSlot().item);
            }
            inv.RefreshInventoryUI();
        }
    }
}
