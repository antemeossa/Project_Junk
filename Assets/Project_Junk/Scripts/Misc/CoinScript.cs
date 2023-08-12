using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;
    private void FixedUpdate()
    {
        transform.Rotate(0, rotateSpeed, 0);
    }
}
