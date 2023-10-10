using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject credits;

    public void StartGame()
    {
        SceneManager.LoadScene("GameDay");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowCredits()
    {
        menu.SetActive(false);
        credits.SetActive(true);
    }

    public void ShowMenu()
    {
        menu.SetActive(true);
        credits.SetActive(false);
    }
}
