using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTower : MonoBehaviour
{

    public int index;
    public SpriteRenderer towerImage;

    private TowerData towerData;
    private DayManager dm;

    // Start is called before the first frame update
    void Start()
    {
        dm = GameObject.Find("DayManager").GetComponent<DayManager>();
        SetTowerData(GameManager.Instance.towerDataList[index]);
    }

    public void SetTowerData(TowerData newData)
    {
        towerData = newData;
        if (towerData.mapImage != null)
        {
            towerImage.sprite = towerData.mapImage;
            towerImage.transform.localScale = Vector3.one * towerData.imageScaling;
        }
    }

    public void SetHighlight(bool highlight)
    {
        // Activate the highlight particle
        if (highlight)
            towerImage.color = Color.yellow;
        else
            towerImage.color = Color.white;
    }
}
