using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawnManager : MonoBehaviour
{
    public Transform[] spawnPositions = new Transform[5];
    public Transform[] startPositions = new Transform[5];

    public GameObject dronePrefab;
    public GameObject targetWreckage;

    public List<GameObject> allSpawnedDrones = new List<GameObject>();
    //public List<GameObject> totalSpawnedDrones = new List<GameObject>();
    [SerializeField] private GameObject allDronesParent;
    [SerializeField] private GameObject droneSwarmParent;

    [SerializeField] private int spawnedDrones;
    [SerializeField] private int maxDroneAmount;

    private Transform currentSpawnPos;
    private Transform currentStartPos;

    private void Start()
    {

    }

    //Checking the start positions of the drones according to the crashsite's position relative to the mothership
    public Transform setSpawnPos()
    {
        if (Utils.checkPositonOfObjectXZ(targetWreckage) == 1)
        {
            currentStartPos = startPositions[1];
            currentSpawnPos = spawnPositions[1];
            return spawnPositions[1];
        }
        else if (Utils.checkPositonOfObjectXZ(targetWreckage) == 2)
        {
            currentStartPos = startPositions[2];
            currentSpawnPos = spawnPositions[2];
            return spawnPositions[2];
        }
        else if (Utils.checkPositonOfObjectXZ(targetWreckage) == 3)
        {
            currentStartPos = startPositions[3];
            currentSpawnPos = spawnPositions[3];
            return spawnPositions[3];
        }
        else if (Utils.checkPositonOfObjectXZ(targetWreckage) == 4)
        {
            currentStartPos = startPositions[4];
            currentSpawnPos = spawnPositions[4];
            return spawnPositions[4];
        }
        else
        {
            return null;
        }
    }

    /*The right way to do it would be spawning some chunks of drones and utilizing them by
    activating and deactivating them. But due to deadline I didn't have the time to do that way
    and fix the bugs. Instantiating and destroying is quite performance heavy but it works without any bugs.*/
    public void sendDrones(int numberOfDrones)
    {
        setSpawnPos();
        GameObject currentSwarmParent;



        if (targetWreckage.GetComponent<WreckAreaScript>() != null)
        {
            if (targetWreckage.GetComponent<WreckAreaScript>().sentDronesParent == null)
            {
                currentSwarmParent = Instantiate(droneSwarmParent, setSpawnPos(), false);
                targetWreckage.GetComponent<WreckAreaScript>().sentDronesParent = currentSwarmParent;
                currentSwarmParent.transform.SetParent(allDronesParent.transform, false);

                for (int i = 0; i < numberOfDrones; i++)
                {
                    GameObject drone = Instantiate(dronePrefab, spawnPositions[0]);
                    drone.transform.SetParent(currentSwarmParent.transform, false);
                    currentSwarmParent.GetComponent<SwarmManager>().drones.Add(drone);
                    drone.transform.position = currentSpawnPos.position;

                }
                currentSwarmParent.GetComponent<SwarmManager>().setSwarmManagerDetails(
            currentStartPos,
            targetWreckage,
            transform.parent.gameObject
            );
            }
            


        }









    }

    public void cancelSalvage(GameObject obj)
    {
        Destroy(obj.GetComponent<WreckAreaScript>().sentDronesParent.gameObject);

    }


}
