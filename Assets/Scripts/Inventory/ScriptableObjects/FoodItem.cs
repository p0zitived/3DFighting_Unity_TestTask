using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Inventory/Create new Food")]
public class FoodItem : ItemObject
{
    public float restoreHP;
    public float restoreStamina;

    private void Awake()
    {
        type = ItemType.food;
    }
}
