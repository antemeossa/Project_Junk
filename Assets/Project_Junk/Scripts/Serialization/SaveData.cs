using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData
{

    //public BuldingData buildingData = new BuldingData();
    public bool isSaved;
    public MothershipData mothership = new MothershipData();
    public PlayerData playerData = new PlayerData();
    public List<BuldingData> buildingDataList = new List<BuldingData>();
    public List<ConnectorData> connectorDataList = new List<ConnectorData>();
    public List<WreckageData> wreckageDataList = new List<WreckageData>();


    public void getAllBuildingData()
    {
        List<BuldingData> buildingDataListtmp = new List<BuldingData>();

        for (int i = 0; i < GameManager.Instance.productionManager.allBuildings.Count; i++)
        {

            buildingDataListtmp.Add(GameManager.Instance.productionManager.allBuildings[i].GetComponent<BuildingSaveData>().bldData);

        }

        buildingDataList = buildingDataListtmp;
    }

    public void saveConnectors()
    {
        List<ConnectorData> connectorDataListtmp = new List<ConnectorData>();

        for (int i = 0; i < GameManager.Instance.productionManager.connectorParent.transform.childCount; i++)
        {
            GameManager.Instance.productionManager.connectorParent.transform.GetChild(i).GetComponent<ConnectorSaveData>().setNodesPosRot();
            connectorDataListtmp.Add(GameManager.Instance.productionManager.connectorParent.transform.GetChild(i).GetComponent<ConnectorSaveData>().connectorData);

        }

        connectorDataList = connectorDataListtmp;
    }

    public void saveCrashSites()
    {
        List<WreckageData> wreckageDataListtmp = new List<WreckageData>();

        for (int i = 0; i < GameManager.Instance.wreckageManager.allWreckages.Count; i++)
        {
            GameManager.Instance.wreckageManager.allWreckages[i].GetComponent<WreckageSaveData>().setWreckageItems();

            wreckageDataListtmp.Add(GameManager.Instance.wreckageManager.allWreckages[i].GetComponent<WreckageSaveData>().wreckageData);




        }
        wreckageDataList = wreckageDataListtmp;


    }

    public void savePlayerData()
    {
        GameManager.Instance.saveDataObj.GetComponent<PlayerSaveData>().setPlayerData();
        playerData = GameManager.Instance.saveDataObj.GetComponent<PlayerSaveData>().playerData;


    }

    public void saveMothershipData()
    {
        GameManager.Instance.mothership.GetComponent<MothershipSaveData>().setMothershipData();
        mothership = GameManager.Instance.mothership.GetComponent<MothershipSaveData>().mothershipData;
    }












}
