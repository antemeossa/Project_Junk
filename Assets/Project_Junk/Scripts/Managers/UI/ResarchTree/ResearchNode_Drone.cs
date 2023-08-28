using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ResearchNode_Drone : MonoBehaviour
{
    private droneTypesEnum droneType;

    public void applyResearch()
    {
       
        
    }

    private void increaseStorageCapacity(int capacity)
    {
        GameManager.Instance.droneManager.cargoDroneCapacity = capacity;
        GameManager.Instance.droneManager.setCargoLimitForCargoDrones();
    }

    private void increaseSpeed(float speed)
    {
        GameManager.Instance.droneManager.setSpeedMultipliers(droneType);
    }

    private void incraseSalvageSpeed(float multiplier)
    {
        GameManager.Instance.droneManager.salvageSpeedMultiplier = multiplier;
        GameManager.Instance.droneManager.setSalvageSpeed();
    }

    private void addScanner()
    {
        GameManager.Instance.droneManager.addScannerToScrapDrones(true);
    }

    private void addAdvancedScanner()
    {
        GameManager.Instance.droneManager.addAdvancedScannerToScrapDrones(true);
    }
}
