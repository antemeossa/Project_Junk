using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapDroneScript : MonoBehaviour
{
    private float salvageSpeed;
    private bool hasScanner, hasAdvancedScanner;

    public void setSalvageSpeed(float multiplier)
    {
        salvageSpeed *= multiplier;
    }

    public void addScanner(bool var)
    {
        hasScanner = var;
    }

    public void addAdvancedScanner(bool var)
    {
        hasAdvancedScanner = var;
    }
}
