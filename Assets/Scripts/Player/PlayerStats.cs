using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static event Action<float> OnHealPlayer;
    public static event Action<float> OnDamagePlayer;
    public static event Action OnPlayerDeath;

    [SerializeField] float maxHp;
    [SerializeField] float maxStamina;
    [SerializeField] Transform itemSpawner;

    private float acctualHp;
    private float acctualStamina;
    private float acctualShieldState;

    private WeaponItem weapon;
    private ShieldItem shield;

    private void Start()
    {
        Inventory_Controller.OnUseSlot += OnUseSlot;

        acctualHp = maxHp;
        acctualStamina = maxStamina;
        if (shield != null)
            acctualShieldState = shield.damageAbsorption;
    }

    public void Heal(float value)
    {
        if (value >= 0)
        {
            acctualHp += value;
            if (acctualHp > maxHp)
                acctualHp = maxHp;
        }
    }
    public void SetDamage(float value)
    {
        if (value >= 0)
        {
            acctualHp -= value;
            if (acctualHp < 0)
            {
                OnPlayerDeath?.Invoke();
            }
        }
    }
    public void BurnStamina(float value)
    {
        if (value >= 0)
        {
            acctualStamina -= value;
            if (acctualStamina < 0)
            {
                acctualStamina = 0;
            }
        }
    }
    public void HealStamina(float value)
    {
        if (value >= 0)
        {
            acctualStamina -= value;
            if (acctualStamina < 0)
            {
                acctualStamina = 0;
            }
        }
    }
    public void SetShieldDamage(float value)
    {
        if (shield != null)
        {
            acctualShieldState -= value;
            if (acctualShieldState < 0)
            {
                acctualShieldState = 0;
            }
        }
    }

    private void OnUseSlot(InventorySlot slot)
    {
        if (slot.item.type == ItemType.food)
        {
            FoodItem food = slot.item as FoodItem;
            Heal(food.restoreHP);
            HealStamina(food.restoreStamina);
        }
        if (slot.item.type == ItemType.weapon)
        {
            if (weapon == null)
            {
                weapon = slot.item as WeaponItem;
            } else
            {
                DropItem(weapon);
                weapon = slot.item as WeaponItem;
            }
        }
        if (slot.item.type == ItemType.shield)
        {
            shield = slot.item as ShieldItem;
            acctualShieldState = shield.damageAbsorption;
        }
    }
    private void DropItem(ItemObject item)
    {
        GameObject obj = Instantiate(item.prefab);
        obj.transform.position = itemSpawner.position;
    }
}
