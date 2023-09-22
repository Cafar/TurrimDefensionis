using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchHealth : MonoBehaviour
{
    public int health = 1000;
    public int maxHealth = 1000;

    private GameManager gameManager;

    private void OnEnable()
    {
        GameManager.onStartDay += SetMaxHealth;
    }

    private void SetMaxHealth()
    {
        health = maxHealth;
    }

    private void Start()
    {
        health = maxHealth;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void TakeDamage(int damage)
    {
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
