using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LaneWave", order = 1)]
public class LaneWaveData : ScriptableObject
{
    public SquadData[] squadSpawnOrder;
}
