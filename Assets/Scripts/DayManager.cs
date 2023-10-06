using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DayManager : MonoBehaviour
{

    [Header("Configuration")]
    public TowerData emptyTowerData;
    public TextMeshProUGUI coinText;
    public DayTower[] dayTowers;

    [Space(10)]

    [Header("Testing")]
    public TowerData selectedTowerData;

    private EconomyManager economyManager;
    private DayTower clickedTower;

    // Start is called before the first frame update
    void Start()
    {
        selectedTowerData = emptyTowerData;
        economyManager = GameObject.Find("GameManager").GetComponent<EconomyManager>();
    }

    void Update()
    {
        coinText.text = economyManager.currentCoin.ToString();
        HandleClickOnPlatform();
    }

    private void HandleClickOnPlatform()
    {
        if (Input.GetMouseButtonDown(0) && selectedTowerData != emptyTowerData)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Platform"))
                {
                    // Handle the click event on the object
                    clickedTower = hitInfo.collider.gameObject.transform.parent.GetComponent<DayTower>();
                    // Check if the index is valid
                    if (Array.Exists(selectedTowerData.possibleLocations, element => element == clickedTower.index))
                    {
                        // Assign the selected tower to the platform
                        economyManager.SpendCoin(selectedTowerData);
                        clickedTower.SetTowerData(selectedTowerData);
                        GameManager.Instance.towerDataList[clickedTower.index] = selectedTowerData;
                    }
                }
            }
            SetHighlightAllPossiblePositions(selectedTowerData, false);
            selectedTowerData = emptyTowerData;
        }
    }

    public void SetSelectedTowerData(TowerData towerData)
    {
        selectedTowerData = towerData;
        SetHighlightAllPossiblePositions(towerData, true);
    }

    private void SetHighlightAllPossiblePositions(TowerData towerData, bool highlight)
    {
        foreach (DayTower tower in dayTowers)
        {
            if (Array.Exists(towerData.possibleLocations, element => element == tower.index))
            {
                tower.SetHighlight(highlight);
            }
        }
    }


}
