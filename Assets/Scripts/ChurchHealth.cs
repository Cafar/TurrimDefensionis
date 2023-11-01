using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChurchHealth : MonoBehaviour
{
    public int maxHealth = 5000;

    public Slider churchHealthbar;

    [Header("SOUNDS")]
    [SerializeField]
    private AudioSource churchDamage;
    //[SerializeField]
    //private AudioSource type;
    //[SerializeField]
    //private AudioSource completeWord;
    //[SerializeField]
    //private AudioSource failSimple;
    //[SerializeField]
    //private AudioSource failComplex;
    //[SerializeField]
    //private AudioSource switchSound;

    private GameManager gameManager;
    private NightManager nightManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        nightManager = GameObject.Find("NightManager").GetComponent<NightManager>();
        churchHealthbar.maxValue = maxHealth;
        churchHealthbar.value = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        churchDamage.Play();
        if (churchHealthbar.value - damage < 0)
            churchHealthbar.value = 0;
        else
            churchHealthbar.value -= damage;
        // Show damage effect
        if (churchHealthbar.value == 0)
        {
            gameManager.GameOver();
            nightManager.GameOver();
        }
    }
}
