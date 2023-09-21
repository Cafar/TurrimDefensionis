using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Squad", order = 1)]
public class SquadData : ScriptableObject
{
    public EnemyData[] topRow = new EnemyData[3];
    public EnemyData[] midRow = new EnemyData[3];
    public EnemyData[] botRow = new EnemyData[3];
}
