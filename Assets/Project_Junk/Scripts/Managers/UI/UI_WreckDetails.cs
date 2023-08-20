using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_WreckDetails : MonoBehaviour
{
    [SerializeField]
    private GameObject storageItemPrefab, container;

    private GameObject selectedWreckage;

    private List<CraftRecipe> allItemsList = new List<CraftRecipe>();
    [SerializeField]
    private List<GameObject> spawnedElementsList = new List<GameObject>();



    private void Start()
    {
        allItemsList = GameManager.Instance.getAllRecipes;

    }

    


    public void createGrid()
    {
        Debug.Log("In");
        setAllItemsList();
        if (spawnedElementsList.Count != allItemsList.Count)
        {
            GameObject obj;


            for (int i = 0; i < allItemsList.Count; i++)
            {
                obj = Instantiate(storageItemPrefab, container.transform, false);
                spawnedElementsList.Add(obj);
            }

            for (int i = 0; i < spawnedElementsList.Count; i++)
            {
                spawnedElementsList[i].GetComponent<UI_StorageListElement>().setStorageListElement(GameManager.Instance.Utils.enumToString(allItemsList[i].outputProduct.outputType),
                0,
                allItemsList[i].img,
                allItemsList[i]);
            }


        }
        updateGrid();




    }

    public void updateGrid()
    {
        for (int i = 0; i < spawnedElementsList.Count; i++)
        {
            spawnedElementsList[i].GetComponent<UI_StorageListElement>().setItemAmount(selectedWreckage.GetComponent<InventoryScript>().getAmountOfSelectedProduct(allItemsList[i].outputProduct.outputType));
        }
    }
    public void setSelectedWreckage(GameObject obj) { selectedWreckage = obj; }
    private void setAllItemsList()
    {
        allItemsList = GameManager.Instance.getAllRecipes;
    }
}
