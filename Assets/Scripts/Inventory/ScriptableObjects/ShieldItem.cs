using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield", menuName = "Inventory/Create new Shield")]
public class ShieldItem : ItemObject
{
    public float damageAbsorption;

    private void Awake()
    {
        type = ItemType.shield;
    }
}
