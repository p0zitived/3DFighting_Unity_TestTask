using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public bool Enabled = false;
    public LayerMask layer;

    private BoxCollider coll;

    private Vector3 center;
    private Vector3 halfExtends;
    private Quaternion orientation;

    private void Start()
    {
        coll = GetComponentInChildren<BoxCollider>();

        PlayerAttack.OnPlayerStartAttack += OnPlayerStartAttacking;
        PlayerAttack.OnPlayerEndAttack += OnPlayerStopAttacking;
    }

    private void OnDestroy()
    {
        PlayerAttack.OnPlayerStartAttack -= OnPlayerStartAttacking;
        PlayerAttack.OnPlayerEndAttack -= OnPlayerStopAttacking;
    }

    private void Update()
    {
        if (Enabled)
        {
            center = transform.position;
            halfExtends = coll.size;
            orientation = coll.transform.rotation;

            Collider[] colliders = Physics.OverlapBox(center, halfExtends, orientation, layer);

            if (colliders != null)
            {
                foreach (Collider col in colliders)
                {
                    if (col.GetComponent<EnemyController>() != null)
                    {
                        ReachedEnemy(col.GetComponent<EnemyController>());
                    }
                }
            }
        }
    }

    private void ReachedEnemy(EnemyController ec)
    {
        try
        {
            WeaponItem weapon = GetComponentInParent<PlayerStats>().weapon;
            ec.Damage(weapon.damage, weapon.ignoreDamageTime);
        } catch
        { }
    }

    private void OnPlayerStartAttacking(WeaponItem weapon)
    {
        Enabled = true;
    }
    private void OnPlayerStopAttacking()
    {
        Enabled = false;
    }
}
