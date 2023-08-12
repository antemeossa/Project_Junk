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
    
    public List<GameObject> allDrones = new List<GameObject>();
    public List<GameObject> transportDrones = new List<GameObject>();
    public List<GameObject> scrapDrones = new List<GameObject>();
    public List<GameObject> fighterDrones = new List<GameObject>();


    public void addDrone(GameObject vehicle)
    {
        allDrones.Add(vehicle);
    }

    public void sortDrones()
    {
        for(int i = 0; i < allDrones.Count; i++)
        {
            if (allDrones[i].GetComponent<DroneScript>().getDroneType == droneTypesEnum.ScrapDrone)
            {
                scrapDrones.Add((GameObject)allDrones[i]);
            }else if (allDrones[i].GetComponent<DroneScript>().getDroneType == droneTypesEnum.TransportDrone)
            {
                transportDrones.Add((GameObject)allDrones[i]);
            }else if (allDrones[i].GetComponent<DroneScript>().getDroneType == droneTypesEnum.FighterDrone)
            {
                fighterDrones.Add((GameObject)allDrones[i]);
            }

        }
    }

    public void sortDronesByFactory()
    {

    }
}
