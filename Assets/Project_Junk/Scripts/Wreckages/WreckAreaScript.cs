using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WreckAreaScript : MonoBehaviour
{



    [SerializeField]
    private InventoryScript wreckageInventory;
    [SerializeField]
    public int crashSiteIndex;

    [SerializeField]
    private List<itemTypes> wreckageItems = new List<itemTypes>();
    [SerializeField]
    private List<int> wreckageItemAmounts = new List<int>();

    public float salvageRadius;
    public float circleHeight, circleRadius;
    public bool isSalvageable = true;
    public bool beingSalvaged = false;
    public Transform currentSalvagePos;

    [SerializeField] private List<GameObject> wreckParts = new List<GameObject>();
    private GameObject nextWreckPartToDissolve;
    [SerializeField] public Material wreckDissolveMat;
    private int nextWreckPartIndex = 0;

    public GameObject sentDronesParent;

    private void Start()
    {
        // wreckageInventory = GetComponent<InventoryScript>();
        setWreckageInventory();
        setWreckParts();
        nextWreckPartToDissolve = wreckParts[nextWreckPartIndex];
    }

    private void setWreckParts()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            wreckParts.Add(transform.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        updateWreckParts();
    }


    public void updateWreckParts()
    {
        int divider = wreckParts.Count;
        int totalItemAmount = 0;
        for (int i = 0; i < wreckageItemAmounts.Count; i++)
        {
            totalItemAmount += wreckageItemAmounts[i];
        }

        currentSalvagePos = wreckParts[nextWreckPartIndex].transform;
        if (wreckageInventory.getCurrentStorage() >= (wreckageInventory.getMaxStorage() / divider) * (divider - (nextWreckPartIndex + 1)))
        {
            wreckParts[nextWreckPartIndex].GetComponent<WreckPartDissolveScript>().updateDissolveAmount();
        }else if(wreckageInventory.getCurrentStorage() < (wreckageInventory.getMaxStorage() / divider) * (divider - (nextWreckPartIndex + 1)))
        {
            wreckParts[nextWreckPartIndex].SetActive(false);
            nextWreckPartIndex++;
            
        }else if(wreckageInventory.getCurrentStorage() == 0)
        {
            Destroy(gameObject);
        }

    }

  
    public void setWreckageInventory()
    {
        for (int i = 0; i < wreckageItems.Count; i++)
        {
            wreckageInventory.addItem(wreckageItems[i], wreckageItemAmounts[i]);
        }
    }
    public InventoryScript getWreckageInventory { get { return wreckageInventory; } }
    


}
