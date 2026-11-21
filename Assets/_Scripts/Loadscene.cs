using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loadscene : MonoBehaviour
{

    [SerializeField] private string sceneToLoad;

    // The ScreenFader in this scene. Drag it in from the Hierarchy.
    [SerializeField] private FadeScreen fader;

    // True once the player has entered the portal and the screen is
    // darkening. Update then waits for the screen to be fully black.
    private bool loading = false;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (fader == null) {
                // No fader set up, so load straight away like before.
                SceneManager.LoadScene(sceneToLoad);
                return;
            }

            loading = true;
            fader.FadeToBlack();
        }
    }

    private void Update() {
        // Wait until the screen is covered, then swap scenes. The new
        // scene starts black and fades up, so the change is not a
        // sudden cut.
        if (loading && fader != null && fader.IsBlack()) {
            loading = false;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
