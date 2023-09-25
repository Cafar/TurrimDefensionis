using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int nightLevel = 0;
    public float[] nightTimes;
    public GameObject dayUI;
    public GameObject nightUI;
    public GameObject dayBackground;
    public GameObject nightBackground;
    [TextArea(3, 10)] public string initialDescriptionText;
    public TextMeshProUGUI descriptionText;
    public GameObject psalmFocus;
    public TextMeshProUGUI clock;

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
    //[SerializeField]
    //private AudioSource failComplex;
    //[SerializeField]
    //private AudioSource switchSound;

    public static event Action onStartDay;
    public static event Action onStartNight;
    public static event Action onEndNight;
    public static event Action onGameOver;

    private float nightStartTime;

    private TowersManager tm;
    private TypingSalmoController tsc;
    private SpawnManager sm;
    private Spawner spawner;
    private bool isDay;

    void Start()
    {
        StartDay();
        nightTimes = new float[6];
        tm = gameObject.GetComponent<TowersManager>();
        tsc = gameObject.GetComponent<TypingSalmoController>();
        sm = gameObject.GetComponent<SpawnManager>();
        spawner = GameObject.Find("Spawners").GetComponentInChildren<Spawner>();
    }

    void Update()
    {
        UpdateClock();
        CheckFocus();
        CheckPause();
        if (!isDay && spawner.squadIndex >= sm.wavesPerNight)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                EndNight();
        }
        
    }

    private void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isDay)
        {
            if (Time.timeScale == 1)
            {
                tm.PauseAllTowers();
                Time.timeScale = 0;
            }
            else
            {
                tm.ResumeAllTowers();
                Time.timeScale = 1;
            }

        }
    }

    private void CheckFocus()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (tsc.isInFocus)
            {
                tsc.isInFocus = false;
                psalmFocus.SetActive(false);
                tm.SetAllTowersIsInFocus(true);
            }
            else
            {
                tsc.isInFocus = true;
                psalmFocus.SetActive(true);
                tm.SetAllTowersIsInFocus(false);
            }
        }
    }

    private void UpdateClock()
    {
        if (!isDay)
        {
            int time = 6 + (((spawner.squadIndex - 1) / 2)) % 6;
            string amOrPm = " pm";
            if ((((spawner.squadIndex - 1) / 2)) / 6 == 1)
            {
                amOrPm = " am";
                time -= 6;
            }
            clock.text = time.ToString() + amOrPm;
        }
    }

    // Desactiva y resetea las palabras de activación de torres (Done)
    // Desactiva y resetea salmo (Done)
    // Desactiva todos los elementos de UI de noche (Done)
    // Desactiva todas las torretas (Done)
    // Desactiva los spawners de enemigos (Done)
    // Destruye todos los enemigos (Done)
    // Pone al máximo la vida de la iglesia (Done)
    // Activa todos los elementos de UI de día (Done)
    // Activa música de día
    // (Primer día) Activa banner tutorial día
    public void StartDay()
    {
        isDay = true;
        onStartDay?.Invoke();
        nightUI.SetActive(false);
        dayUI.SetActive(true);
        nightBackground.SetActive(false);
        dayBackground.SetActive(true);
        descriptionText.text = initialDescriptionText;
        audioSource.clip = baseDayClip;
        audioSource.Play();
    }

    // Desactiva todos los elementos de UI de día
    // Activa todos los elementos de UI de noche
    // Activa los spawners de enemigos (Done)
    // Inicializa palabras de activación de torres (Done)
    // Inicializa salmo
    // Activa música de noche
    // (Primera noche) Activa banner tutorial noche
    public void StartNight()
    {
        isDay = false;
        dayUI.SetActive(false);
        nightUI.SetActive(true);
        dayBackground.SetActive(false);
        nightBackground.SetActive(true);
        psalmFocus.SetActive(false);
        onStartNight?.Invoke();
        nightStartTime = Time.time;
        audioSource.clip = baseNightClip;
        audioSource.Play();

    }

    // Calcular el favor divino conseguido en la noche
    public void EndNight()
    {
        nightTimes[nightLevel] = Time.time - nightStartTime;
        onEndNight?.Invoke();
        nightLevel++;
        StartDay();
        audioSource.PlayOneShot(endNightClip);
    }

    // Mostrar pantalla de game over con el botón de volver al inicio o reiniciar
    public void GameOver()
    {
        nightTimes[nightLevel] = Time.time - nightStartTime;
        onGameOver?.Invoke();
        audioSource.PlayOneShot(gameOverClip);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("Game");
    }

}
