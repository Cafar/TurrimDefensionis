using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public EnemyData data;

    [SerializeField] private int health;
    private Rigidbody rb;
    private SpriteRenderer sp;


    private void OnEnable()
    {
        GameManager.onStartDay += GameManager_OnStartDay;
    }

    private void GameManager_OnStartDay()
    {
        Die();
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * data.moveSpeed;

        health = data.maxHealth;

        if (data.mapImage != null)
        {
            sp = gameObject.GetComponentInChildren<SpriteRenderer>();
            sp.sprite = data.mapImage;
            sp.transform.localScale = Vector3.one * data.imageScaling;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ChurchHealth church = other.gameObject.GetComponent<ChurchHealth>();
        if (church != null)
        {
            church.TakeDamage(data.attackDamage);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage");
        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Enemy destroyed");
        Destroy(gameObject);
        // death effect
    }

}
