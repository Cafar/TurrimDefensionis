using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class NightManager : MonoBehaviour
{

    public GameObject psalmFocus;
    public TextMeshProUGUI clock;
    public GameObject gameOverScreen;

    private TowersManager tm;
    private TypingSalmoController tsc;
    private SpawnManager sm;
    private Spawner spawner;
    private EconomyManager em;

    public static event Action onGameOver;

    void Start()
    {
        tm = gameObject.GetComponent<TowersManager>();
        tsc = gameObject.GetComponent<TypingSalmoController>();
        sm = gameObject.GetComponent<SpawnManager>();
        spawner = GameObject.Find("Spawners").GetComponentInChildren<Spawner>();
        em = GameObject.Find("GameManager").GetComponent<EconomyManager>();
        onGameOver += GameOver;

        tm.StartAllTowers();
        PsalmSetFocus(false);
    }

    void Update()
    {
        UpdateClock();
        CheckFocus();
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        if (spawner.squadIndex >= sm.wavesPerNight && gameObjects.Length == 0)
        {
            GameManager.Instance.EndNight();
        }
    }

    private void CheckFocus()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (tsc.isInFocus)
            {
                PsalmSetFocus(false);
                tm.SetAllTowersIsInFocus(true);
            }
            else
            {
                PsalmSetFocus(true);
                tm.SetAllTowersIsInFocus(false);
            }
        }
    }

    private void UpdateClock()
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

    private void PsalmSetFocus(bool focus)
    {
        psalmFocus.SetActive(focus);
        tsc.isInFocus = focus;
    }

    public void EndNight()
    {
        GameManager.Instance.EndNight();
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }


}
