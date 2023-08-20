using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmManager : MonoBehaviour
{
    public GameObject dronePrefab;
    public Transform pointA; // Starting point
    public Transform pointB; // Destination point
    public int numDrones = 5;
    public float speed;
    List<GameObject> drones = new List<GameObject>();
    private void Start()
    {
        spawnDrones();
        setDroneTargets();
        startMovement();
    }

    public void spawnDrones()
    {
        for (int i = 0; i < numDrones; i++)
        {
            GameObject obj = Instantiate(dronePrefab, transform, true) ;
            drones.Add(obj);
        }
    }

    public void setDroneTargets()
    {
        for(int i = 0; i < drones.Count; i++)
        {
            drones[i].GetComponent<SwarmElement>().pointA = pointA;
            drones[i].GetComponent <SwarmElement>().pointB = pointB;
            drones[i].GetComponent<SwarmElement>().setDetails(pointB.position, speed);
        }
    }

    public void startMovement()
    {
        for (int i = 0; i < drones.Count; i++)
        {
            drones[i].GetComponent<SwarmElement>().currentState = SwarmElement.DroneState.Takeoff;
        }
    }
}
