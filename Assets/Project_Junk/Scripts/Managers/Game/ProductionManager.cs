using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProductionManager : MonoBehaviour
{
    public GameObject factoryParent;
    public GameObject connectorParent;
    public List<FactoryScript> factoriesList = new List<FactoryScript>();
    public List<GameObject> allBuildings = new List<GameObject>();
    public int currentCycle, defaultCycle, cycleCounter = 0;
    public float cycleTime;
    private void Start()
    {

        
        InvokeRepeating("cycleCalculator", 0, cycleTime);
        
    }
  

    private void cycleCalculator()
    {
        if(currentCycle + 1 < defaultCycle)
        {
            currentCycle++;
            if(cycleCounter < 100)
            {
                cycleCounter++;
                GameManager.Instance.UI_M.cycle = cycleCounter;
                GameManager.Instance.UI_M.updateContractsCycle();
            }
            else
            {
                GameManager.Instance.UI_M.updateContractsCycle();
                GameManager.Instance.UI_M.updateContractsPanel();
                GameManager.Instance.UI_M.updateBlackMarketPanel();
                cycleCounter = 0;
            }
        }
        else
        {
            currentCycle = 0;
            cycleCompletedAction();
        }
    }

    private void cycleCompletedAction()
    {
        massProduce();   
        GameManager.Instance.UI_M.updateSmallDetailsPanel();
    }

    public void addFactory(FactoryScript obj)
    {
        factoriesList.Add(obj);
    }

    public void removeFactory(FactoryScript obj)
    {
        factoriesList.Remove(obj);
    }
    public void setAllBuildingsInWorld(Transform obj)
    {
        allBuildings.Clear();
        for (int i = 0; i < obj.childCount; i++)
        {
            allBuildings.Add(obj.GetChild(i).gameObject);
        }
    }
    private void massProduce()
    {
        for(int i = 0;i < allBuildings.Count; i++)
        {
            if (allBuildings[i].GetComponent<ProductionScript>() != null)
            {
                allBuildings[i].GetComponent<ProductionScript>().produce(allBuildings[i].GetComponent<InventoryScript>(), allBuildings[i].GetComponent<BuildingScript>().getSelectedRecipe);
            }
        }
    }
}
