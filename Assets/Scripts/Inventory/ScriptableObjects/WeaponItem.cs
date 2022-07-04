using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    sword,
    axe,
    peak
}

[CreateAssetMenu(fileName = "New Weapon",menuName = "Inventory/Create new Weapon")]
public class WeaponItem : ItemObject
{
    public float damage;
    public float stamina;
    public float ignoreDamageTime;
    public WeaponType weaponType;

    private void Awake()
    {
        type = ItemType.weapon;
    }
}
