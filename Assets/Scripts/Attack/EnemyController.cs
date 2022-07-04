using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    #region Fields
    [SerializeField] Enemy enemy;

    [Header("Player detection")]
    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] float detectionRange;
    [SerializeField] LayerMask playerLayer;
    public GameObject player;

    [Header("UI")]
    [SerializeField] Canvas canvas;
    [SerializeField] Image hpimg;
    #endregion

    #region Events
    public static event Action<float,EnemyController> OnHitedEnemy;
    public static event Action OnEnemyKill;
    public event Action OnDeath;
    #endregion

    private float remainedTime;
    private bool canBeDamaged = true;

    private float hp;
    private float damage;

    private void Start()
    {
        hp = enemy.hp;
        damage = enemy.damage;
    }

    private void Update()
    {
        PlayerDetect();
        SetUI();

        IgnoreTimeCalc();
    }

    public void Kill()
    {
        OnDeath?.Invoke();
        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<EnemyController>());
        Destroy(GetComponentInChildren<Canvas>().transform.gameObject);
        Destroy(transform.gameObject, 10f);

        OnEnemyKill?.Invoke();
    }
    public void Damage(float value,float ignoreDamageTime)
    {
        if (canBeDamaged)
        {
            hp -= value;
            canBeDamaged = false;
            remainedTime = ignoreDamageTime;

            OnHitedEnemy?.Invoke(value,this);

            if (hp <= 0)
            {
                Kill();
            }
        }
    }
    public float GetEnemyDamage()
    {
        return damage;
    }

    private void RotateInPlayerDirection()
    {
        if (player != null)
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }
    private void PlayerDetect()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
        if (colliders.Length > 0)
        {
            player = colliders[0].gameObject;
            navAgent.SetDestination(player.transform.position);
        }

        if (Vector3.Distance(transform.position,navAgent.destination) <= navAgent.stoppingDistance)
        {
            RotateInPlayerDirection();
        }
    }
    private void IgnoreTimeCalc()
    {
        remainedTime -= Time.deltaTime;
        if (remainedTime<=0)
            canBeDamaged = true;
    }
    private void SetUI()
    {
        canvas.transform.LookAt(Camera.main.transform);
        hpimg.fillAmount = hp / enemy.hp;
    }
}
