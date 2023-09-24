using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChurchHealthbarHandler : MonoBehaviour
{
    private Slider slider;
    private ChurchHealth churchHealth;

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        if (slider == null)
            gameObject.SetActive(false);
        churchHealth = GameObject.Find("Church").GetComponent<ChurchHealth>();
        if (churchHealth == null)
            gameObject.SetActive(false);
        slider.maxValue = churchHealth.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = churchHealth.health;
    }
}
