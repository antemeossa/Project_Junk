using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoDroneScript : MonoBehaviour
{
    private int cargoLimit;

    public void setCargoLimit(int capacity)
    {
        cargoLimit = capacity;
    }
}
