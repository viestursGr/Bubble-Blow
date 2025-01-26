using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); 
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting..."); // Won't show in builds, but useful for testing
    }
}