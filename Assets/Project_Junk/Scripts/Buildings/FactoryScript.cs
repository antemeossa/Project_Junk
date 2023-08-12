using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryScript : MonoBehaviour
{
    [SerializeField]
    private GameObject Grid, allConnectors;
    [SerializeField]
    private List<GameObject> allBuildingsInFactory = new List<GameObject>();
    private List<GameObject> storages = new List<GameObject>();
    private List<GameObject> smelters = new List<GameObject>();
    private List<GameObject> refiners = new List<GameObject>();
    private List<GameObject> assemblers = new List<GameObject>();

    private void Start()
    {
        if(allBuildingsInFactory.Count > 0)
        updateBuildingList();
    }

    public void updateBuildingList()
    {
        allBuildingsInFactory.Clear();
        for(int i = 0; i < Grid.transform.childCount; i++)
        {
            if(Grid.transform.GetChild(i).GetComponent<BuildingScript>() != null)
            {
                allBuildingsInFactory.Add(Grid.transform.GetChild(i).gameObject);
            }
        }

        sortBuildingsOnFactory();
    }

    public void sortBuildingsOnFactory()
    {
        smelters.Clear();
        refiners.Clear();
        assemblers.Clear();
        storages.Clear();
        for (int i = 0; i < allBuildingsInFactory.Count; i++)
        {
            if (allBuildingsInFactory[i].GetComponent<BuildingScript>().getBuildingType.Equals(buildingTypesEnum.Smelter))
            {
                smelters.Add(allBuildingsInFactory[i]);
            }else if (allBuildingsInFactory[i].GetComponent<BuildingScript>().getBuildingType.Equals(buildingTypesEnum.Refiner))
            {
                refiners.Add(allBuildingsInFactory[i]);
            }else if (allBuildingsInFactory[i].GetComponent<BuildingScript>().getBuildingType.Equals(buildingTypesEnum.Assembler))
            {
                assemblers.Add(allBuildingsInFactory[i]);
            }else if (allBuildingsInFactory[i].GetComponent<BuildingScript>().getBuildingType.Equals(buildingTypesEnum.Storage))
            {
                storages.Add(allBuildingsInFactory[i]);
            }
        }
    }
       

    public List<GameObject> getAllBuildingsOnFactory { get { return allBuildingsInFactory; } }

    public List<GameObject> getSpecificBuildingsList(buildingTypesEnum type)
    {
        if(type == buildingTypesEnum.Smelter)
        {
            return smelters;
        }else if (type == buildingTypesEnum.Refiner)
        {
            return refiners;
        }
        else if(type == buildingTypesEnum.Assembler)
        {
            return assemblers;
        }else if (type == buildingTypesEnum.Storage)
        {
            return storages;
        }
        else
        {
            return allBuildingsInFactory;
        }
    }

    public Transform getAllConnectorsTransform { get { return allConnectors.transform; } }
    public GameObject getGrid { get { return Grid; } }
}
