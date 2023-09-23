using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TowerType
{
    Projectile,
    AOE,
    Trap,
}

public enum TargettingStrategy
{
    Closest,
    ClosestUntilDeath,
    Furthest,
    Fixed,
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Tower", order = 1)]
public class TowerData : ScriptableObject
{
    [Header("General")]
    public TowerType towerType;
    public int resistance;
    public float autonomyTime;
    public Sprite mapImage;
    public float imageScaling = 1f;
    public GameObject attackEffect;
    [Space(10)]

    [Header("Attack")]
    public TargettingStrategy targettingStrategy;
    public float attackRange;
    public int attackDamage;
    public float attackRate;
    public float attackAOE;
    [Space(10)]

    [Header("Shop")]
    public string towerName;
    [TextArea(3, 10)] public string description;
    public Image shopImage;
    public int cost;
    public int[] possibleLocations;
}
