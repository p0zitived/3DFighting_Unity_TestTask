using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon",menuName = "Inventory/Create new Weapon")]
public class WeaponItem : ItemObject
{
    public float damage;
    public float stamina;

    private void Awake()
    {
        type = ItemType.weapon;
    }
}
