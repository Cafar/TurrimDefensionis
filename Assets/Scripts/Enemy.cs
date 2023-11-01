using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public EnemyData data;
    public Animator animator;
    public Collider enemyCollider;
    public bool isDead = false;

    [SerializeField] private int health;
    private Rigidbody rb;
    private SpriteRenderer sp;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * data.moveSpeed;

        health = data.maxHealth;

        sp = gameObject.GetComponentInChildren<SpriteRenderer>();
        sp.transform.localScale = Vector3.one * data.imageScaling;

        animator.runtimeAnimatorController = data.animator;
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
        animator.SetTrigger("takeDamage");
        if (health <= 0 && !isDead)
            Die();

    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("die");
        enemyCollider.enabled = false;
        transform.gameObject.tag = "Untagged";
    }

}
