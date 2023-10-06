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
    public TextMeshProUGUI descriptionText;
    public Button button;

    private EconomyManager em;
    private DayManager dm;
    private Image buttonImage;

    // Start is called before the first frame update
    void Start()
    {
        towerNameMP.text = data?.towerName;
        towerImage.sprite = data?.mapImage;
        towerImage.SetVerticesDirty();
        towerCostMP.text = data?.cost.ToString();
        em = GameObject.Find("GameManager").GetComponent<EconomyManager>();
        dm = GameObject.Find("DayManager").GetComponent<DayManager>();
        buttonImage = button.gameObject.GetComponentInChildren<Image>();
    }

    private void Update()
    {
        CheckInteractable();
        if (dm.selectedTowerData == dm.emptyTowerData)
        {
            // Disable particle effect
            buttonImage.color = Color.white;
        }
    }

    public void SetDescriptionText()
    {
        descriptionText.text = data.description;
    }

    public void SetSelectedTowerData()
    {
        dm.SetSelectedTowerData(data);
        // Activate particle effect
        buttonImage.color = Color.yellow;
    }

    private void CheckInteractable()
    {
        if (em.currentCoin < data.cost)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

}
