using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] float speed;

    private Vector3 dir;

    private void Update()
    {
        if (CanMove())
        {
            dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            characterController.Move(dir * speed * Time.deltaTime);
        }
    }

    // move condition
    private bool CanMove()
    {
        if (Global.CanMove)
        {
            return true;
        }
        else
            return false;
    }
}
