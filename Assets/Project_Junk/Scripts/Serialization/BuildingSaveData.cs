using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BuildingSaveData : MonoBehaviour
{
    public BuldingData bldData = new BuldingData();


    private void Start()
    {
        bldData.ID = GetComponent<BuildingScript>().uniqueID;
        bldData.buildingType = Utils.enumToString(GetComponent<BuildingScript>().getBuildingType);
    }


    private void Update()
    {

        bldData.position = transform.position;
        bldData.rotation = transform.rotation;
        bldData.placedDown = GetComponent<BuildingScript>().placedDown;
        //bldData.currentRecipe = Utils.enumToString(GetComponent<BuildingScript>().getSelectedRecipe.outputProduct.outputType);


    }

}

[System.Serializable]
public struct BuldingData
{
    //Building Details
    public int ID;
    public string buildingType;
    public bool placedDown;
    public Vector3 position;
    public Quaternion rotation;

    //Production Details
    public string currentRecipe;
    


}
