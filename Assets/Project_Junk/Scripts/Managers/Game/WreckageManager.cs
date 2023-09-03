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
        setWreckageList();
    }

    public void setWreckageList()
    {
        allWreckages.Clear();

        for (int i = 0; i < wreckageParent.transform.childCount; i++)
        {
            if(wreckageParent.transform.GetChild(i) == null)
            {
                allWreckages.Remove(allWreckages[i]);
            }
        }

        for (int i = 0; i < wreckageParent.transform.childCount; i++)
        {
            allWreckages.Add(wreckageParent.transform.GetChild(i).gameObject);
        }

    }
    public void unlockCrashSites(int x)
    {
        setWreckageList();

       if(x == 1)
        {
            for(int i = 0;i < allWreckages.Count; i++)
            {
                if (allWreckages[i].GetComponent<WreckAreaScript>().crashSiteType.Equals(crashSiteTypes.NeonFighters))
                {
                    allWreckages[i].GetComponent<WreckAreaScript>().isSalvageable = true;
                }
            }
        }else if(x == 2)
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
        else if(x == 3)
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
