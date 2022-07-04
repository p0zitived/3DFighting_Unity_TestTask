using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] EnemyAnimationController anim;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] EnemyController controller;
    [SerializeField] float hitsTime = 0.2f;
    public bool isActivated = false;

    public static event Action<float> OnPlayerDamaged;

    private BoxCollider coll;
    private Vector3 center;
    private Vector3 halfExtends;
    private Quaternion orientation;

    private float remainedTime;
    private bool canDamage = true;

    private void Start()
    {
        coll = GetComponentInChildren<BoxCollider>();
    }

    private void Update()
    {
        isActivated = anim.GetCanAttack();

        if (isActivated)
        {
            center = transform.position;
            halfExtends = coll.size;
            orientation = coll.transform.rotation;

            Collider[] colliders = Physics.OverlapBox(center, halfExtends, orientation, playerLayer);

            if (colliders != null)
            {
                foreach (Collider col in colliders)
                {
                    if (col.GetComponent<PlayerStats>() != null)
                    {
                        ReachedPlayer(col.GetComponent<PlayerStats>(), controller.GetEnemyDamage());
                    }
                }
            }
        }

        TimerCalcs();
    }

    private void TimerCalcs()
    {
        remainedTime -= Time.deltaTime;
        if (remainedTime <= 0)
        {
            canDamage = true;
        }
    }
    private void ReachedPlayer(PlayerStats stats,float damage)
    {
        if (canDamage)
        {
            stats.SetDamage(damage);
            OnPlayerDamaged?.Invoke(damage);
            canDamage = false;
            remainedTime = hitsTime;
        }
    }
}
