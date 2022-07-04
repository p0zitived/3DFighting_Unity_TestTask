using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumbers : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] float move_distance;

    public float damage = 0;

    private void Start()
    {
        text.text = damage+"";

        transform.LookAt(Camera.main.transform.position);
        transform.DOMove(transform.position + Random.onUnitSphere * move_distance, 2f).OnComplete(() => {
            Destroy(transform.gameObject);
        });
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
        text.text = damage + "";
    }
}
