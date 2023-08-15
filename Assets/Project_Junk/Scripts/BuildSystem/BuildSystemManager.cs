using JetBrains.Annotations;
using SplineMesh;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public enum buildingTypesEnum
{
    Storage,
    Smelter,
    Refiner,
    Assembler,
    Connector

}
[System.Serializable]
public struct buildRequirements
{
    public itemTypes requiredProduct;
    public int requiredAmount;
}
[System.Serializable]
public class Building
{

    public buildingTypesEnum buildingType;
    public List<buildRequirements> buildRequirements;
    public GameObject buildingBlueprint;


}



public class BuildSystemManager : MonoBehaviour
{

    //Serializeable Vars

    public GameManager GM;
    public ProductionManager PM;
    public PlayerController PC;
    public float gridSizeX, gridSizeY, placementHeight = 200f;
    public List<Building> buildableObjects;

    [SerializeField]
    private GameObject currentFactory;
    private GameObject prefab_BP, prefab_Built;
    private Building selectedBP;
    private ImprovedConnectorScript connector;




    public bool isBuilding, isConnecting;







    #region BuildingActions


    public void highlighConnectableBuildings(GameObject selectedObj)
    {
        BuildingScript slctdBldng;
        List<GameObject> tmpList;
        List<GameObject> tmpStorageList;

        resetHighlights();

        if (selectedObj != null && selectedObj.GetComponent<BuildingScript>() != null)
        {
            slctdBldng = selectedObj.GetComponent<BuildingScript>();
            tmpStorageList = currentFactory.transform.parent.GetComponent<FactoryScript>().getSpecificBuildingsList(buildingTypesEnum.Storage);

            if (slctdBldng.getBuildingType.Equals(buildingTypesEnum.Smelter))
            {
                tmpList = currentFactory.transform.parent.GetComponent<FactoryScript>().getSpecificBuildingsList(buildingTypesEnum.Refiner);

            }
            else if (slctdBldng.getBuildingType.Equals(buildingTypesEnum.Refiner))
            {
                tmpList = currentFactory.transform.parent.GetComponent<FactoryScript>().getSpecificBuildingsList(buildingTypesEnum.Assembler);

            }
            else
            {
                tmpList = new List<GameObject>();
            }

            for (int i = 0; i < tmpList.Count; i++)
            {
                tmpList[i].GetComponent<Renderer>().material.color = Color.yellow;
            }

            for (int i = 0; i < tmpStorageList.Count; i++)
            {
                tmpStorageList[i].GetComponent<Renderer>().material.color = Color.yellow;
            }
        }
    }

    public void resetHighlights()
    {
        for (int i = 0; i < currentFactory.transform.parent.GetComponent<FactoryScript>().getAllBuildingsOnFactory.Count; i++)
        {
            //currentFactory.transform.parent.GetComponent<FactoryScript>().getAllBuildingsOnFactory[i].GetComponent<Renderer>().material.color = Color.white;
        }
    }


    public void buildConnector()
    {
        
        if (transform.GetComponent<ImprovedConnectorScript>() != null)
        {
            connector = transform.GetComponent<ImprovedConnectorScript>();
            if (connector.enabled)
            {
                connector.enabled = false;
                activateCurrentGrid(false);
                GM.currentMode = currentModeType.PlayMode;
            }
            else
            {
                connector.enabled = true;
                GM.currentMode = currentModeType.ConnectionMode;
            }
        }
    }

    public void cancelConnector()
    {
        if (transform.GetComponent<ImprovedConnectorScript>() != null)
        {
            isConnecting = false;
            connector = transform.GetComponent<ImprovedConnectorScript>();
            connector.enabled = false;
        }
    }

    public void buildObjectOnClick(GameObject BP)
    {
        if (prefab_BP == null)
        {
            prefab_BP = Instantiate(BP);
            if (prefab_BP.GetComponent<PlaceableObject>() != null)
            {
                if (prefab_BP.GetComponent<PlaceableObject>().getObjectToBuild.GetComponent<BuildingScript>() != null)
                {
                    prefab_Built = prefab_BP.GetComponent<PlaceableObject>().getObjectToBuild;
                }

            }
            isBuilding = true;
        }
        else
        {
            Destroy(prefab_BP);
            prefab_BP = null;

            prefab_BP = Instantiate(BP);
            if (prefab_BP.GetComponent<PlaceableObject>() != null)
            {
                if (prefab_BP.GetComponent<PlaceableObject>().getObjectToBuild.GetComponent<BuildingScript>() != null)
                {
                    prefab_Built = prefab_BP.GetComponent<PlaceableObject>().getObjectToBuild;
                }

            }
            isBuilding = true;
        }




    }


    public void buildObject()
    {
        PlaceableObject placeableObj;
        GameObject obj;
        if(isBuilding)
        {
            if (prefab_BP.GetComponent<PlaceableObject>() != null && !EventSystem.current.IsPointerOverGameObject())
            {
                placeableObj = prefab_BP.GetComponent<PlaceableObject>();
                if (GM.currentMode == currentModeType.BuildMode)
                {
                    if (placeableObj.checkCanBuild())
                    {

                        obj = Instantiate(prefab_Built, prefab_BP.transform.position, prefab_BP.transform.rotation);
                        obj.transform.parent = currentFactory.transform;
                        obj.GetComponent<BuildingScript>().playPlacementVFX();
                        Destroy(prefab_BP.gameObject);
                        prefab_BP = null;
                        isBuilding = false;
                        currentFactory.transform.parent.GetComponent<FactoryScript>().updateBuildingList();
                        PM.setAllBuildingsInWorld();

                    }
                }
            }
        }
        
    }

    public void deleteBuilding(GameObject obj)
    {
        
        
        Destroy(obj.gameObject);
        
    }

    public void cancelBuildAction()
    {
        isBuilding = false;
        Destroy(prefab_BP);
        prefab_BP = null;
        if (GM.currentMode.Equals(currentModeType.PlayMode))
        {
            activateCurrentGrid(false);
        }
        
    }

    public void activateCurrentGrid(bool activate)
    {
        if(activate)
        {
            if(currentFactory != null)
            {
                currentFactory.GetComponent<Renderer>().enabled = true;
            }
        }
        else
        {
            if (currentFactory != null)
            {
                currentFactory.GetComponent<Renderer>().enabled = false;
            }
        }
    }

    #endregion

    #region Getters/Setters

    public GameObject getCurrentFactory { get { return currentFactory; } }
    public void setCurrentFactory(GameObject obj) { 
        if(currentFactory == null)
        {
            currentFactory = obj;
            currentFactory.transform.parent.GetComponent<SelectableObject>().selectIt();
        }
        else
        {
            if (currentFactory != obj)
            {
                currentFactory.transform.parent.GetComponent<SelectableObject>().deselectIt();
                currentFactory = obj;
                if(currentFactory != null)
                {
                    currentFactory.transform.parent.GetComponent<SelectableObject>().selectIt();
                }
                
            }
            else if (currentFactory == obj)
            {
                if (currentFactory != null)
                {
                    currentFactory.transform.parent.GetComponent<SelectableObject>().selectIt();
                }
            }
        }
        
    }

    #endregion


























    /*
    private void Start()
    {
        isBuilding = false;
        //PM = FindFirstObjectByType<ProductionManager>();

    }

    public void buildButtonOnClick(GameObject BP)
    {
        prefab_BP = Instantiate(BP);
        prefab_Built = prefab_BP.GetComponent<PlaceableObject>().getObjectToBuild;
        isBuilding = true;

    }
    public void buildObject()
    {

        if (prefab_BP.GetComponent<PlaceableObject>().getCanBuild)
        {
            GameObject obj = Instantiate(prefab_Built, prefab_BP.transform.position, prefab_BP.transform.rotation);
            obj.transform.SetParent(currentFactory.transform, true);

            if (prefab_BP.GetComponent<PlaceableObject>().getPreviousObject != null)
            {
                Debug.Log("out");

                prefab_BP.GetComponent<PlaceableObject>().getPreviousObject.GetComponent<Renderer>().material.color = Color.white;
                if (checkConnectible(prefab_BP, prefab_BP.GetComponent<PlaceableObject>().getPreviousObject))
                {
                    obj.transform.SetParent(prefab_BP.GetComponent<PlaceableObject>().getPreviousObject.transform, true);
                    Debug.Log("in");
                }

            }

            PM.updateBuildings();
            Destroy(prefab_BP);
            isBuilding = false;
        }




    }
    public void cancelBuilding()
    {
        Destroy(prefab_BP);
        isBuilding = false;
    }

    public bool checkConnectible(GameObject connectableObj, GameObject objToConnect)
    {
        if (connectableObj.GetComponent<PlaceableObject>().getPlaceableBuildingType == "Smelter")
        {
            if (objToConnect.GetComponent<BuildingScript>().getBuildingType == "Refiner")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (connectableObj.GetComponent<PlaceableObject>().getPlaceableBuildingType == "Refiner")
        {
            if (objToConnect.GetComponent<BuildingScript>().getBuildingType == "Assembler")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (connectableObj.GetComponent<PlaceableObject>().getPlaceableBuildingType == "Assembler")
        {
            return false;
        }
        else { return false; }
    }


    #region getters and setters

    public bool getIsBuilding { get { return isBuilding; } }



    #endregion

    */
}
