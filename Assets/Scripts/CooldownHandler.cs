using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownHandler : MonoBehaviour
{
    public Tower tower;

    private Slider slider;

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        if (slider == null)
            gameObject.SetActive(false);
        if (tower == null)
            gameObject.SetActive(false);
        slider.maxValue = tower.data.autonomyTime;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = tower.data.autonomyTime - tower.autonomyTimer;
    }
}
