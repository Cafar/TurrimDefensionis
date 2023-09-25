using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    public TowerData data;
    public TextMeshProUGUI towerNameMP;
    public Image towerImage;
    public TextMeshProUGUI towerCostMP;


    // Start is called before the first frame update
    void Start()
    {
        towerNameMP.text = data?.towerName;
        towerImage.sprite = data?.mapImage;
        towerImage.SetVerticesDirty();
        towerCostMP.text = data?.cost.ToString();
    }
}
