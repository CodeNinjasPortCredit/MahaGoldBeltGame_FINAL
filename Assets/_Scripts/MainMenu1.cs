using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu1 : MonoBehaviour
{
    // Call this function to load a scene by its exact name
    public void LoadSceneByName()
    {
        SceneManager.LoadScene("MahaInterior");
    }

    // Optional: Call this function to close the game (useful for a Quit Button)
    public void QuitGame()
    {
        Debug.Log("Game Exiting...");
        Application.Quit();
    }
}
