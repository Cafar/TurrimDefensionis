using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public EnemyData data;

    [SerializeField] private int health;
    private Rigidbody2D rb;
    private SpriteRenderer sp;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * data.moveSpeed;

        health = data.maxHealth;

        //sp = gameObject.GetComponent<SpriteRenderer>();
        //sp.sprite = data.mapImage;
    }

    // Update is called once per frame
    void Update()
    {
        
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
