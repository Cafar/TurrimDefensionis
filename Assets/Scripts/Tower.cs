using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [Header("Configuration")]
    public int index;
    public TowerData data;
    public Slider cooldownSlider;
    public Slider towerHealthbar;
    public Sprite destroyedImage;
    public float destroyedImageScale = 1f;
    public TypingTowerController tpc;
    public Animator animator;
    [Space(10)]

    [Header("UI")]
    public GameObject backgroundFocus;
    public GameObject uiCanvas;

    [Space(10)]

    [Header("Testing")]
    public bool isActive = false;
    public bool isDestroyed = false;
    public bool showGizmos = true;
    public float autonomyTimer = 0f;

    [Header("SOUNDS")]
    private AudioSource audioSource;
    //[SerializeField]
    //private AudioSource failComplex;
    //[SerializeField]
    //private AudioSource switchSound;


    private List<Enemy> enemiesInattackRange = new List<Enemy>();
    private List<Enemy> enemiesDamagedByTrap = new List<Enemy>();
    private Enemy currentTarget = null;
    private float lastAttackTime = 0f;
    private int towerResistance;
    private TowersManager tm;


    public void SetTowerData(TowerData newData)
    {
        data = newData;
        towerResistance = data.resistance;
        cooldownSlider.maxValue = data.autonomyTime;
        animator.runtimeAnimatorController = data.animator;
        towerHealthbar.value = data.resistance;
    }

    private void Start()
    {
        tm = GameObject.Find("NightManager").GetComponent<TowersManager>();
        // audioSource = GetComponent<AudioSource>();
        // if (audioSource == null)
        //     gameObject.AddComponent<AudioSource>();
        SetTowerData(GameManager.Instance.towerDataList[index]);
        isActive = false;
        cooldownSlider.value = 0f;
        if (data.resistance == 0)
            uiCanvas.SetActive(false);
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

    private IEnumerator StartTowerCooldown()
    {
        cooldownSlider.value = cooldownSlider.maxValue;
        while (cooldownSlider.value > 0f)
        {
            cooldownSlider.value -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        cooldownSlider.value = 0f;
        isActive = false;
        animator.SetBool("isActive", false);
        if (!tm.towerSelected)
            tpc.ResumeTower();
    }

    public void ActivateTower()
    {
        isActive = true;
        animator.SetBool("isActive", isActive);
        StartCoroutine(StartTowerCooldown());
    }

    private void DetectEnemies()
    {
        // Clear the list of enemies in attackRange
        enemiesInattackRange.Clear();

        // Find all colliders in the detection radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, data.attackRange);

        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && !enemy.isDead)
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
        animator.SetTrigger("shoot");
        // if (data.attackSound)
        //     audioSource.PlayOneShot(data.attackSound);
        if (data.towerType == TowerType.AOE || data.attackAOE > 0)
            DamageArea(data.attackDamage);
        else
        {
            currentTarget.TakeDamage(data.attackDamage);
            if (currentTarget.isDead)
                currentTarget = null;
        }

    }

    private void DamageArea(int damage)
    {
        // Find all colliders in the detection radius
        Collider[] colliders = Physics.OverlapSphere(currentTarget.gameObject.gameObject.transform.position, data.attackAOE);

        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);
        }
    }

    private void DamageEnemiesInLineOnce(int damage)
    {
        // Define the starting and ending points of the line
        Vector3 start = transform.position;
        Vector3 direction = transform.right;

        // Perform a linecast and get all colliders along the line
        RaycastHit[] hits = Physics.RaycastAll(start, direction, data.attackRange);

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
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
        if (showGizmos)
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
                    transform.position + transform.right * data.attackRange);
            }
        }
    }

    public void TakeDamage()
    {
        towerResistance--;
        towerHealthbar.value = towerResistance;
        if (towerResistance <= 0)
            DestroyTower();
    }

    private void DestroyTower()
    {
        // if (data.destroySound)
        //     audioSource.PlayOneShot(data.destroySound);
        tpc.SetTowerPaused();
        isActive = false;
        isDestroyed = true;
        animator.SetTrigger("destroy");
        tm.ResumeAllTowers();
        if (destroyedImage != null)
        {
            transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = destroyedImage;
            transform.localScale = Vector3.one * destroyedImageScale;
        }
        uiCanvas.SetActive(false);
    }
}
