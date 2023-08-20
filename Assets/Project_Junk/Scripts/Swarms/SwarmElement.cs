using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmElement : MonoBehaviour
{
    public Transform pointA; // Starting point (takeoff)
    public Transform pointB; // Destination point (landing)
    public float takeoffSpeed = 2.0f;
    public float moveSpeed = 5.0f;
    public float landingSpeed = 2.0f;
    public float circleRadius = 3.0f;
    public float circleSpeed = 1.0f;
    public float spreadRadius = 20f;
    public bool canMove = false;

    public enum DroneState {Idle, Takeoff, Moving, Landing, Circling }
    public DroneState currentState = DroneState.Idle;
    private Vector3 takeoffStartPosition;
    private Vector3 landingEndPosition;
    private Vector3 circleCenter;
    private float angle = 0.0f;
    private Ray ray;

    private void Start()
    {
        
    }

    public void setDetails(Vector3 target, float speed)
    {
        takeoffStartPosition = transform.position;
        landingEndPosition = pointB.position;
        moveSpeed = speed;
        takeoffSpeed = speed;
        landingSpeed = speed;
        circleCenter = landingEndPosition;
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
                    MoveToPointB();
                    spread();
                    break;

                case DroneState.Landing:
                    Land();
                    break;

                case DroneState.Circling:
                    CircleAroundTarget();
                    break;
            }
        
        
    }

    public void Takeoff()
    {
        float step = takeoffSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pointA.position, step);

        if (Vector3.Distance(transform.position, pointA.position) < 0.01f)
        {
            currentState = DroneState.Moving;
        }
    }

    public void MoveToPointB()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, landingEndPosition, step);

        if (Vector3.Distance(transform.position, landingEndPosition) < 0.01f)
        {
            currentState = DroneState.Circling;
        }

        // Rotate the drone in the direction of movement
        Vector3 moveDirection = (landingEndPosition - transform.position).normalized;
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 100.0f);
        }
    }

    public void CircleAroundTarget()
    {
        angle += circleSpeed * Time.deltaTime;
        float x = circleCenter.x + circleRadius * Mathf.Cos(angle);
        float z = circleCenter.z + circleRadius * Mathf.Sin(angle);

        transform.position = new Vector3(x, circleCenter.y + 20f, z);

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
        transform.position = Vector3.MoveTowards(transform.position, landingEndPosition, step);

        if (Vector3.Distance(transform.position, landingEndPosition) < 0.01f)
        {
            // Optionally, you can destroy or disable the drone here
        }
    }

    public void spread()
    {
        
        

        RaycastHit hit;

        if (Physics.SphereCast(ray, spreadRadius,out hit, spreadRadius))
        {
            if (hit.transform.CompareTag("Drone"))
            {
                if((transform.position - hit.transform.position).sqrMagnitude < spreadRadius)
                {
                    transform.position += Utils.GetRandomUnitVector();
                }
            }
        }
    }
}
