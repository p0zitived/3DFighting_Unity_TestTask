using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerShield : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] PlayerStats stats;
    [Header("Settings")]
    [SerializeField] GameObject ShieldObj;
    [SerializeField] float shieldStaminaBurn;
    [SerializeField] float shieldProtection; // 0 min -> 1 max

    private bool shieldActivated = false;

    public static event Action OnShieldActivate;
    public static event Action OnShieldDesactivate;

    private void Start()
    {
        stats.SetShieldProtection(shieldProtection);
        stats.SetShieldStaminaBurning(shieldStaminaBurn);
    }

    private void Update()
    {
        ShieldActivation();
    }

    private void ShieldActivation()
    {
        if (stats.GetStamina() >= shieldStaminaBurn*Time.deltaTime)
        {
            if (Input.GetMouseButtonDown(1))
            {
                shieldActivated = true;

                ShieldObj.transform.DOScale(1, 0.25f);

                OnShieldActivate?.Invoke();
            }

            if (Input.GetMouseButtonUp(1))
            {
                shieldActivated = false;

                ShieldObj.transform.DOScale(0, 0.25f);

                OnShieldDesactivate?.Invoke();
            }
        } else
        {
            ShieldObj.transform.DOScale(0, 0.25f);
            shieldActivated = false;

            OnShieldDesactivate?.Invoke();
        }
    }
}
