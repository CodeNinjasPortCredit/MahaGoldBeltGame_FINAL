using UnityEngine;
using UnityEngine.UI;

// Fades a full-screen black Image in and out.
// The scene starts fully black and clears, so arriving in a new scene
// looks smooth instead of snapping in. Loadscene calls FadeToBlack()
// before it loads the next scene.
//
// This works the same way as the stamina bar in PlayerControllerTopDown:
// a number is moved up or down a little bit each frame using
// Time.deltaTime. Here the number is the alpha (how solid the black is)
// instead of how much stamina is left.
public class FadeScreen : MonoBehaviour
{
    [SerializeField] private Image blackImage;

    // How fast the screen fades. 2 means it takes about half a second,
    // because alpha goes from 0 to 1 and 1 / 2 = 0.5 seconds.
    [SerializeField] private float fadeSpeed = 2f;

    private bool fadingToBlack = false;

    void Start()
    {
        // Begin covered, so the scene fades up from black.
        SetAlpha(1f);
    }

    void Update()
    {
        if (blackImage == null) return;

        float alpha = blackImage.color.a;

        if (fadingToBlack)
        {
            alpha += fadeSpeed * Time.deltaTime;   // getting darker
        }
        else
        {
            alpha -= fadeSpeed * Time.deltaTime;   // getting clearer
        }

        // Clamp01 keeps the value between 0 and 1 so it cannot overshoot.
        SetAlpha(Mathf.Clamp01(alpha));
    }

    // Loadscene calls this when the player steps into a portal.
    public void FadeToBlack()
    {
        fadingToBlack = true;
    }

    // Loadscene checks this so it only loads once the screen is covered.
    public bool IsBlack()
    {
        if (blackImage == null) return true;
        return blackImage.color.a >= 1f;
    }

    private void SetAlpha(float alpha)
    {
        if (blackImage == null) return;

        Color colour = blackImage.color;
        colour.a = alpha;
        blackImage.color = colour;
    }
}
