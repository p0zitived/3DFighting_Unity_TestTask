using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Create Inventory Object")]
public class InventoryObject : ScriptableObject
{
    public static event Action OnSlotDeleting;
    public static event Action<ItemObject> OnAddItem;
    public static event Action<ItemObject> OnRemoveItem;

    public int maxSlots;
    public List<InventorySlot> container = new List<InventorySlot>();

    public void AddItem(ItemObject _item,int _count)
    {
        bool hasItem = false;

        foreach (InventorySlot slot in container)
        {
            if (slot.item == _item && slot.item.maxInStack != slot.amount)
            {
                hasItem = true;
                // max in stack condition
                if (slot.amount + _count <= slot.item.maxInStack)
                {
                    slot.AddAmount(_count);
                }
                else
                {
                    slot.AddAmount(slot.item.maxInStack - slot.amount);
                    AddSlot(_item, _count - (slot.item.maxInStack - slot.amount));
                }
                break;
            }
        }

        if (!hasItem)
        {
            AddSlot(_item, _count);
        }

        OnAddItem?.Invoke(_item);
    }
    public void AddWithoutEvent(ItemObject _item, int _count)
    {
        bool hasItem = false;

        foreach (InventorySlot slot in container)
        {
            if (slot.item == _item && slot.item.maxInStack != slot.amount)
            {
                hasItem = true;
                // max in stack condition
                if (slot.amount + _count <= slot.item.maxInStack)
                {
                    slot.AddAmount(_count);
                }
                else
                {
                    slot.AddAmount(slot.item.maxInStack - slot.amount);
                    AddSlot(_item, _count - (slot.item.maxInStack - slot.amount));
                }
                break;
            }
        }

        if (!hasItem)
        {
            AddSlot(_item, _count);
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

        OnRemoveItem?.Invoke(_item);
    }
    public void RemoveItem(int slotidx, int _count)
    {
        OnRemoveItem?.Invoke(container[slotidx].item);

        InventorySlot slot = container[slotidx];

        slot.RemoveAmount(_count);
        if (slot.amount <= 0)
        {
            container.Remove(slot);

            OnSlotDeleting?.Invoke(); // <= event invoke
        }
    }
    public void RemoveItem(int slotidx, int _count,bool withoutEvent)
    {
        if (!withoutEvent)
            OnRemoveItem?.Invoke(container[slotidx].item);

        InventorySlot slot = container[slotidx];

        slot.RemoveAmount(_count);
        if (slot.amount <= 0)
        {
            container.Remove(slot);

            OnSlotDeleting?.Invoke(); // <= event invoke
        }
    }
    private void AddSlot(ItemObject _item,int _count)
    {
        container.Add(new InventorySlot(_item, _count));
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
