using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    public GameObject gameOverUI;

    void Update()
    {
        if (gameOverUI != null && gameOverUI.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            restart();
        }
    }

    // The "restart?" buttons on the game over screen call this by name.
    // Reloading the active scene puts the player back at the start of the level.
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
