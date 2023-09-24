using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Text/TowerText", order = 1)]
public class TowerTextData: ScriptableObject
{
    public string[] words = new string[7];
}

