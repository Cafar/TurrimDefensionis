using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerData data;

    // Implement autonomy time when activated
    public bool isActive = false;
    private List<Enemy> enemiesInattackRange = new List<Enemy>();
    private List<Enemy> enemiesDamagedByTrap = new List<Enemy>();
    private Enemy currentTarget = null;
    private float lastAttackTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (!isActive)
            return;
        if (data.towerType == TowerType.Trap)
        {
            DamageEnemiesInLineOnce(data.attackDamage);
            return;
        }
        // Don't do anything while reloading
        if (Time.time - lastAttackTime < data.attackRate)
            return;
        // Check for enemies to change target
        Debug.Log("Pasa");
        if (currentTarget == null
            || data.targettingStrategy != TargettingStrategy.ClosestUntilDeath)
        {
            DetectEnemies();
            if (enemiesInattackRange.Count > 0)
            {
                if (data.targettingStrategy == TargettingStrategy.Closest
                    || data.targettingStrategy == TargettingStrategy.ClosestUntilDeath)
                    SelectClosestEnemy();
                else if (data.targettingStrategy == TargettingStrategy.Furthest)
                    SelectFurthestEnemy();
            }
        }

        // Shoot if target is in attackRange, else change target
        if (currentTarget != null)
        {
            if (IsInRange(currentTarget))
                Shoot();
            else
                currentTarget = null;

        }
    }

    private void DetectEnemies()
    {
        // Clear the list of enemies in attackRange
        enemiesInattackRange.Clear();

        // Find all colliders in the detection radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, data.attackRange);

        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemiesInattackRange.Add(enemy);
            }
        }
    }

    private void SelectClosestEnemy()
    {
        float closestDistance = data.attackRange;

        foreach (Enemy enemy in enemiesInattackRange)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = enemy;
            }
        }
    }

    private void SelectFurthestEnemy()
    {
        float furthestDistance = 0f;

        foreach (Enemy enemy in enemiesInattackRange)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance > furthestDistance)
            {
                furthestDistance = distance;
                currentTarget = enemy;
            }
        }
    }

    private bool IsInRange(Enemy enemy)
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        if (distance <= data.attackRange)
            return true;
        return false;
    }

    private void Shoot()
    {
        lastAttackTime = Time.time;

        if (data.towerType == TowerType.AOE || data.attackAOE > 0)
            DamageArea(data.attackDamage);
        else
            currentTarget.TakeDamage(data.attackDamage);
    }

    private void DamageArea(int damage)
    {
        // Find all colliders in the detection radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(currentTarget.gameObject.gameObject.transform.position, data.attackAOE);

        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);
        }
    }

    private void DamageEnemiesInLineOnce(int damage)
    {
        // Define the starting and ending points of the line
        Vector2 start = transform.position;
        Vector2 end = transform.position + transform.up * data.attackRange;

        // Perform a linecast and get all colliders along the line
        RaycastHit2D[] hits = Physics2D.LinecastAll(start, end);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                // Check if the hit collider belongs to an enemy
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null && !enemiesDamagedByTrap.Contains(enemy))
                {
                    // Damage the enemy
                    enemy.TakeDamage(damage);
                    enemiesDamagedByTrap.Add(enemy);
                }
                if (enemiesDamagedByTrap.Count > 50)
                    enemiesDamagedByTrap.RemoveRange(0, 20);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (data.towerType != TowerType.Trap)
        {
            Gizmos.DrawWireSphere(transform.position, data.attackRange);
            if (data.towerType == TowerType.AOE || data.attackAOE > 0)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, data.attackAOE);
            }
        }
        else
        {
            Gizmos.DrawLine(transform.position, 
                transform.position + transform.up * data.attackRange);
        }
    }
}
