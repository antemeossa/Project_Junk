using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBPScript : MonoBehaviour
{

    private enum sizeOptions { Small, Medium, Large}
    
    [SerializeField]
    private sizeOptions size;
    [SerializeField]
    private int smallStorage, mediumStorage, largeStorage;

    [SerializeField]
    private GameObject[] storageToSpawnArr = new GameObject[3];
    private GameObject storageToSpawn;

    

    private int maxStorage;
    private bool isIntake;

    private void Start()
    {
        maxStorage = 0;
    }
    public void setStorage()
    {
        if(size == sizeOptions.Small)
        {
            maxStorage = smallStorage;
            storageToSpawn = storageToSpawnArr[0];

        }else if(size == sizeOptions.Medium)
        {
            maxStorage = largeStorage;
            storageToSpawn = storageToSpawnArr[1];

        }
        else if (size == sizeOptions.Large)
        {
            maxStorage = largeStorage;
            storageToSpawn = storageToSpawnArr[2];

        }
    }
}
