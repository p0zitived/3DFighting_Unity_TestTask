using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerUI_Controller : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] PlayerStats stats;
    [Header("Objects")]
    [SerializeField] Transform canvas;
    [SerializeField] Image health;
    [SerializeField] Image stamina;
    [Header("Numbers")]
    [SerializeField] GameObject numbersPrefab;
    [Header("Enemy Count")]
    [SerializeField] Text enemyKilled;
    [Header("Death")]
    [SerializeField] Image black;
    [SerializeField] Text score;
    [SerializeField] Cinemachine.CinemachineFreeLook machine;
    private bool setedScore = false;

    private void Start()
    {
        PlayerMoveController.OnSprintCoolDown += OnSprintCoolDown;
        PlayerMoveController.OnSprintCoolDownEnd += OnSprintCoolDownEnd;
        Inventory_Controller.OnUseSlot += SpawnHealNumbers;
        PlayerStats.OnPlayerDeath += OnDeath;
    }

    private void OnDestroy()
    {
        PlayerMoveController.OnSprintCoolDown -= OnSprintCoolDown;
        PlayerMoveController.OnSprintCoolDownEnd -= OnSprintCoolDownEnd;
        Inventory_Controller.OnUseSlot -= SpawnHealNumbers;
        PlayerStats.OnPlayerDeath -= OnDeath;
    }

    private void Update()
    {
        health.fillAmount = stats.GetHealth()/stats.GetMaxHP();
        stamina.fillAmount = stats.GetStamina()/stats.GetMaxStamina();

        canvas.LookAt(Camera.main.transform);

        RefreshEnemyKilled();
    }

    private void OnSprintCoolDown()
    {
        stamina.color = Color.blue;
    }
    private void OnSprintCoolDownEnd()
    {
        stamina.color = Color.white;
    }
    private void SpawnHealNumbers(InventorySlot slot)
    {
        FoodItem food = slot.item as FoodItem;
        if (food != null) {
            GameObject aux;
            if (food.restoreHP != 0)
            {
                aux = Instantiate(numbersPrefab);
                aux.transform.position = transform.position + new Vector3(0, 1, 0);
                aux.GetComponent<DamageNumbers>().damage = food.restoreHP;
                aux.GetComponentInChildren<Text>().color = Color.red;
                aux.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            if (food.restoreStamina != 0)
            {
                aux = Instantiate(numbersPrefab);
                aux.transform.position = transform.position + new Vector3(0, 1, 0);
                aux.GetComponent<DamageNumbers>().damage = food.restoreStamina;
                aux.GetComponentInChildren<Text>().color = Color.green;
                aux.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
    }
    private void RefreshEnemyKilled()
    {
        enemyKilled.text = " Enemy killed : " + Global.enemyKilled;
    }
    private void OnDeath()
    {
        black.rectTransform.DOLocalMoveY(0, 0.5f);
        Cursor.lockState = CursorLockMode.None;
        machine.m_YAxis.m_MaxSpeed = 0;
        machine.m_XAxis.m_MaxSpeed = 0;
        AudioListener.volume = 0;

        if (!setedScore)
        {
            score.text += Global.enemyKilled;
            setedScore = true;
        }
    }

    public void Ok()
    {
        SceneManager.LoadScene(0);
    }
}
