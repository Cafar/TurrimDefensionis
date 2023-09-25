using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [Header("Configuration")]
    public TowerData data;
    public CooldownHandler cooldownHandler;
    public Slider cooldownSlider;
    public Slider towerHealthbar;
    public Sprite destroyedImage;
    public float destroyedImageScale = 1f;
    public TypingTowerController tpc;
    [Space(10)]

    [Header("UI")]
    public GameObject towerHealthbarEnabler;
    public Image textBackground;
    public Image cooldownFill;
    public GameObject towerPlaceNumber;
    public GameObject backgroundFocus;

    [Space(10)]

    [Header("Testing")]
    public bool isActive = false;
    public bool isDestroyed = false;
    public bool showGizmos = true;
    public float autonomyTimer = 0f;

    [Header("SOUNDS")]
    [SerializeField]
    private AudioSource buildTower;
    [SerializeField]
    private AudioSource destroyTower;
    [SerializeField]
    private AudioSource ballesta;
    [SerializeField]
    private AudioSource canon;
    [SerializeField]
    private AudioSource catapulta;
    //[SerializeField]
    //private AudioSource failComplex;
    //[SerializeField]
    //private AudioSource switchSound;


    private List<Enemy> enemiesInattackRange = new List<Enemy>();
    private List<Enemy> enemiesDamagedByTrap = new List<Enemy>();
    private Enemy currentTarget = null;
    private float lastAttackTime = 0f;
    private SpriteRenderer sp;
    private int towerResistance;
    private TowersManager tm;
    

    private void OnEnable()
    {
        GameManager.onStartNight += GameManager_OnStartNight;
    }

    private void GameManager_OnStartNight()
    {
        towerResistance = data.resistance;
    }

    public void SetTowerData(TowerData newData)
    {
        data = newData;
        towerResistance = data.resistance;
        cooldownSlider.maxValue = data.autonomyTime;
        if (data.mapImage != null)
        {
            sp.sprite = data.mapImage;
            sp.transform.localScale = Vector3.one * data.imageScaling;

        }
        towerHealthbar.value = data.resistance;
    }

    public void SetNightUIVisibility(bool setVisible)
    {
        towerHealthbarEnabler.SetActive(setVisible);
        textBackground.enabled = setVisible;
        if (setVisible == false)
            backgroundFocus.SetActive(false);
        cooldownFill.enabled = false;
        towerPlaceNumber.SetActive(!setVisible);
}

    private void Start()
    {
        tm = GameObject.Find("GameManager").GetComponent<TowersManager>();
        sp = gameObject.GetComponentInChildren<SpriteRenderer>();
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
        cooldownFill.enabled = true;
        while (autonomyTimer <= data.autonomyTime)
        {
            autonomyTimer += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        isActive = false;
        autonomyTimer = 0f;
        cooldownFill.enabled = false;
        tpc.ResumeTower();
    }

    public void ActivateTower()
    {
        isActive = true;
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
        tpc.SetTowerPaused();
        SetNightUIVisibility(false);
        isActive = false;
        isDestroyed = true;
        tm.ResumeAllTowers();
        if (destroyedImage != null)
        {
            transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = destroyedImage;
            transform.localScale = Vector3.one * destroyedImageScale;
        }
    }
}
