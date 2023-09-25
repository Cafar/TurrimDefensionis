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
    public Button[] buyButtons;
    public TextMeshProUGUI descriptionText;

    private EconomyManager em;

    // Start is called before the first frame update
    void Start()
    {
        towerNameMP.text = data?.towerName;
        towerImage.sprite = data?.mapImage;
        towerImage.SetVerticesDirty();
        towerCostMP.text = data?.cost.ToString();
        em = GameObject.Find("GameManager").GetComponent<EconomyManager>();
    }

    private void Update()
    {
        CheckInteractable();
    }

    public void SetDescriptionText()
    {
        descriptionText.text = data.description;
    }

    private void CheckInteractable()
    {
        if (em.currentCoin < data.cost)
        {
            foreach (Button button in buyButtons)
                button.interactable = false;
        }
        else
        {
            foreach (Button button in buyButtons)
                button.interactable = true;
        }
    }

}
