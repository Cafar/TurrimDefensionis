using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int waveLevel = 0;
    public float[] nightTime;

    public static event Action onStartDay;
    public static event Action onStartNight;
    public static event Action onEndNight;
    public static event Action onGameOver;

    private float nightStartTime;

    void Start()
    {
        StartDay();
        nightTime = new float[6];
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
        onStartNight?.Invoke();
        nightStartTime = Time.time;
    }

    // Calcular el favor divino conseguido en la noche
    public void EndNight()
    {
        nightTime[waveLevel] = Time.time - nightStartTime;
        onEndNight?.Invoke();
        waveLevel++;
    }

    // Mostrar pantalla de game over con el botón de volver al inicio o reiniciar
    public void GameOver()
    {
        nightTime[waveLevel] = Time.time - nightStartTime;
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
