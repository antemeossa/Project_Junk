using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData
{

    //public BuldingData buildingData = new BuldingData();
    public bool isSaved;
    public List<BuldingData> buildingDataList = new List<BuldingData>();
    public List<ConnectorData> connectorDataList = new List<ConnectorData>();
    public List<WreckageData> wreckageDataList = new List<WreckageData>();

    public void getAllBuildingData()
    {
        for (int i = 0; i < GameManager.Instance.productionManager.allBuildings.Count; i++)
        {
            buildingDataList.Add(GameManager.Instance.productionManager.allBuildings[i].GetComponent<BuildingSaveData>().bldData);
        }
    }

    public void saveConnectors()
    {
        for (int i = 0; i < GameManager.Instance.productionManager.connectorParent.transform.childCount; i++)
        {
            GameManager.Instance.productionManager.connectorParent.transform.GetChild(i).GetComponent<ConnectorSaveData>().setNodesPosRot();
            connectorDataList.Add(GameManager.Instance.productionManager.connectorParent.transform.GetChild(i).GetComponent<ConnectorSaveData>().connectorData);
        }
    }

    public void saveCrashSites()
    {
        for (int i = 0; i < GameManager.Instance.wreckageManager.allWreckages.Count; i++)
        {
            GameManager.Instance.wreckageManager.allWreckages[i].GetComponent<WreckageSaveData>().setWreckageItems();
            wreckageDataList.Add(GameManager.Instance.wreckageManager.allWreckages[i].GetComponent<WreckageSaveData>().wreckageData);

        }
    }












}
