using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [SerializeField]
    private int maxStorage;
    private int currentStorage;

    
    private Dictionary<itemTypes, int> inventory = new Dictionary<itemTypes, int>();



    private void Start()
    {
        addItem(itemTypes.ScrapCopper, 200);
        addItem(itemTypes.ScrapIron, 200);



    }
    public void addItem(itemTypes type, int amount)
    {
        if (currentStorage + amount <= maxStorage)
        {
            if (inventory.ContainsKey(type))
            {
                inventory[type] += amount;
                currentStorage += amount;

            }
            else
            {
                inventory.Add(type, amount);
                currentStorage += amount;
            }

        }
        else if (currentStorage + amount > maxStorage)
        {
            if (inventory.ContainsKey(type))
            {
                inventory[type] += maxStorage - currentStorage;
                currentStorage += maxStorage - currentStorage;

            }
            else
            {
                inventory.Add(type, maxStorage - currentStorage);
                currentStorage += maxStorage - currentStorage;

            }
        }
    }

    public void addProducedItem(itemTypes type, int amount)
    {

        if (inventory.ContainsKey(type))
        {
            inventory[type] += amount;

        }
        else
        {
            inventory.Add(type, amount);
        }

    }



    public void removeItem(itemTypes type, int amount)
    {
        if (inventory.ContainsKey(type))
        {
            if (inventory[type] - amount >= 0)
            {
                inventory[type] -= amount;
                currentStorage -= amount;
            }
        }
    }

    public void transferItem(itemTypes type, int amount, InventoryScript toInventory)
    {
        if (inventory.ContainsKey(type))
        {
            if (inventory[type] >= amount)
            {
                if (toInventory.getCurrentStorage() + amount > toInventory.getMaxStorage())
                {
                    int amountToRemove = toInventory.getMaxStorage() - toInventory.getCurrentStorage();                    
                    removeItem(type, amountToRemove);
                    currentStorage -= amountToRemove;
                    toInventory.addItem(type, amountToRemove);
                    toInventory.setCurrentStorage(amountToRemove);
                }
                else
                {
                    removeItem(type, amount);
                    currentStorage -= amount;
                    toInventory.addItem(type, amount);
                    toInventory.setCurrentStorage(amount);
                }
                
            }
        }
    }

    public int getItemCount(itemTypes type)
    {
        if (inventory.ContainsKey(type))
        {
            return inventory[type];
        }
        else
        {
            return 0;
        }

    }

    public Dictionary<itemTypes, int> getInventory
    {
        get { return inventory; }
    }

    public int getAmountOfSelectedProduct(itemTypes type) { if (inventory.ContainsKey(type)) return inventory[type]; return 0; }
    public bool canTakeMore()
    {
        if (currentStorage < maxStorage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void setCurrentStorage(int amount) { currentStorage += amount; }
    public int getCurrentStorage() { return currentStorage; }
    public int getMaxStorage() { return maxStorage; }
}
