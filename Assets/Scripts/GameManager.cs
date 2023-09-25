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

    public static event Action onStartDay;
    public static event Action onStartNight;
    public static event Action onEndNight;
    public static event Action onGameOver;

    private float nightStartTime;

    private TowersManager tm;
    private TypingSalmoController tsc;
    private SpawnManager sm;
    private Spawner spawner;

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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (tsc.isInFocus)
            {
                tsc.isInFocus = false;
                tm.SetAllTowersIsInFocus(true);
            }
            else
            {
                tsc.isInFocus = true;
                tm.SetAllTowersIsInFocus(false);
            }
        }
        if (spawner.squadIndex >= sm.wavesPerNight)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                EndNight();
        }
    }

    // Desactiva y resetea las palabras de activación de torres
    // Desactiva y resetea salmo
    // Desactiva todos los elementos de UI de noche
    // Desactiva todas las torretas (Done)
    // Desactiva los spawners de enemigos (Done)
    // Destruye todos los enemigos (Done)
    // Pone al máximo la vida de la iglesia (Done)
    // Activa todos los elementos de UI de día
    // Activa música de día
    // (Primer día) Activa banner tutorial día
    public void StartDay()
    {
        onStartDay?.Invoke();
        nightUI.SetActive(false);
        dayUI.SetActive(true);
        nightBackground.SetActive(false);
        dayBackground.SetActive(true);
        descriptionText.text = initialDescriptionText;
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
        dayUI.SetActive(false);
        nightUI.SetActive(true);
        dayBackground.SetActive(false);
        nightBackground.SetActive(true);
        onStartNight?.Invoke();
        nightStartTime = Time.time;
    }

    // Calcular el favor divino conseguido en la noche
    public void EndNight()
    {
        nightTimes[nightLevel] = Time.time - nightStartTime;
        onEndNight?.Invoke();
        nightLevel++;
        StartDay();
    }

    // Mostrar pantalla de game over con el botón de volver al inicio o reiniciar
    public void GameOver()
    {
        nightTimes[nightLevel] = Time.time - nightStartTime;
        onGameOver?.Invoke();
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
