using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WheledVehicleManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] vehicleStartPos = new Transform[4];

    [SerializeField]
    private List<GameObject> vehicles = new List<GameObject>();

    [SerializeField]
    private int maxVehicleCount;
    [SerializeField]
    private Vector2 spawnInterval;

    [SerializeField]
    private Vector3 vehicleSpawnPos;

    [SerializeField]
    private float rotation1, rotation2;

    private bool hasSpawned;
    [SerializeField]
    private List<GameObject> spawnedVehicles = new List<GameObject>();
    private List<GameObject> vehiclesOnRoad = new List<GameObject>();

    [SerializeField]
    GameObject vehiclesParent;

    private void Start()
    {
        spawnVehicles();
        InvokeRepeating("spawnInTunnels", 0, Random.Range(spawnInterval.x, spawnInterval.y));
    }

    private void spawnVehicles()
    {
        if (!hasSpawned)
        {
            for (int i = 0; i < maxVehicleCount; i++)
            {
                GameObject obj = Instantiate(vehicles[0], vehicleSpawnPos, Quaternion.identity);
                spawnedVehicles.Add(obj);
                obj.transform.SetParent(vehiclesParent.transform, true);
            }
            hasSpawned = true;
        }
    }

    private int vehicleIndex = 0;
    
    private void spawnInTunnels()
    {
        
        if (spawnedVehicles[vehicleIndex].GetComponent<WheeldVehicleScript>() != null && !spawnedVehicles[vehicleIndex].GetComponent<WheeldVehicleScript>().getMoving)
        {
            int random = Random.Range(0, vehicleStartPos.Length);
            if(vehicleIndex >= spawnedVehicles.Count - 1) { vehicleIndex = 0; }
            if(random < vehicleStartPos.Length / 2)
            {
                spawnedVehicles[vehicleIndex].transform.position = vehicleStartPos[random].position;
                spawnedVehicles[vehicleIndex].transform.Rotate(new Vector3(0, rotation1,0));
                spawnedVehicles[vehicleIndex].GetComponent<WheeldVehicleScript>().setCanMove(true);
                //spawnedVehicles[vehicleIndex].GetComponent<WheeldVehicleScript>().setMoveRot(-1);

                vehicleIndex++;
            }
            else
            {
                spawnedVehicles[vehicleIndex].transform.position = vehicleStartPos[random].position;
                spawnedVehicles[vehicleIndex].transform.Rotate(new Vector3(0, rotation2, 0));
                spawnedVehicles[vehicleIndex].GetComponent<WheeldVehicleScript>().setCanMove(true);
                vehicleIndex++;
            }
                
           
            
        }


    }

}
