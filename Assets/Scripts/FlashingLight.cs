using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingLight : MonoBehaviour
{
    private bool increasing = true;
    public float flashSpeed = 1f;
    public float maxIntensity = 1f;

    void Update()
    {
        if (GetComponent<Light>().intensity <= 0)
            increasing = true;
        else if (GetComponent<Light>().intensity >= 1)
            increasing = false;
        if (increasing == true)
            GetComponent<Light>().intensity += flashSpeed;
        else
            GetComponent<Light>().intensity -= flashSpeed;

    }
}
