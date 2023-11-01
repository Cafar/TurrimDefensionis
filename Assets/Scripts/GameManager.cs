using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }



    public int nightLevel;
    public float[] nightTimes;
    public TowerData emptyTowerData;

    public int savedChurchHealth;

    [Header("SOUNDS")]
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip endNightClip;
    [SerializeField]
    private AudioClip baseDayClip;
    [SerializeField]
    private AudioClip baseNightClip;
    [SerializeField]
    private AudioClip baseLastNightClip;
    [SerializeField]
    private AudioClip gameOverClip;


    // public static event Action onStartDay;
    // public static event Action onStartNight;
    public static event Action onEndNight;
    public static event Action onGameOver;


    public List<TowerData> towerDataList;
    private float nightStartTime;
    private bool isDay;

    private EconomyManager economyManager;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        nightTimes = new float[5];
        towerDataList = new List<TowerData>{emptyTowerData, emptyTowerData, emptyTowerData, emptyTowerData, emptyTowerData,
                                            emptyTowerData, emptyTowerData, emptyTowerData, emptyTowerData, emptyTowerData};
        economyManager = gameObject.GetComponent<EconomyManager>();
    }

    public void RestartGame()
    {
        nightLevel = 0;
        nightTimes = new float[5];
        towerDataList = new List<TowerData>{emptyTowerData, emptyTowerData, emptyTowerData, emptyTowerData, emptyTowerData,
                                            emptyTowerData, emptyTowerData, emptyTowerData, emptyTowerData, emptyTowerData};
        economyManager.ResetEconomy();
    }


    

    void Update()
    {
        CheckPause();        
    }

    private void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isDay)
        {
            if (Time.timeScale == 1)
            {
                // tm.PauseAllTowers();
                Time.timeScale = 0;
            }
            else
            {
                // tm.ResumeAllTowers();
                Time.timeScale = 1;
            }

        }
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
    }

    // Desactiva y resetea las palabras de activaci�n de torres (Done)
    // Desactiva y resetea salmo (Done)
    // Desactiva todos los elementos de UI de noche (Done)
    // Desactiva todas las torretas (Done)
    // Desactiva los spawners de enemigos (Done)
    // Destruye todos los enemigos (Done)
    // Pone al m�ximo la vida de la iglesia (Done)
    // Activa todos los elementos de UI de d�a (Done)
    // Activa m�sica de d�a
    // (Primer d�a) Activa banner tutorial d�a
    // public void StartDay()
    // {
    //     isDay = true;
    //     onStartDay?.Invoke();
    //     nightUI.SetActive(false);
    //     dayUI.SetActive(true);
    //     nightBackground.SetActive(false);
    //     dayBackground.SetActive(true);
    //     descriptionText.text = initialDescriptionText;
    //     audioSource.clip = baseDayClip;
    //     audioSource.Play();
    // }

    // Desactiva todos los elementos de UI de d�a
    // Activa todos los elementos de UI de noche
    // Activa los spawners de enemigos (Done)
    // Inicializa palabras de activaci�n de torres (Done)
    // Inicializa salmo
    // Activa m�sica de noche
    // (Primera noche) Activa banner tutorial noche
    public void EndDay()
    {
        isDay = false;
        // psalmFocus.SetActive(false);
        SceneManager.LoadScene("GameNight");
        nightStartTime = Time.time;
        // audioSource.clip = baseNightClip;
        // audioSource.Play();

    }

    public void EndNight()
    {
        isDay = true;
        nightTimes[nightLevel] = Time.time - nightStartTime;
        economyManager.GetEndOfNightReward();
        nightLevel++;
        onEndNight?.Invoke();
        SceneManager.LoadScene("GameDay");
        // audioSource.PlayOneShot(endNightClip);
    }

    // Mostrar pantalla de game over con el bot�n de volver al inicio o reiniciar
    public void GameOver()
    {
        nightTimes[nightLevel] = Time.time - nightStartTime;
        onGameOver?.Invoke();
        // audioSource.PlayOneShot(gameOverClip);

    }

    public void ReturnToMenu()
    {
        RestartGame();
        SceneManager.LoadScene("Menu");
    }

}
