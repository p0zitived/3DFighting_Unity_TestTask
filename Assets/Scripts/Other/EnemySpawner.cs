using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject healSalves;
    [SerializeField] GameObject staminaSalves;

    private void Start()
    {
        EnemyController.OnEnemyKill += SpawnEnemy;
    }

    private void SpawnEnemy()
    {
        GameObject aux;
        int rand = Random.Range(0, 10);
        if (Global.enemyKilled % 5 == 0)
        {
            aux = Instantiate(prefab);
            aux.transform.position = (Vector3)Random.insideUnitCircle * 2 + transform.position;
        }

        if (rand == 0)
        {
            aux = Instantiate(healSalves);
            aux.transform.position = (Vector3)Random.insideUnitCircle * 10 + transform.position;
        }
        if (rand == 1)
        {
            aux = Instantiate(staminaSalves);
            aux.transform.position = (Vector3)Random.insideUnitCircle * 10 + transform.position;
        }
    }
}
