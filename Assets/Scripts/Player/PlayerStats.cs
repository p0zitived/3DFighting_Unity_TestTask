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
    [SerializeField] Inventory_Controller inv;
    [SerializeField] Transform itemSpawner;

    [Header("Regeneration")]
    [SerializeField] float hp_regeneration;
    [SerializeField] float stamina_regeneration;
    [SerializeField] float sprintCost;
    private bool sprinting = false;
    private bool staminaRegen = false;
    private bool hpRegen = false;

    private float acctualHp;
    private float acctualStamina;
    private float acctualShieldState;

    // shield
    private bool shieldActivated = false;
    private float shieldProtection = 0.5f;
    private float shieldStaminaBurning;

    public WeaponItem weapon;

    private void Start()
    {
        Inventory_Controller.OnUseSlot += OnUseSlot;

        acctualHp = maxHp;
        acctualStamina = maxStamina;

        PlayerMoveController.OnStartSprinting += OnSprintStart;
        PlayerMoveController.OnEndSprinting += OnSprintEnd;
        PlayerShield.OnShieldActivate += OnShieldOn;
        PlayerShield.OnShieldDesactivate += OnShieldOff;
    }

    private void OnDestroy()
    {
        Inventory_Controller.OnUseSlot -= OnUseSlot;

        PlayerMoveController.OnStartSprinting -= OnSprintStart;
        PlayerMoveController.OnEndSprinting -= OnSprintEnd;
        PlayerShield.OnShieldActivate -= OnShieldOn;
        PlayerShield.OnShieldDesactivate -= OnShieldOff;
    }

    private void Update()
    {
        if (sprinting)
        {
            staminaRegen = false;
            BurnStamina(sprintCost * Time.deltaTime);
        }
        if (shieldActivated)
        {
            staminaRegen = false;
            BurnStamina(shieldStaminaBurning * Time.deltaTime);
        }
        if (!sprinting && !shieldActivated)
        {
            staminaRegen = true;
        }

        if (staminaRegen)
        {
            HealStamina(stamina_regeneration * Time.deltaTime);
        }
    }

    public void Heal(float value)
    {
        if (value >= 0)
        {
            Debug.Log(acctualHp);
            acctualHp += value;
            if (acctualHp > maxHp)
                acctualHp = maxHp;
        }
    }
    public void SetDamage(float value)
    {
        if (!shieldActivated)
        {
            if (value >= 0)
            {
                acctualHp -= value;
                if (acctualHp <= 0)
                {
                    acctualHp = 0;
                    OnPlayerDeath?.Invoke();
                }
            }
        }
        else
        {
            if (value >= 0)
            {
                acctualHp -= value*(1-shieldProtection);
                if (acctualHp <= 0)
                {
                    acctualHp = 0;
                    OnPlayerDeath?.Invoke();
                }
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
            acctualStamina += value;
            if (acctualStamina > maxStamina)
            {
                acctualStamina = maxStamina;
            }
        }
    }
    public WeaponItem GetPlayerWeapon()
    {
        return weapon;
    }

    public float GetHealth()
    {
        return acctualHp;
    }
    public float GetStamina()
    {
        return acctualStamina;
    }
    public float GetMaxHP()
    {
        return maxHp;
    }
    public float GetMaxStamina()
    {
        return maxStamina;
    }

    // shield
    public void SetShieldProtection(float value)
    {
        if (value >= 0 && value <= 1)
            shieldProtection = value;
    }
    public void ActivateShield(bool input)
    {
        shieldActivated = input;
    }
    public void SetShieldStaminaBurning(float value)
    {
        if (value >= 0)
            shieldStaminaBurning = value;
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
                RestoreUsedEquipmentInInventory(weapon);
                weapon = slot.item as WeaponItem;
            }
        }
    }
    private void RestoreUsedEquipmentInInventory(ItemObject item)
    {
        if (item.type == ItemType.weapon)
        {
            inv.RestoreToInventory(weapon);
        }
    }

    // event handlers
    private void OnSprintStart()
    {
        sprinting = true;
    }
    private void OnSprintEnd()
    {
        sprinting = false;
    }
    private void OnShieldOn()
    {
        shieldActivated = true;
    }
    private void OnShieldOff()
    {
        shieldActivated = false;
    }
}
