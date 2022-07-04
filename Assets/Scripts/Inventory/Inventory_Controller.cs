using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Controller : MonoBehaviour
{
    #region Events
    public static event Action<ItemObject> OnSelectingInventorySlot;
    public static event Action<ItemObject> OnPickUpWhenFullInventory;
    public static event Action<InventorySlot> OnUseSlot;
    #endregion

    #region Fields
    [Header("Inventory object")]
    [SerializeField] InventoryObject inventory;

    [Header("Inventory UI parts")]
    [SerializeField] Transform container;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] GameObject pickUpInterface;

    [Header("Gizmos")]
    [SerializeField] Vector3 detecting_box_sizes;

    private int selectedSlotIdx = -1;
    #endregion

    private void Start()
    {
        InventoryObject.OnSlotDeleting += OnSlotDeleting;
    }

    public List<Item> CheckForItems()
    {
        List<Item> output = new List<Item>();

        Collider[] colliders = Physics.OverlapBox(transform.position, detecting_box_sizes);
        if (colliders != null)
        {
            foreach (Collider col in colliders)
            {
                if (col.GetComponentInParent<Item>())
                {
                    output.Add(col.GetComponentInParent<Item>());
                }
            }
        }

        return output;
    }
    public void PickUpItem(Item item)
    {
        if (!IsFull(item.item))
        {
            if (IsEmpty())
            {
                selectedSlotIdx = -1;
                inventory.AddItem(item.item, 1);
                SelectSlot(0);
                Destroy(item.gameObject);
            }
            else
            {
                inventory.AddItem(item.item, 1);
                Destroy(item.gameObject);
            }
            RefreshInventoryUI();
        } else
        {
            OnPickUpWhenFullInventory?.Invoke(item.item);
        }
    }
    public void RestoreToInventory(ItemObject item)
    {
        if (IsEmpty())
        {
            selectedSlotIdx = -1;
            inventory.AddWithoutEvent(item, 1);
            SelectSlot(0);
        }
        else
        {
            inventory.AddWithoutEvent(item, 1);
        }
        RefreshInventoryUI();
    }
    public void RefreshInventoryUI()
    {
        // clear UI
        foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }

        // Add slots
        for (int i = 0;i<inventory.container.Count;i++)
        {
            InventorySlot slot = inventory.container[i];

            GameObject obj = Instantiate(slotPrefab, container);
            
            // set name
            obj.transform.Find("SlotName").GetComponent<Text>().text = slot.item.itemName;
            // set icon
            obj.transform.Find("SlotIcon").GetComponent<Image>().sprite = slot.item.icon;
            // set count
            obj.transform.Find("SlotCount").GetComponent<Text>().text = slot.amount + "";

            // set frame
            if (i == selectedSlotIdx)
            {
                obj.transform.Find("SlotFrame").gameObject.SetActive(true);
            }
        }
    }

    public void SelectSlot(int idx)
    {
        if (inventory.container.Count != 0)
        {
            // maybe not the best place for this
            if (idx == selectedSlotIdx)
            {
                OnUseSlot?.Invoke(GetSelectedSlot());
                UseItem(selectedSlotIdx);
            }

            if (idx < inventory.container.Count)
            {
                selectedSlotIdx = idx;
                OnSelectingInventorySlot?.Invoke(GetSelectedSlot().item);
            }
            else
            {
                selectedSlotIdx = inventory.container.Count - 1;
                OnSelectingInventorySlot?.Invoke(GetSelectedSlot().item);
            }
        }
    }
    public void DropItem(ItemObject item)
    {
        if (!IsEmpty())
        {
            inventory.RemoveItem(item, 1);
        }
    }
    public void DropItem(int slotidx)
    {
        if (!IsEmpty())
        {
            inventory.RemoveItem(slotidx, 1);
        }
    }
    public void UseItem(int slotidx)
    {
        if (!IsEmpty())
        {
            inventory.RemoveItem(slotidx, 1,true);
        }
    }
    public InventorySlot GetSelectedSlot()
    {
        return inventory.container[selectedSlotIdx];
    }
    public int GetSelectedSlotIdx()
    {
        return selectedSlotIdx;
    }

    public bool IsEmpty()
    {
        if (inventory.container.Count == 0)
        {
            return true;
        }
        else
            return false;
    }
    public bool IsFull(ItemObject item)
    {
        bool result = true;

        if (inventory.container.Count < inventory.maxSlots)
        {
            result = false;
        }

        foreach (InventorySlot slot in inventory.container)
        {
            if (slot.item == item)
            {
                if (slot.amount < slot.item.maxInStack)
                {
                    result = false;
                }
            }
        }

        return result;
    }
    public void ShowPickUp(bool enable)
    {
        pickUpInterface.SetActive(enable);
    }
    public void ShowPickUp(bool enable,ItemObject item)
    {
        pickUpInterface.SetActive(enable);
        pickUpInterface.GetComponent<Text>().text = "Press [E] to pick up : " + item.name;
    }

    private void OnApplicationQuit()
    {
        inventory.container.Clear();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, detecting_box_sizes * 2);
    }

    // event handlers :
    private void OnSlotDeleting()
    {
        if (selectedSlotIdx >= inventory.container.Count)
        {
            SelectSlot(inventory.container.Count - 1);
        }
    }
}
