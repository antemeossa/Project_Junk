using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class SwarmElement : MonoBehaviour
{
    public Transform takeOffPos;
    public GameObject targetSalvage;

    public float droneSpeed;
    public int salvageRate;
    public int loadSpeed;
    public int unloadRate;

    public DroneStates currentDroneState;

    private DroneStates nextDroneState;
    private GameObject mothership;
    private InventoryScript droneInventory;
    private bool salvagingDone = false;

    private void Start()

    {
        currentDroneState = DroneStates.Idle;
        droneInventory = GetComponent<InventoryScript>();
        droneSpeed = UnityEngine.Random.Range(droneSpeed - 20, droneSpeed + 20);

    }

    public enum DroneStates
    {
        Idle,
        TakeOff,
        Moving,
        Returning,
        Landing,
        Salvaging,
        Unloading
    }

    public void setDroneDetails(Transform takeOff, GameObject target, GameObject mothership)
    {
        takeOffPos = takeOff;
        targetSalvage = target;
        this.mothership = mothership;
    }
    private void Update()
    {
        switch (currentDroneState)
        {
            case DroneStates.Idle:
                break;
            case DroneStates.TakeOff:
                droneTakeOff(takeOffPos);
                break;
            case DroneStates.Moving:
                moveToTheWreck(targetSalvage.GetComponent<WreckAreaScript>().currentSalvagePos);
                break;
            case DroneStates.Salvaging:

                loadDroneAction();
                break;
            case DroneStates.Returning:

                ReturnFromWreck(takeOffPos);
                break;
            case DroneStates.Landing:
                droneLand(mothership.transform.position);
                break;
            case DroneStates.Unloading:
                unloadDroneAction();
                break;

        }
    }

    private void OnDestroy()
    {
        InventoryScript targetInventory = mothership.GetComponent<InventoryScript>();

        for (int i = 0; i < GameManager.Instance.getAllRecipes.Count; i++)
        {
            if (droneInventory.getInventory.ContainsKey(GameManager.Instance.getAllRecipes[i].outputProduct.outputType))
            {
                if (droneInventory.getInventory[GameManager.Instance.getAllRecipes[i].outputProduct.outputType] > 0 && targetInventory.getCurrentStorage() < targetInventory.getMaxStorage())
                {
                    targetInventory.addItem(GameManager.Instance.getAllRecipes[i].outputProduct.outputType, droneInventory.getInventory[GameManager.Instance.getAllRecipes[i].outputProduct.outputType]);
                    droneInventory.removeItem(GameManager.Instance.getAllRecipes[i].outputProduct.outputType, droneInventory.getInventory[GameManager.Instance.getAllRecipes[i].outputProduct.outputType]);
                }
            }

        }
    }

    #region movementOperations
    private void droneTakeOff(Transform end)
    {
        float step = Time.deltaTime * droneSpeed;
        transform.position = Vector3.MoveTowards(transform.position, end.position, step);

        if (Vector3.Distance(transform.position, end.position) < 0.01f)
        {
            currentDroneState = DroneStates.Moving;
        }

        //Rotate the drone in respect to movement direction
        Vector3 moveDirection = (end.position - transform.position).normalized;
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 100.0f);
        }
    }

    private void droneLand(Vector3 end)
    {
        float step = Time.deltaTime * droneSpeed;
        transform.position = Vector3.MoveTowards(transform.position, end, step);

        if (Vector3.Distance(transform.position, end) < 0.01f)
        {
            if (salvagingDone)
            {
                currentDroneState = DroneStates.Idle;

            }
            else
            {
                currentDroneState = DroneStates.Unloading;


            }
        }

        //Rotate the drone in respect to movement direction
        Vector3 moveDirection = (end - transform.position).normalized;
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 100.0f);
        }
    }


    Ray ray;
    RaycastHit hit;
    private void ReturnFromWreck(Transform end)
    {
        float step = Time.deltaTime * droneSpeed;
        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, 500f))
        {

            if (hit.transform != null && hit.transform.CompareTag("Landscape"))
            {
                if (Vector3.Distance(transform.position, end.position) > 200f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0f, 100f, 0f), step);

                }
            }

        }

        transform.position = Vector3.MoveTowards(transform.position, end.position, step);

        if (Vector3.Distance(transform.position, end.position) < 0.01f)
        {

            currentDroneState = DroneStates.Landing;

        }

        //Rotate the drone in respect to movement direction
        Vector3 moveDirection = (end.position - transform.position).normalized;
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 100.0f);
        }
    }
    private void moveToTheWreck(Transform end)
    {
        float step = Time.deltaTime * droneSpeed;
        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, 500f))
        {

            if (hit.transform != null && hit.transform.CompareTag("Landscape"))
            {
                Debug.Log("hit");
                if (Vector3.Distance(transform.position, end.position) > 200f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0f, 100f, 0f), step);

                }
            }

        }

        transform.position = Vector3.MoveTowards(transform.position, end.position + new Vector3(0f, targetSalvage.GetComponent<WreckAreaScript>().circleHeight, 0f), step);

        if (Vector3.Distance(transform.position, end.position) < 0.01f)
        {

            currentDroneState = DroneStates.Salvaging;

        }

        //Rotate the drone in respect to movement direction
        Vector3 moveDirection = (end.position - transform.position).normalized;
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 100.0f);
        }
    }
    public float salvagePerc = 0f;


    #endregion




    public float circleSpeed;
    private float angle = 0f;
    private void circlingMotion(Transform circleTarget)
    {
        Vector3 circleCenter = circleTarget.position;
        angle += circleSpeed * Time.deltaTime;
        float x = circleCenter.x + targetSalvage.GetComponent<WreckAreaScript>().circleRadius * Mathf.Cos(angle);
        float z = circleCenter.z + targetSalvage.GetComponent<WreckAreaScript>().circleRadius * Mathf.Sin(angle);

        if (Vector3.Distance(transform.position, new Vector3(x, circleCenter.y + targetSalvage.GetComponent<WreckAreaScript>().circleHeight, z)) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(x, circleCenter.y + targetSalvage.GetComponent<WreckAreaScript>().circleHeight, z)
                , Time.deltaTime * droneSpeed);

        }
        else
        {
            transform.position = new Vector3(x, circleCenter.y + targetSalvage.GetComponent<WreckAreaScript>().circleHeight, z);

        }





        // Rotate the drone to face the center of the circle
        Vector3 lookDirection = (circleCenter - transform.position).normalized;
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 100.0f);
        }


    }

    private void loadDroneAction()
    {
        if (targetSalvage != null && droneInventory != null && droneInventory.getCurrentStorage() < droneInventory.getMaxStorage())
        {
            circlingMotion(targetSalvage.transform);
        }
        
        if (droneInventory.getCurrentStorage() >= droneInventory.getMaxStorage() || targetSalvage.GetComponent<InventoryScript>().getCurrentStorage() == 0)
        {
            currentDroneState = DroneStates.Returning;
        }
    }
    private void unloadDroneAction()
    {

        
        if (droneInventory.getCurrentStorage() <= 0)
        {
            currentDroneState = DroneStates.TakeOff;
        }
    }

    private void loadDrone()
    {
        
        InventoryScript targetInventory = targetSalvage.GetComponent<InventoryScript>();

        for (int i = 0; i < GameManager.Instance.getAllRecipes.Count; i++)
        {
            if (targetInventory.getInventory.ContainsKey(GameManager.Instance.getAllRecipes[i].outputProduct.outputType))
            {
                if (targetInventory.getInventory[GameManager.Instance.getAllRecipes[i].outputProduct.outputType] > 0 && droneInventory.getCurrentStorage() < droneInventory.getMaxStorage())
                {
                    droneInventory.addItem(GameManager.Instance.getAllRecipes[i].outputProduct.outputType, salvageRate);
                    targetInventory.removeItem(GameManager.Instance.getAllRecipes[i].outputProduct.outputType, salvageRate);
                }
            }

        }
    }

    private void unloadDrone()
    {
        InventoryScript targetInventory = mothership.GetComponent<InventoryScript>();

        for (int i = 0; i < GameManager.Instance.getAllRecipes.Count; i++)
        {
            if (droneInventory.getInventory.ContainsKey(GameManager.Instance.getAllRecipes[i].outputProduct.outputType))
            {
                if (droneInventory.getInventory[GameManager.Instance.getAllRecipes[i].outputProduct.outputType] > 0 && targetInventory.getCurrentStorage() < targetInventory.getMaxStorage())
                {
                    targetInventory.addItem(GameManager.Instance.getAllRecipes[i].outputProduct.outputType, unloadRate);
                    droneInventory.removeItem(GameManager.Instance.getAllRecipes[i].outputProduct.outputType, unloadRate);
                }
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(targetSalvage))
        {
            currentDroneState = DroneStates.Salvaging;
            InvokeRepeating("loadDrone", 0, .5f);
        }else if (other.gameObject.Equals(mothership))
        {
            if(targetSalvage == null)
            {
                transform.gameObject.SetActive(false);
            }
            else
            {
                InvokeRepeating("unloadDrone", 0, .5f);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(targetSalvage))
        {
            CancelInvoke("loadDrone");
        }else if (other.gameObject.Equals(mothership))
        {
            CancelInvoke("unloadDrone");
        }
    }




















}
/*
    public Transform pointA; // Starting point (takeoff)
    public Transform pointB; // Destination point (landing)
    public GameObject salvageTarget;
    public float takeoffSpeed = 2.0f;
    public float moveSpeed = 5.0f;
    public float landingSpeed = 2.0f;
    public float circleRadius = 3.0f;
    public float circleSpeed = 1.0f;
    public float circlingHeight = 50f;
    public bool canMove = false;

    public enum DroneState { Idle, Takeoff, Moving, Landing, Circling }
    public DroneState currentState = DroneState.Idle;

    private InventoryScript droneInventory;
    private Vector3 circleCenter;
    private float angle = 0.0f;

    private Ray ray;

    public Transform MoveTarget;
    public Transform spawnPos;

    public void setDetails(Transform start, Transform end, Transform mothership,GameObject salvageTarget, float speed, float circleHeight, float circleRadius)
    {
        pointA = start;
        pointB = end;
        spawnPos = mothership;
        this.salvageTarget = salvageTarget;
        takeoffSpeed = speed;
        moveSpeed = speed;
        landingSpeed = speed;
        circlingHeight = circleHeight;
        this.circleRadius = circleRadius;
    }

    private void Start()
    {

    }


    private void Update()
    {

        switch (currentState)
        {
            case DroneState.Idle:

                break;
            case DroneState.Takeoff:
                Takeoff();
                break;

            case DroneState.Moving:
                MoveToPointB(MoveTarget);
                break;

            case DroneState.Landing:
                Land();
                break;

            case DroneState.Circling:
                CircleAroundTarget();
                InvokeRepeating("salvage", 1, 5); 
                break;
        }


    }

    public void Takeoff()
    {
        float step = takeoffSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pointA.position, step);

        if (Vector3.Distance(transform.position, pointA.position) < 0.01f)
        {
            MoveTarget = pointB;
            currentState = DroneState.Moving;
        }
    }

    public void MoveToPointB(Transform targetPoint)
    {
        float step = moveSpeed * Time.deltaTime;
        ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 500f))
        {

            if (hit.transform != null && hit.transform.CompareTag("Landscape"))
            {
                if (Vector3.Distance(transform.position, pointB.position) > 200f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0f, 100f, 0f), step);

                }
            }

        }

        transform.position = Vector3.MoveTowards(transform.position, pointB.position, step);


        // Rotate the drone in the direction of movement
        Vector3 moveDirection = (pointB.position - transform.position).normalized;
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 100.0f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.GetComponent<WreckAreaScript>() != null)
        {
            currentState = DroneState.Circling;
        }

        if(other.transform.gameObject.GetComponent<MotherShipMovement>() != null)
        {
            if(droneInventory.getCurrentStorage() > 0)
            {
                GetComponent<InventoryScript>().transferAnyItem(other.transform.gameObject.GetComponent<InventoryScript>());
            }
        }
    }
    public void CircleAroundTarget()
    {
        circleCenter = pointB.position;
        angle += circleSpeed * Time.deltaTime;
        float x = circleCenter.x + circleRadius * Mathf.Cos(angle);
        float z = circleCenter.z + circleRadius * Mathf.Sin(angle);

        if(currentState == DroneState.Circling)
        transform.position = new Vector3(x, circleCenter.y + circlingHeight, z);

        // Rotate the drone to face the center of the circle
        Vector3 lookDirection = (circleCenter - transform.position).normalized;
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 100.0f);
        }
    }

    public void Land()
    {
        float step = landingSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pointB.position, step);

        if (Vector3.Distance(transform.position, pointB.position) < 0.01f)
        {
            // Optionally, you can destroy or disable the drone here
        }
    }

    private void salvage()
    {
        if(salvageTarget != null && currentState == DroneState.Circling)
        {
            salvageTarget.GetComponent<InventoryScript>().transferAnyItem(droneInventory);
            MoveTarget = pointA;
            currentState = DroneState.Moving;
        }

        if(salvageTarget == null)
        {
            MoveTarget = spawnPos;
            currentState = DroneState.Moving;
        }
    }

   


}
*/