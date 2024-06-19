using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EcranTitre : MonoBehaviour
{
    public GameObject ecranTitre;
    public GameObject credits;

    public void Begin()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenEcranTitre()
    {
        ecranTitre.SetActive(true);
    }

    public void OpenCredits()
    {
        credits.SetActive(true);
    }

    public void CloseEcranTitre()
    {
        ecranTitre.SetActive(false);
    }

    public void CloseCredits()
    {
        credits.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
