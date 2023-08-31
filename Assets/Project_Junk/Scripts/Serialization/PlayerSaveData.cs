using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSaveData : MonoBehaviour
{
    public PlayerData playerData = new PlayerData();

    public void setPlayerData()
    {
        playerData.money = GameManager.Instance.economyManager.currentMoney;
        playerData.maxDroneCount = GameManager.Instance.droneManager.maxDroneAmount;
        playerData.availableDroneCount = GameManager.Instance.droneManager.useableDroneAmount;
        playerData.mothershipLevel = GameManager.Instance.mothership.GetComponent<MothershipLevelScript>().motherShipLevel;
    }

    

}

[System.Serializable]
public struct PlayerData
{
    public int money;
    public int maxDroneCount;
    public int availableDroneCount;
    public int mothershipLevel;
}
