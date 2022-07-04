using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform cameraTarget;
    [SerializeField] Cinemachine.CinemachineFreeLook cm;

    private void Start()
    {
        
    }

    private void Update()
    {
        cameraTarget.rotation = Quaternion.Euler(new Vector3(0, cm.m_XAxis.Value));
    }
}
