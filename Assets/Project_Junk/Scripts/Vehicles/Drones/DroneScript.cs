using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneScript : MonoBehaviour
{
    private string droneName;
    private droneTypesEnum droneType;
    private FactoryScript currentFactory;

    private float hp, armor, movementSpeed;

    public void setDroneDetails(string name, droneTypesEnum type)
    {
        droneName = name;
        transform.name = name;
        droneType = type;
    }

    public DroneScript(float hp, float armor, float movementSpeed)
    {
        switch (droneType)
        {
            case droneTypesEnum.ScrapDrone:
                this.movementSpeed = GameManager.Instance.DroneManager.baseDroneSpeed * GameManager.Instance.DroneManager.salvageSpeedMultiplier;
                break;
            case droneTypesEnum.TransportDrone:
                this.movementSpeed = GameManager.Instance.DroneManager.baseDroneSpeed * GameManager.Instance.DroneManager.cargoDroneSpeedMultiplier;

                break;
            case droneTypesEnum.FighterDrone:
                this.movementSpeed = GameManager.Instance.DroneManager.baseDroneSpeed * GameManager.Instance.DroneManager.fighterDroneSpeedMultiplier;

                break;
        }
        this.hp = hp;
        this.armor = armor;        
        
    }

    public void setMovementSpeed(float speedMultiplier)
    {
        movementSpeed = movementSpeed * speedMultiplier;
    }
    public droneTypesEnum getDroneType
    {
        get { return droneType; }
    }
}
