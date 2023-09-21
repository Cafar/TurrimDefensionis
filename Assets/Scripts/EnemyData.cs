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
    public Sprite mapImage;
    public GameObject attackEffect;
    public GameObject deathEffect;
}
