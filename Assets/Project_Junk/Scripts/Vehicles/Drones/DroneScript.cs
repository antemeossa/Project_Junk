using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneScript : MonoBehaviour
{
    private string droneName;
    private droneTypesEnum droneType;
    private FactoryScript currentFactory;

    public void setDroneDetails(string name, droneTypesEnum type)
    {
        droneName = name;
        transform.name = name;
        droneType = type;
    }

    public droneTypesEnum getDroneType
    {
        get { return droneType; }
    }
}
