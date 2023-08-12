using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class crashableShips : ScriptableObject
{
    public string shipName;
    public string description;
    public int salvageTime;
    public Mesh mesh;
    public List<itemTypes> materialsInside;

    
}

public class DebrisManager : MonoBehaviour
{
    private List<crashableShips> shipTypes = new List<crashableShips>();
    private List<GameObject> totalDebris = new List<GameObject>();
    private Vector3 crashLocation;

    private void setCrashLocation()
    {
        if(totalDebris.Count > 0)
        {

        }
        else
        {
            
        }

        crashLocation.y += 500;
    }

    private void crashShip()
    {
        setCrashLocation();
        Instantiate(shipTypes[Random.Range(0, shipTypes.Count)], crashLocation, Quaternion.identity);
    }
}
