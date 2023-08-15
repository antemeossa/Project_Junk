using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed, rotationDirection = 1;

    private void Start()
    {
        rotationDirection = Mathf.Clamp(rotationDirection, -1f, 1f);
    }

    private void FixedUpdate()
    {
        transform.Rotate(0f, rotationSpeed * rotationDirection,0f);
    }

    private void stutterRotation()
    {
        
    }
}
