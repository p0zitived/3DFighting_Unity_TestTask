using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAnimationController : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] float stunDuration;
    [SerializeField] EnemyController controller;
    [SerializeField] EnemyWeapon weapon;
    [Header("UI")]
    [SerializeField] Canvas canvas;
    [SerializeField] Animator animator;
    [SerializeField] GameObject stunBar;
    [SerializeField] GameObject damageNumbers;
    [Header("Animator")]
    [SerializeField] NavMeshAgent nav;

    private Vector3 oldPos;
    private Vector3 direction;
    private float speed;

    private bool stuned = false;
    private float stunRemained;

    private bool canAttack;
    private bool canWalk;
    private float setedSpeed;
    private bool isDead = false;


    private void Start()
    {
        setedSpeed = nav.speed;
        oldPos = transform.position;

        EnemyController.OnHitedEnemy += OnEnemyHit;
        controller.OnDeath += DeathAnimation;
    }

    private void OnDestroy()
    {
        EnemyController.OnHitedEnemy -= OnEnemyHit;
        controller.OnDeath -= DeathAnimation;
    }

    private void Update()
    {
        if (!isDead)
        {
            MathCalcs();
            SetMovingAnimation();

            CalcTimers();
            CanAttack();

            if (canAttack || !canWalk)
            {
                nav.speed = 0;
            }
            else
            {
                nav.speed = setedSpeed;
            }
        }
    }

    private void CalcTimers()
    {
        stunBar.transform.GetChild(0).GetComponent<Image>().fillAmount = stunRemained / stunDuration;
        stunRemained -= Time.deltaTime;
        if (stunRemained <= 0)
        {
            canWalk = true;
            stuned = false;

            stunBar.SetActive(false);
        }
    }
    private void OnEnemyHit(float damage, EnemyController controller)
    {
        if (controller == this.controller)
        {
            animator.SetTrigger("Hited");
            Stun(stunDuration);

            SpawnDamageNumbers(damage);
        }
    }
    private void MathCalcs()
    {
        direction = (transform.position - oldPos).normalized;
        speed = Vector3.Distance(transform.position, oldPos) / Time.deltaTime;

        oldPos = transform.position;
    }
    private void SetMovingAnimation()
    {
        animator.SetFloat("Speed", speed);
    }
    private void CanAttack()
    {
        if (controller.player != null)
        {
            if (!stuned)
            {
                if (Vector3.Distance(nav.destination, transform.position) <= nav.stoppingDistance)
                {
                    canAttack = true;
                    animator.SetBool("CanAttack", true);

                    weapon.isActivated = true;
                }
                else
                {
                    canAttack = false;
                    animator.SetBool("CanAttack", false);

                    weapon.isActivated = false;
                }
            }
            else
            {
                canAttack = false;
                animator.SetBool("CanAttack", false);

                weapon.isActivated = false;
            }
        }
    }
    private void Stun(float time)
    {
        canWalk = false;
        stuned = true;
        stunRemained = time;

        stunBar.SetActive(true);
    }
    private void DeathAnimation()
    {
        animator.SetBool("Dead", true);
        isDead = true;
    }
    private void SpawnDamageNumbers(float damage)
    {
        GameObject obj = Instantiate(damageNumbers);
        obj.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        obj.GetComponent<DamageNumbers>().damage = damage;
    }

    public bool GetCanAttack()
    {
        return canAttack;
    }
}
