using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Create Inventory Object")]
public class InventoryObject : ScriptableObject
{
    public static event Action OnSlotDeleting;

    public List<InventorySlot> container = new List<InventorySlot>();

    public void AddItem(ItemObject _item,int _count)
    {
        bool hasItem = false;

        foreach (InventorySlot slot in container)
        {
            if (slot.item == _item)
            {
                hasItem = true;
                slot.AddAmount(_count);
                break;
            }
        }

        if (!hasItem)
        {
            container.Add(new InventorySlot(_item, _count));
        }
    }
    public void RemoveItem(ItemObject _item,int _count)
    {
        foreach (InventorySlot slot in container)
        {
            if (slot.item == _item)
            {
                slot.RemoveAmount(_count);
                if (slot.amount <= 0)
                {
                    container.Remove(slot);

                    OnSlotDeleting?.Invoke(); // <= event invoke
                }
                break;
            }
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;

    public InventorySlot(ItemObject _item,int _count)
    {
        item = _item;
        amount = _count;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
    public void RemoveAmount(int value)
    {
        amount -= value;
    }
}
