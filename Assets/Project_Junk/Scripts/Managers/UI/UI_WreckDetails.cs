using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_WreckDetails : MonoBehaviour
{
    [SerializeField]
    private GameObject storageItemPrefab, container;
    [SerializeField] private DroneSpawnManager droneSpawnManager;
    [SerializeField] private Slider droneAmountSlider;
    [SerializeField] private GameObject droneAmountText, salvageBtnText;

    public GameObject currentDronesParent;

    [SerializeField]
    private GameObject selectedWreckage;

    private List<CraftRecipe> allItemsList = new List<CraftRecipe>();
    [SerializeField]
    private List<GameObject> spawnedElementsList = new List<GameObject>();





    private void Start()
    {
        allItemsList = GameManager.Instance.getAllRecipes;
        droneAmountSlider.maxValue = GameManager.Instance.droneManager.maxDroneAmount;

    }

    private void Update()
    {
        updateGrid();
    }
    public void updateDroneAmountSlider()
    {
        droneAmountText.GetComponent<TextMeshProUGUI>().text = "DRONE AMOUNT: " + droneAmountSlider.value;
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
                spawnedElementsList[i].GetComponent<UI_StorageListElement>().setStorageListElement(Utils.enumToString(allItemsList[i].outputProduct.outputType),
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
            if (spawnedElementsList[i].GetComponent<UI_StorageListElement>().getItemAmount() == 0)
            {
                spawnedElementsList[i].SetActive(false);
            }
            else
            {
                spawnedElementsList[i].SetActive(true);
            }
        }
    }
    public void setSelectedWreckage(GameObject obj)
    {
        selectedWreckage = obj;
        droneSpawnManager.targetWreckage = selectedWreckage;
        droneAmountSlider.value = 0;
        if(obj.GetComponent<WreckAreaScript>().beingSalvaged)
        {
            droneAmountSlider.interactable = false;
        }
        else
        {
            droneAmountSlider.interactable = true;
        }
    }
    private void setAllItemsList()
    {
        allItemsList = GameManager.Instance.getAllRecipes;
    }

    public void salvageButtonOnClick()
    {
        if (!selectedWreckage.GetComponent<WreckAreaScript>().beingSalvaged)
        {
            droneAmountSlider.interactable = false;
            GameManager.Instance.droneManager.activeDroneAmount += (int)droneAmountSlider.value;
            droneAmountSlider.maxValue = GameManager.Instance.droneManager.maxDroneAmount - GameManager.Instance.droneManager.activeDroneAmount;
            droneSpawnManager.sendDrones((int)droneAmountSlider.value);
            selectedWreckage.GetComponent<WreckAreaScript>().beingSalvaged = true;
            salvageBtnText.GetComponent<TextMeshProUGUI>().text = "SALVAGING!";
            
        }
        else
        {
            GameManager.Instance.droneManager.activeDroneAmount -= (int)droneAmountSlider.value;
            droneAmountSlider.maxValue = GameManager.Instance.droneManager.maxDroneAmount - GameManager.Instance.droneManager.activeDroneAmount;
            droneSpawnManager.cancelSalvage(selectedWreckage);
            selectedWreckage.GetComponent<WreckAreaScript>().beingSalvaged = false;
            salvageBtnText.GetComponent<TextMeshProUGUI>().text = "SALVAGE";
            
        }

    }

}
