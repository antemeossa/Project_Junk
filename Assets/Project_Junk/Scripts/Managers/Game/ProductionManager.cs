using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProductionManager : MonoBehaviour
{
    public GameObject factoryParent;
    public List<GameObject> factoriesList = new List<GameObject>();
    public List<GameObject> allBuildings = new List<GameObject>();
    public int currentCycle, defaultCycle;
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

    public void setAllBuildingsInWorld()
    {
        allBuildings.Clear();
        for(int i = 0;i < factoriesList.Count; i++)
        {
            for (int j = 0; j < factoriesList[i].GetComponent<FactoryScript>().getAllBuildingsOnFactory.Count; j++)
            {
                allBuildings.Add(factoriesList[i].GetComponent<FactoryScript>().getAllBuildingsOnFactory[j]);
            }
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
