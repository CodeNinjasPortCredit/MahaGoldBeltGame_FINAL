using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyandgate : MonoBehaviour
{
    public GameObject gate;
    public bool HasKey = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            HasKey = true;
            Destroy(other.gameObject);
            gate.SetActive(true);
        }

    }

}
