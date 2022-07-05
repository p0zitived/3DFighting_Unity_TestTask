using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Global : MonoBehaviour
{
    // Using in : PlayerMoveController
    public static Vector3 PlayerDir { get; set; }
    public static bool CanMove { get; set; }
    public static int enemyKilled = 0;

    [Header("UI")]
    [SerializeField] Image blackDisplay;

    [Header("Fixed fps :")]
    [SerializeField] bool enable;
    [SerializeField] int target_fps;

    private void Start()
    {
        blackDisplay.rectTransform.DOLocalMoveY(1000, 3).OnComplete(() => {
            blackDisplay.transform.GetChild(0).gameObject.SetActive(true);
        });

        EnemyController.OnEnemyKill += OnEnemyKill;

        CanMove = true;
        SetFixedFps(enable, target_fps);
    }

    private void OnDestroy()
    {
        EnemyController.OnEnemyKill -= OnEnemyKill;
        enemyKilled = 0;
    }

    private void OnEnemyKill()
    {
        enemyKilled++;
    }
    private void SetFixedFps(bool enable,int targetFps)
    {
        if (enable)
            Application.targetFrameRate = targetFps;
    }
}
