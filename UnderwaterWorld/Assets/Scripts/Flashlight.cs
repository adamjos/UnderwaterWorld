using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlight;
    
    private bool isTurnedOn = false;

    private void Start()
    {
        flashlight = GetComponent<Light>();        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isTurnedOn)
        {
            flashlight.enabled = true;
            isTurnedOn = true;
        }
        else if (Input.GetKeyDown(KeyCode.F) && isTurnedOn)
        {
            flashlight.enabled = false;
            isTurnedOn = false;
        }

    }

}
