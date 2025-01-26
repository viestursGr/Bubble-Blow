using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void GameOver()
    {
        Application.Quit();
        // SceneManager.LoadScene("GameOver"); // Replace with the name of your Game Over scene
    }
}
