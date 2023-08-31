using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DroneManager : MonoBehaviour
{
   
    public int maxDroneAmount;
    public int useableDroneAmount;
    public int activeDroneAmount;
    
    

    
    public List<GameObject> allDronesList = new List<GameObject>();
    public List<GameObject> transportDronesList = new List<GameObject>();
    public List<GameObject> scrapDronesList = new List<GameObject>();
    public List<GameObject> fighterDronesList = new List<GameObject>();


    
}
