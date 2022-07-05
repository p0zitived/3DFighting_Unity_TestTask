using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Transform WeaponPoint;
    [SerializeField] PlayerStats stats;
    [SerializeField] Animator animator;

    public static event Action<WeaponItem> OnPlayerStartAttack;
    public static event Action OnPlayerEndAttack;

    private bool changedBaseState = false;
    private int last_attackCount;
    private GameObject weaponInHand;
    private int last_weaponType = 0;

    private void Start()
    {
        Inventory_Controller.OnUseSlot += OnUseSlot;

        if (stats.weapon != null)
        {
            SpawnWeaponInHand(stats.weapon);
        }
    }

    private void OnDestroy()
    {
        Inventory_Controller.OnUseSlot -= OnUseSlot;
    }

    private void Update()
    {
        Animation();

        StaminaCalcCost();
    }

    private void OnUseSlot(InventorySlot slot)
    {
        if (slot.item as WeaponItem != null)
        {
            if (stats.GetPlayerWeapon() == null)
            {
                SpawnWeaponInHand(slot.item as WeaponItem);
            } else
            {
                Destroy(WeaponPoint.GetChild(0).gameObject);
                SpawnWeaponInHand(slot.item as WeaponItem);
            }
        }
    }
    private void SpawnWeaponInHand(WeaponItem weapon)
    {
        weaponInHand = Instantiate(weapon.prefab,WeaponPoint);
        Destroy(weaponInHand.GetComponentInChildren<Rigidbody>());
        Destroy(weaponInHand.GetComponent<Item>());
        weaponInHand.transform.position = WeaponPoint.position;
        weaponInHand.transform.rotation = WeaponPoint.rotation;
    }
    private void Animation()
    {
        string aux_anim_name = "";
        if (stats.weapon != null)
        {
            if (stats.weapon.weaponType == WeaponType.sword)
            {
                animator.SetInteger("WeaponType", 1);
                aux_anim_name = "Sword";
            }
            if (stats.weapon.weaponType == WeaponType.axe)
            {
                animator.SetInteger("WeaponType", 2);
                aux_anim_name = "Axe";
            }
            if (stats.weapon.weaponType == WeaponType.peak)
            {
                animator.SetInteger("WeaponType", 3);
                aux_anim_name = "Spear";
            }
            // bug fix (changing weapon while animating)
            if (last_weaponType != animator.GetInteger("WeaponType"))
            {
                animator.SetInteger("AttackCount", 0);
                last_weaponType = animator.GetInteger("WeaponType");
            }
            else
            {

                // =================== Input ==============
                if (Input.GetMouseButtonDown(0) && stats.GetStamina() >= stats.weapon.stamina)
                {
                    animator.SetInteger("AttackCount", animator.GetInteger("AttackCount") + 1);
                    changedBaseState = false;
                    OnPlayerStartAttack?.Invoke(stats.weapon);
                }

                if (animator.GetCurrentAnimatorStateInfo(2).IsName(aux_anim_name))
                {
                    if (changedBaseState)
                    {
                        animator.SetInteger("AttackCount", 0);
                        OnPlayerEndAttack?.Invoke();
                    }
                }
                else
                {
                    changedBaseState = true;
                }
            }
        }
    }
    private void StaminaCalcCost()
    {
        int count = animator.GetInteger("AttackCount");

        if (count == 0)
        {
            last_attackCount = 0;
        }

        if (last_attackCount < count && last_attackCount <= 3)
        {
            if (stats.GetStamina() >= stats.weapon.stamina)
            {
                stats.BurnStamina(stats.weapon.stamina);
            }
            last_attackCount = count;
        }
    }
}
