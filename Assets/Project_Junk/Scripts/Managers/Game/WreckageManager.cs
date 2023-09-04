using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckageManager : MonoBehaviour
{
    public List<GameObject> allWreckages = new List<GameObject>();

    [SerializeField]
    public GameObject wreckageParent;

    private void Start()
    {
        Invoke("setWreckageList", 4);
        Invoke("removeDuplicates", 5);
        
    }

    public void setWreckageList()
    {
        allWreckages.Clear();

        for (int i = 0; i < wreckageParent.transform.childCount; i++)
        {
            if (wreckageParent.transform.GetChild(i) == null)
            {
                allWreckages.Remove(allWreckages[i]);
            }
        }

        for (int i = 0; i < wreckageParent.transform.childCount; i++)
        {
            allWreckages.Add(wreckageParent.transform.GetChild(i).gameObject);
        }

    }

    public void removeDuplicates()
    {

        HashSet<GameObject> uniqueGameObjects = new HashSet<GameObject>();
        List<GameObject> duplicatesRemovedList = new List<GameObject>();

        foreach (GameObject obj in allWreckages)
        {
            if (uniqueGameObjects.Add(obj))
            {
                // If the object is not already in the HashSet, it's unique.
                duplicatesRemovedList.Add(obj);
            }
            else
            {
                // If the object is already in the HashSet, it's a duplicate and won't be added.
                // You can also destroy the duplicate GameObject here if needed.
                Destroy(obj);
            }
        }

        // Replace the original list with the list containing duplicates removed.
        allWreckages = duplicatesRemovedList;

    }
    public void unlockCrashSites(int x)
    {
        setWreckageList();

        if (x == 1)
        {
            for (int i = 0; i < allWreckages.Count; i++)
            {
                if (allWreckages[i].GetComponent<WreckAreaScript>().crashSiteType.Equals(crashSiteTypes.NeonFighters))
                {
                    allWreckages[i].GetComponent<WreckAreaScript>().isSalvageable = true;
                }
            }
        }
        else if (x == 2)
        {
            for (int i = 0; i < allWreckages.Count; i++)
            {
                if (allWreckages[i].GetComponent<WreckAreaScript>().crashSiteType.Equals(crashSiteTypes.NeonFighters) ||
                    allWreckages[i].GetComponent<WreckAreaScript>().crashSiteType.Equals(crashSiteTypes.HeavyCargoShip))
                {
                    allWreckages[i].GetComponent<WreckAreaScript>().isSalvageable = true;
                }
            }
        }
        else if (x == 3)
        {
            for (int i = 0; i < allWreckages.Count; i++)
            {
                if (allWreckages[i].GetComponent<WreckAreaScript>().crashSiteType.Equals(crashSiteTypes.NeonFighters) ||
                    allWreckages[i].GetComponent<WreckAreaScript>().crashSiteType.Equals(crashSiteTypes.HeavyCargoShip) ||
                    allWreckages[i].GetComponent<WreckAreaScript>().crashSiteType.Equals(crashSiteTypes.Freighter))
                {
                    allWreckages[i].GetComponent<WreckAreaScript>().isSalvageable = true;
                }
            }
        }
    }
}
