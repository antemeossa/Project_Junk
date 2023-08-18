using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHolderScript : MonoBehaviour
{
    private bool isInBorders = true;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("CamBorder")){
            isInBorders = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CamBorder")){
            isInBorders = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CamBorder")){
            isInBorders = true;
        }
    }

    public bool getIsInBorders { get { return isInBorders; } }
}
