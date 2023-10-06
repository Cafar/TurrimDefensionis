using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchHealth : MonoBehaviour
{
    public int health = 5000;
    public int maxHealth = 5000;

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

    // private void OnEnable()
    // {
    //     GameManager.onStartDay += GameManager_OnStartDay;
    // }

    // private void GameManager_OnStartDay()
    // {
    //     health = maxHealth;
    // }

    private void Start()
    {
        health = maxHealth;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void TakeDamage(int damage)
    {
        churchDamage.Play();
        Debug.Log("The church took " + damage + " damage");
        health -= damage;
        // Show damage effect
        if (health <= 0)
        {
            Debug.Log("The church was destroyed. Game Over");
            gameManager.GameOver();
        }
    }
}
