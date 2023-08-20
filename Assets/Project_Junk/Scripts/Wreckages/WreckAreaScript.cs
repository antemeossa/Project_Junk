using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckAreaScript : MonoBehaviour
{

    private bool canSalvage = false;

    [SerializeField]
    private InventoryScript wreckageInventory;

    [SerializeField]
    private List<itemTypes> wreckageItems = new List<itemTypes>();
    [SerializeField]
    private List<int> wreckageItemAmounts = new List<int>();

    private void Start()
    {
       // wreckageInventory = GetComponent<InventoryScript>();
        setWreckageInventory();
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
