using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyData : ScriptableObject
{
    [Header("General")]
    public int maxHealth;
    public float moveSpeed;
    public int attackDamage;
    public int loot;
    [Space(10)]

    [Header("Visuals")]
    public RuntimeAnimatorController animator;
    public Sprite mapImage;
    public float imageScaling = 1f;
    public GameObject attackEffect;
    public GameObject deathEffect;
}
