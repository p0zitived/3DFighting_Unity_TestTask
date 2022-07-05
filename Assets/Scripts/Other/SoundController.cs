using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] AudioClip EnemyHit;
    [SerializeField] AudioClip bottleUsing;
    [SerializeField] AudioClip pickUp;
    [SerializeField] GameObject prefab;

    private void Start()
    {
        AudioListener.volume = 1;
        EnemyController.OnHitedEnemy += OnEnemyHit;
        InventoryObject.OnAddItem += OnPickUp;
        Inventory_Controller.OnUseSlot += OnUse;
    }

    private void OnDestroy()
    {
        EnemyController.OnHitedEnemy -= OnEnemyHit;
        InventoryObject.OnAddItem -= OnPickUp;
        Inventory_Controller.OnUseSlot -= OnUse;
    }

    private void OnEnemyHit(float value,EnemyController controller)
    {
        GameObject aux = Instantiate(prefab);
        aux.transform.position = transform.position;
        aux.GetComponent<AudioSource>().clip = EnemyHit;
        aux.GetComponent<AudioSource>().Play();
        Destroy(aux, 5);
    }

    private void OnPickUp(ItemObject obj)
    {
        GameObject aux = Instantiate(prefab);
        aux.transform.position = transform.position;
        aux.GetComponent<AudioSource>().clip = pickUp;
        aux.GetComponent<AudioSource>().Play();
        Destroy(aux, 5);
    }

    private void OnUse(InventorySlot slot)
    {
        if (slot.item.type == ItemType.food)
        {
            GameObject aux = Instantiate(prefab);
            aux.transform.position = transform.position;
            aux.GetComponent<AudioSource>().volume = 0.5f;
            aux.GetComponent<AudioSource>().clip = bottleUsing;
            aux.GetComponent<AudioSource>().Play();
            Destroy(aux, 5);
        }
    }
}
