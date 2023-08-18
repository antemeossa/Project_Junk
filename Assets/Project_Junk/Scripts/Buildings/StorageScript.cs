using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StorageScript : MonoBehaviour
{



    [SerializeField]
    GameObject storageUnit;

    private int maxStorage;
    private int currentStorage;


    public Dictionary<OutputType, int> storedItems = new Dictionary<OutputType, int>();


    private int previousStorageUnitCount = 0;
    private List<GameObject> containers = new List<GameObject>();
    [SerializeField]
    private int rowCount, columnCount, containerCapacity;
    [SerializeField]
    private float spacingX, spacingY, spacingZ;
    [SerializeField]
    Transform containerStartPos;


    private void Start()
    {
        setValues();
        setStorageUnits(20);
        setContainersActive();
    }

    private void Update()
    {
    }
    private void setValues()
    {
        maxStorage = GetComponent<InventoryScript>().getMaxStorage();
        currentStorage = GetComponent<InventoryScript>().getCurrentStorage();
        
    }

    public void setContainersActive()
    {
        currentStorage = GetComponent<InventoryScript>().getCurrentStorage();
        int storageUnitCount = (currentStorage / containerCapacity);
        for (int i = 0; i < containers.Count; i++)
        {
            containers[i].SetActive(false);
        }
        for (int i = 0; i < storageUnitCount; i++)
        {
            containers[i].SetActive(true);
        }
    }
    private void setStorageUnits(int amount)
    {
        int maxContainerCount = maxStorage / amount;
        int storageUnitCount = (currentStorage / amount);
        int stackCount = maxContainerCount / (rowCount * columnCount);
        for(int i = 0;  i < maxContainerCount; i++)
        {
            GameObject obj = Instantiate(storageUnit, transform);
            containers.Add(obj);
            obj.SetActive(false);
        }                 

        int index = 0;
        Vector3 newPosition = containerStartPos.transform.position;
        for(int stack = 0; stack < stackCount; stack++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    if (index < containers.Count)
                    {

                        containers[index].transform.position = newPosition;                        
                        newPosition = new Vector3(containerStartPos.transform.position.x + (col * spacingX),
                            containerStartPos.transform.position.y + (stack * spacingY),
                            containerStartPos.transform.position.z +( row * spacingZ));

                        index++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        





    }
}
