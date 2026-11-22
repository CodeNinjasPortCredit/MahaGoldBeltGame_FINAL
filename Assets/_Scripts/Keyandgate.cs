using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keyandgate : MonoBehaviour
{
    public GameObject gate;
    public bool HasKey = false;

    // Objective line shown in the corner of the screen. Drag the text object
    // in here in the Inspector. If it is left empty the key still works
    // exactly as before, there is just no message.
    public TMP_Text objectiveText;
    public string objectiveBeforeKey = "Get the key";
    public string objectiveAfterKey = "Find the exit";


    private void Start()
    {
        ShowObjective(objectiveBeforeKey);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            HasKey = true;
            Destroy(other.gameObject);

            if (gate != null)
            {
                gate.SetActive(true);
            }

            ShowObjective(objectiveAfterKey);
        }

    }

    private void ShowObjective(string message)
    {
        if (objectiveText != null)
        {
            objectiveText.text = message;
        }
    }

}
