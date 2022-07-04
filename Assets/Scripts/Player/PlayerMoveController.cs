using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField] CharacterController characterController;
    [SerializeField] float speed;
    [SerializeField] float smoothSpeed;
    [SerializeField] PlayerCamera camCotroller;
    [SerializeField] PlayerStats stats;
    [SerializeField] float minStaminaForSprint;

    [Header("Gravity")]
    [SerializeField] float gravity = -1;
    [SerializeField] Transform groundCheck;
    [SerializeField] float checkDistance;
    [SerializeField] LayerMask mask;
    private bool isGrounded;

    [Header("Rotation")]
    [SerializeField] float rotationSpeed;

    public static event Action OnStartSprinting;
    public static event Action OnEndSprinting;
    public static event Action OnSprintCoolDown;
    public static event Action OnSprintCoolDownEnd;

    private Vector3 move;
    private Vector3 direction;
    private Vector3 relative_direction;
    private Vector3 smooth_direction;
    private Vector3 current_direction = Vector3.zero; // used to smooth movements via SmoothDamp
    private Vector3 smoothInputVelocity;
    private Vector3 smoothInputVelocity2 = Vector3.zero;

    private bool sprinting = false;
    private bool SprintCoolDown = false;
    private float aux_rotationSpeed;

    private bool canMove = true;

    private void Start()
    {
        PlayerAttack.OnPlayerStartAttack += OnPlayerStartAttack;
        PlayerAttack.OnPlayerEndAttack += OnPlayerEndAttack;

        aux_rotationSpeed = rotationSpeed;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (canMove)
        {
            SetDirection();
            RotateToForward();
            SetMove();
            CheckIfSprint();
            current_direction = Vector3.SmoothDamp(current_direction, relative_direction, ref smoothInputVelocity, smoothSpeed);
            characterController.Move(move);

            smooth_direction = Vector3.SmoothDamp(smooth_direction, direction, ref smoothInputVelocity2, smoothSpeed);
        }
        else
        {
            OnEndSprinting?.Invoke();
            sprinting = false;
            rotationSpeed = aux_rotationSpeed;

            current_direction = Vector3.zero;
            smooth_direction = Vector3.zero;
            GravityCalc();
        }
    }

    private void SetMove()
    {
        move.x = current_direction.x * speed * Time.deltaTime;
        move.z = current_direction.z * speed * Time.deltaTime;
        GravityCalc();
    }
    private void SetDirection()
    {
        if (Input.GetKey(KeyCode.W))
        {
            direction.z = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction.z = -1;
        }
        else
        {
            direction.z = 0;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            direction.z = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1;
        }
        else
        {
            direction.x = 0;
        }
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            direction.x = 0;
        }

        direction.Normalize();
        relative_direction = camCotroller.cameraTarget.forward * direction.z + camCotroller.cameraTarget.right * direction.x;
    }
    private void RotateInDirection(Vector3 dir)
    {
        Vector3 aux = new Vector3(dir.normalized.x, 0, dir.normalized.z);
        if (aux != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(aux, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
    private void RotateToForward()
    {
        if (direction != Vector3.zero)
        {
            Vector3 aux = new Vector3(camCotroller.cameraTarget.transform.forward.x, 0, camCotroller.cameraTarget.transform.forward.z);
            if (aux != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(aux, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
    private void GravityCalc()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, checkDistance, mask);

        if (isGrounded)
        {
            move.y = 0;
        } else
        {
            move.y += gravity * Time.deltaTime;
        }
    }
    private void CheckIfSprint()
    {
        if (SprintCoolDown)
        {
            if (stats.GetStamina() >= minStaminaForSprint)
            {
                SprintCoolDown = false;
                OnSprintCoolDownEnd?.Invoke();
            }
        }

        if (stats.GetStamina() != 0 && !SprintCoolDown)
        {
            if (direction != Vector3.zero)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (!sprinting)
                    {
                        rotationSpeed *= 2;
                        sprinting = true;
                    }
                    direction *= 2;
                    relative_direction *= 2;
                    OnStartSprinting?.Invoke();
                }

                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    rotationSpeed /= 2;
                    sprinting = false;
                    OnEndSprinting?.Invoke();
                }
            }
            else
            {
                if (sprinting)
                {
                    sprinting = false;
                    rotationSpeed /= 2;
                    OnEndSprinting?.Invoke();
                }
            }
        } else
        {
            SprintCoolDown = true;
            OnSprintCoolDown?.Invoke();
            OnEndSprinting?.Invoke();
        }
    }

    public Vector3 GetMove()
    {
        return move;
    }
    public Vector3 GetSmoothDirection()
    {
        return smooth_direction;
    }
    // move condition
    private void OnPlayerStartAttack(WeaponItem weapon)
    {
        canMove = false;
    }
    private void OnPlayerEndAttack()
    {
        canMove = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, checkDistance);
    }
}
