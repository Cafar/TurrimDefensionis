using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public int waveLevel = 0;
    public float[] nightTime;

    public static event Action OnStartDay;
    public static event Action OnStartNight;
    public static event Action OnEndNight;
    public static event Action OnGameOver;

    private float nightStartTime;

    void Start()
    {
        StartDay();
        nightTime = new float[6];
    }

    void Update()
    {
        
    }

    // Desactiva todos los elementos de UI de noche
    // Desactiva todas las torretas
    // Desactiva los spawners de enemigos
    // Destruye todos los enemigos en pantalla
    // Activa todos los elementos de UI de d�a
    // Activa m�sica de d�a
    // (Primer d�a) Activa banner tutorial d�a
    public void StartDay()
    {
        OnStartDay?.Invoke();
    }

    // Desactiva todos los elementos de UI de d�a
    // Activa todos los elementos de UI de noche
    // Activa los spawners de enemigos
    // Inicializa palabras de activaci�n de torres
    // Inicializa salmo
    // Activa m�sica de noche
    // (Primera noche) Activa banner tutorial noche
    public void StartNight()
    {
        OnStartNight?.Invoke();
        nightStartTime = Time.time;
    }

    // Calcular el favor divino conseguido en la noche
    public void EndNight()
    {
        nightTime[waveLevel] = Time.time - nightStartTime;
        waveLevel++;
        OnEndNight?.Invoke();
    }

}
