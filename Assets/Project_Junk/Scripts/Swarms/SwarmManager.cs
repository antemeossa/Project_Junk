using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmManager : MonoBehaviour
{
    public Transform pointA; // Starting point
    public Transform pointB; // Destination point
    public Transform takeOffPos;
    public GameObject mothership;
    public GameObject salvageTarget;
    public List<GameObject> drones = new List<GameObject>();
    public float speed;
    public float circleHeight;
    public float circleRadius;
    private void Start()
    {
        setDroneTargets();
        StartCoroutine(sendDrones());
    }


    public void setSwarmManagerDetails(Transform takeoff, GameObject target, GameObject mothership)
    {

        takeOffPos = takeoff;
        salvageTarget = target;
        this.mothership = mothership;

    }
    public void setDroneTargets()
    {
        for (int i = 0; i < drones.Count; i++)
        {

            drones[i].GetComponent<SwarmElement>().setDroneDetails(takeOffPos, salvageTarget, mothership);
        }
    }

    public void startMovement(int Index)
    {

        drones[Index].GetComponent<SwarmElement>().currentDroneState = SwarmElement.DroneStates.TakeOff;

    }
    int droneIndex = 0;
    IEnumerator sendDrones()
    {
        float currentT = 0;


        while (currentT < 1)
        {

            currentT += Time.deltaTime;
            yield return null;
        }

        if (droneIndex < drones.Count)
        {
            startMovement(droneIndex);
            droneIndex++;
            StartCoroutine(sendDrones());
        }

        yield return null;
    }
}
