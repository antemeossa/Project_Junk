using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 startPoint, targetPoint;           // Starting point and Destination point
    [SerializeField]
    private float targetHeight = 10.0f, movementSpeed, ascentSpeed, descentSpeed; // Target height for the drone's flight
   
    private bool isFlying = false;

    private void Start()
    {
        // Set initial position to pointA
        //transform.position = startPoint;
        
    }

    

    public void moveDronw(Vector3 start,  Vector3 target)
    {
        startPoint = start;
        targetPoint = target;

        StartCoroutine(MoveToDestination());
    }
    private IEnumerator MoveToDestination()
    {
        isFlying = true;

        // Take off vertically        
        while (transform.position.y < targetHeight)
        {
            transform.Translate(Vector3.up * ascentSpeed * Time.deltaTime);
            yield return null;
        }

        // Move horizontally to the destination point
        float startTime = Time.time;
        while (Time.time - startTime < Vector3.Distance(startPoint, targetPoint) / movementSpeed)
        {
            float distanceCovered = (Time.time - startTime) * movementSpeed;
            float fractionOfJourney = distanceCovered / Vector3.Distance(startPoint, targetPoint);
            Vector3 newPosition = Vector3.Lerp(startPoint, targetPoint, fractionOfJourney);
            newPosition.y = targetHeight; // Maintain the target height
            transform.position = newPosition;

            // Rotate the drone to face the movement direction
            Vector3 targetDirection = (targetPoint - startPoint).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            // Rotate the thrusters based on the drone's velocity direction            

            yield return null;
        }

        // Land the drone vertically
        while (transform.position.y > startPoint.y)
        {
            transform.Translate(Vector3.down * descentSpeed * Time.deltaTime);
            yield return null;
        }

        // Reset isFlying flag
        isFlying = false;
    }


}
