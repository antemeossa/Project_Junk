using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum droneTypesEnum
{
    ScrapDrone,
    TransportDrone,
    FighterDrone
}
public class DroneManager : MonoBehaviour
{
    [Header("Drone Stats")]
    public float baseDroneSpeed;
    [Header("Salvage Drone")]
    public float salvageDroneHP;
    public float salvageDroneSpeed, salvageDroneSpeedMultiplier;
    public float salvageSpeed, salvageSpeedMultiplier = 1f;
    public bool haveScanner = false, haveAdvancedScanner = false;
    [Header("Cargo Drone")]
    public float cargoDroneHP;
    public float cargoDroneSpeedMultiplier = 1f;
    public int cargoDroneCapacity;
    [Header("Figter Drone")]
    public float fighterDroneHP;
    public float fighterDroneSpeedMultiplier = 1f;
    public float damage;
    public float armor;

    public int maxDroneAmount;
    public int useableDroneAmount;
    public int activeDroneAmount;
    
    

    
    public List<GameObject> allDronesList = new List<GameObject>();
    public List<GameObject> transportDronesList = new List<GameObject>();
    public List<GameObject> scrapDronesList = new List<GameObject>();
    public List<GameObject> fighterDronesList = new List<GameObject>();


    public void addDrone(GameObject vehicle)
    {
        allDronesList.Add(vehicle);
    }

    public void sortDrones()
    {
        for(int i = 0; i < allDronesList.Count; i++)
        {
            if (allDronesList[i].GetComponent<DroneScript>().getDroneType == droneTypesEnum.ScrapDrone)
            {
                scrapDronesList.Add((GameObject)allDronesList[i]);
            }else if (allDronesList[i].GetComponent<DroneScript>().getDroneType == droneTypesEnum.TransportDrone)
            {
                transportDronesList.Add((GameObject)allDronesList[i]);
            }else if (allDronesList[i].GetComponent<DroneScript>().getDroneType == droneTypesEnum.FighterDrone)
            {
                fighterDronesList.Add((GameObject)allDronesList[i]);
            }

        }
    }

    public void sortDronesByFactory()
    {

    }

    #region allDronesOperations
    public void setSpeedMultipliers(droneTypesEnum droneType)
    {
        switch (droneType)
        {
            case droneTypesEnum.ScrapDrone:
                setSpeeds(scrapDronesList, salvageDroneSpeedMultiplier);
                break;
            case droneTypesEnum.TransportDrone:
                setSpeeds(transportDronesList, cargoDroneSpeedMultiplier);
                break;
            case droneTypesEnum.FighterDrone:
                setSpeeds(fighterDronesList, fighterDroneSpeedMultiplier);
                break;
        }
    }

    private void setSpeeds(List<GameObject> list, float speedMultiplier)
    {
        foreach (GameObject drone in list)
        {
            drone.GetComponent<DroneScript>().setMovementSpeed(speedMultiplier);
        }
    }

    #endregion

    #region scrapDroneOperations
    public void setSalvageSpeed()
    {
        foreach(GameObject drone in scrapDronesList)
        {
            drone.GetComponent<ScrapDroneScript>().setSalvageSpeed(salvageSpeedMultiplier);
        }
    }

    public void addScannerToScrapDrones(bool var)
    {
        foreach(GameObject drone in scrapDronesList)
        {
            drone.GetComponent<ScrapDroneScript>().addScanner(var);
        }
    }
    public void addAdvancedScannerToScrapDrones(bool var)
    {
        foreach (GameObject drone in scrapDronesList)
        {
            drone.GetComponent<ScrapDroneScript>().addAdvancedScanner(var);
        }
    }

    #endregion

    #region carDroneOperations

    public void setCargoLimitForCargoDrones()
    {
        foreach(GameObject drone in transportDronesList)
        {
            drone.GetComponent<CargoDroneScript>().setCargoLimit(cargoDroneCapacity);
        }
    }

    #endregion
}
