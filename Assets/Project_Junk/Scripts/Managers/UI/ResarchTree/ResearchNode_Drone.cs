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
        GameManager.Instance.DroneManager.cargoDroneCapacity = capacity;
        GameManager.Instance.DroneManager.setCargoLimitForCargoDrones();
    }

    private void increaseSpeed(float speed)
    {
        GameManager.Instance.DroneManager.setSpeedMultipliers(droneType);
    }

    private void incraseSalvageSpeed(float multiplier)
    {
        GameManager.Instance.DroneManager.salvageSpeedMultiplier = multiplier;
        GameManager.Instance.DroneManager.setSalvageSpeed();
    }

    private void addScanner()
    {
        GameManager.Instance.DroneManager.addScannerToScrapDrones(true);
    }

    private void addAdvancedScanner()
    {
        GameManager.Instance.DroneManager.addAdvancedScannerToScrapDrones(true);
    }
}
