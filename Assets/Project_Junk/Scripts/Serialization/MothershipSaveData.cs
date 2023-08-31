using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothershipSaveData : MonoBehaviour
{
    public MothershipData mothershipData = new MothershipData();

    public void setMothershipData()
    {

        mothershipData.maxStorage = GameManager.Instance.mothership.GetComponent<InventoryScript>().getMaxStorage();
        mothershipData.hasLanded = GameManager.Instance.mothership.GetComponent<MotherShipMovement>().hasLanded;

            List<string> tmpListString = new List<string>();
            List<int> tmpListInt = new List<int>();
            foreach (var kvp in GetComponent<InventoryScript>().getInventory)
            {
                string enumKeyString = kvp.Key.ToString();
                tmpListString.Add(enumKeyString);
                tmpListInt.Add(kvp.Value);
            }

        mothershipData.itemNamesList = tmpListString;
        mothershipData.itemAmountList = tmpListInt;
        
    }
}



[System.Serializable]
public struct MothershipData
{
    public bool hasLanded;
    public int maxStorage;
    public List<string> itemNamesList;
    public List<int> itemAmountList;
}
