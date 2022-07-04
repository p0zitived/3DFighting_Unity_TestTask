using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies/Create new Enemy")]
public class Enemy : ScriptableObject
{
    public float hp;
    public float damage;
    public string enemyName;
    public GameObject prefab;
}
