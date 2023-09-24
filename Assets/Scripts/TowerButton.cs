using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    public TowerData data;

    private TextMeshProUGUI towerNameMP;
    private Image towerImage;
    private TextMeshProUGUI towerCostMP;

    // Start is called before the first frame update
    void Start()
    {
        towerNameMP = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0];
        towerImage = gameObject.GetComponentInChildren<Image>();
        towerCostMP = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];

        towerNameMP.text = data?.towerName;
        towerImage.sprite = data?.mapImage;
        towerImage.SetVerticesDirty();
        towerCostMP.text = data?.cost.ToString();
    }
}
