using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGameMono : MonoBehaviour
{
    public GameObject[] buildingPrefabs = new GameObject[4];
    public List<GameObject> crashSitePrefabs = new List<GameObject>();
    public Mesh connectorTurnMesh;
    public GameObject connectorNodePrefab;
    public GameObject connectorParentPrefab;
    public Transform factoryPrefab;
    public void saveGameOnClick()
    {

        SaveGameManager.currentSaveData.getAllBuildingData();
        SaveGameManager.currentSaveData.saveConnectors();
        SaveGameManager.currentSaveData.saveCrashSites();
        SaveGameManager.currentSaveData.isSaved = true;
        SaveGameManager.Save();
    }

    public void loadGameOnClick()
    {
        SaveGameManager.Load();
        SceneManager.LoadScene(0);

        //loadOperationsBuildings();
    }

    public void loadOperationsBuildings()
    {

        if (SaveGameManager.currentSaveData.isSaved)
        {
            GameObject obj = null;
            BuldingData crntData = new BuldingData();

            for (int i = 0; i < SaveGameManager.currentSaveData.buildingDataList.Count; i++)
            {
                if (SaveGameManager.currentSaveData.buildingDataList[i].buildingType.Equals(Utils.enumToString(buildingTypesEnum.Smelter)))
                {
                    crntData = SaveGameManager.currentSaveData.buildingDataList[i];
                    obj = buildingPrefabs[0];
                }
                else if (SaveGameManager.currentSaveData.buildingDataList[i].buildingType.Equals(Utils.enumToString(buildingTypesEnum.Refiner)))
                {
                    crntData = SaveGameManager.currentSaveData.buildingDataList[i];
                    obj = buildingPrefabs[1];
                }
                else if (SaveGameManager.currentSaveData.buildingDataList[i].buildingType.Equals(Utils.enumToString(buildingTypesEnum.Assembler)))
                {
                    crntData = SaveGameManager.currentSaveData.buildingDataList[i];
                    obj = buildingPrefabs[2];
                }
                else if (SaveGameManager.currentSaveData.buildingDataList[i].buildingType.Equals(Utils.enumToString(buildingTypesEnum.Storage)))
                {
                    crntData = SaveGameManager.currentSaveData.buildingDataList[i];
                    obj = buildingPrefabs[3];
                }



                GameObject spwn = Instantiate(obj);
                spwn.transform.position = crntData.position;
                spwn.transform.rotation = crntData.rotation;
                spwn.GetComponent<BuildingScript>().uniqueID = crntData.ID;
                if (spwn.GetComponent<BuildingScript>() != null)
                {
                    for (int k = 0; k < GameManager.Instance.getAllRecipes.Count; k++)
                    {
                        if (Utils.enumToString(GameManager.Instance.getAllRecipes[k].outputProduct.outputType).Equals(crntData.currentRecipe))
                        {
                            spwn.GetComponent<BuildingScript>().setSelectedRecipe(GameManager.Instance.getAllRecipes[k]);
                        }
                    }


                }

                Ray ray = new Ray(spwn.transform.position, Vector3.down);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.CompareTag("Factory"))
                    {
                        Debug.Log(hit.transform.gameObject);
                        spwn.transform.SetParent(hit.transform.parent.GetChild(3), true);

                    }
                }
                //spwn.transform.SetParent(GameManager.Instance.PM.factoriesList[0].transform);
            }
        }

    }

    public void loadOperationsConnectors()
    {


        if (SaveGameManager.currentSaveData.isSaved)
        {
            GameObject obj = Instantiate(connectorParentPrefab, GameManager.Instance.mothership.transform.GetChild(1).GetChild(2).GetChild(2), true);
            ConnectorData crntData = new ConnectorData();
            for (int i = 0; i < SaveGameManager.currentSaveData.connectorDataList.Count; i++)
            {
                crntData = SaveGameManager.currentSaveData.connectorDataList[i];
                for (int j = 0; j < crntData.connectorNodeCount; j++)
                {


                    GameObject spwn = Instantiate(connectorNodePrefab);
                    spwn.transform.position = crntData.nodePositions[j];
                    spwn.transform.rotation = crntData.nodeRotations[j];
                    spwn.transform.SetParent(obj.transform, true);
                    if (crntData.isTurnList[j])
                    {
                        spwn.GetComponent<improvedConnectorNodeScript>().switchToMerger();
                    }
                }
            }

            for (int i = 0; i < GameManager.Instance.productionManager.allBuildings.Count; i++)
            {
                if (crntData.inFacility.Equals(GameManager.Instance.productionManager.allBuildings[i].GetComponent<BuildingScript>().uniqueID))
                {
                    obj.GetComponent<TransferItemsScript>().setInFacility(GameManager.Instance.productionManager.allBuildings[i]);
                }
                else if (crntData.outFacility.Equals(GameManager.Instance.productionManager.allBuildings[i].GetComponent<BuildingScript>().uniqueID))
                {
                    obj.GetComponent<TransferItemsScript>().setOutFacility(GameManager.Instance.productionManager.allBuildings[i]);
                }

                obj.GetComponent<TransferItemsScript>().setFacilitiesBool(true);
            }
        }
    }

    public void loadOperationsWreckages()
    {

        for (int i = 0; i < GameManager.Instance.wreckageManager.allWreckages.Count; i++)
        {
            Destroy(GameManager.Instance.wreckageManager.allWreckages[i].gameObject);
        }
        if (SaveGameManager.currentSaveData.isSaved)
        {
            GameObject obj;
            int index;
            WreckageData crntData = new WreckageData();
            for (int i = 0; i < SaveGameManager.currentSaveData.wreckageDataList.Count; i++)
            {
                crntData = SaveGameManager.currentSaveData.wreckageDataList[i];

                switch (crntData.crashSiteIndex)
                {
                    case 0:
                        index = 0;
                        break;
                    case 1:
                        index = 1;
                        break;
                    case 2:
                        index = 2;
                        break;
                    case 3:
                        index = 3;
                        break;
                    default:
                        index = 0;
                        break;
                }
                obj = crashSitePrefabs[index];
                GameObject spwn = Instantiate(obj);

                spwn.transform.position = crntData.position;
                spwn.transform.rotation = crntData.rotation;
                spwn.GetComponent<InventoryScript>().setMaxStorage(crntData.maxStorage);
                for (int k = 0; k < crntData.itemAmounts.Count; k++)
                {
                    itemTypes tmpKey;
                    if (Enum.TryParse(crntData.itemNames[k], out tmpKey))
                    {
                        spwn.GetComponent<InventoryScript>().addItem(tmpKey, crntData.itemAmounts[k]);
                    }

                }

            }

        }
    }
}