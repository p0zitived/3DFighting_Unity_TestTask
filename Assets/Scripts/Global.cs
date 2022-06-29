using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    // Using in : PlayerMoveController
    public static Vector3 PlayerDir { get; set; }
    public static bool CanMove { get; set; }

    [Header("Fixed fps :")]
    [SerializeField] bool enable;
    [SerializeField] int target_fps;

    private void Start()
    {
        CanMove = true;

        SetFixedFps(enable, target_fps);
    }

    private void SetFixedFps(bool enable,int targetFps)
    {
        if (enable)
            Application.targetFrameRate = targetFps;
    }
}
