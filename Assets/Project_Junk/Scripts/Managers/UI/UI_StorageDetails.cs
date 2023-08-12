using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StorageDetails : MonoBehaviour
{
    [SerializeField]
    private GameObject storageItemPrefab, container, hoverPanel;

    private GameObject selectedStorage;

    private List<CraftRecipe> allItemsList = new List<CraftRecipe>();
    private List<GameObject> spawnedElementsList = new List<GameObject>();

    private bool hasCreated;


    private void Start()
    {
        allItemsList = GameManager.Instance.getAllRecipes;
        hasCreated = false;

    }

    public GameObject getHoverPanel
    {
        get { return hoverPanel; }
    }
    public void setHoverPanelName(string text)
    {
        hoverPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
    }


    public void createGrid()
    {
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


            hasCreated = true;
        }
        updateGrid();




    }

    public void updateGrid()
    {
        for (int i = 0; i < spawnedElementsList.Count; i++)
        {
            spawnedElementsList[i].GetComponent<UI_StorageListElement>().setItemAmount(selectedStorage.GetComponent<InventoryScript>().getAmountOfSelectedProduct(allItemsList[i].outputProduct.outputType));
        }
    }
    public void setSelectedStorage(GameObject obj) { selectedStorage = obj; }
    private void setAllItemsList()
    {
        allItemsList = GameManager.Instance.getAllRecipes;
    }
}
