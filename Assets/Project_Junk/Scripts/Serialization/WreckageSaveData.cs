using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckageSaveData : MonoBehaviour
{
    public WreckageData wreckageData = new WreckageData();



    public void setWreckageItems()
    {
        wreckageData.crashSiteIndex = GetComponent<WreckAreaScript>().crashSiteIndex;
        wreckageData.position = transform.position;
        wreckageData.rotation = transform.rotation;
        wreckageData.maxStorage = GetComponent<InventoryScript>().getMaxStorage();

        List<string> tmpListString = new List<string>();
        List<int> tmpListInt = new List<int>();
        foreach (var kvp in GetComponent<InventoryScript>().getInventory)
        {
            string enumKeyString = kvp.Key.ToString();
            tmpListString.Add(enumKeyString);
            tmpListInt.Add(kvp.Value);
        }

        wreckageData.itemNames = tmpListString;
        wreckageData.itemAmounts = tmpListInt;
    }
}

[System.Serializable]
public struct WreckageData
{
    public int ID;
    public int crashSiteIndex;
    public int maxStorage;
    public Vector3 position;
    public Quaternion rotation;
    public List<string> itemNames;
    public List<int> itemAmounts;
}
