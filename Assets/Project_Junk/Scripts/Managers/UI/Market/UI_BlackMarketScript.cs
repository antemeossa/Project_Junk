using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_BlackMarketScript : MonoBehaviour
{
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private GameObject blackMarketItemPrefab;
    [SerializeField] private GameObject buyModeBtn, sellModeBtn;
    [SerializeField] private VerticalLayoutGroup layoutGroup;
    [SerializeField] public List<UI_BlackMarketListElement> blackMarketList = new List<UI_BlackMarketListElement>();
    private List<CraftRecipe> allRecipes = new List<CraftRecipe>();



    private void Start()
    {
        allRecipes = GameManager.Instance.getAllRecipes;

        createContractElemets();
        setDealDetailsForList();
        UpdateUIPositions();
    }

    private void OnEnable()
    {

        UpdateUIPositions();
    }

    private void OnDisable()
    {
        UpdateUIPositions();
    }


    public void refreshList()
    {

        for (int i = 0; i < blackMarketList.Count; i++)
        {
            blackMarketList[i].resetSliders();
        }
    }
    private void createContractElemets()
    {


        for (int i = 0; i < allRecipes.Count; i++)
        {
            GameObject obj = Instantiate(blackMarketItemPrefab, contentPanel.transform, false);
            blackMarketList.Add(obj.GetComponent<UI_BlackMarketListElement>());
        }
    }

    private void setDealDetailsForList()
    {
        for (int i = 0; i < blackMarketList.Count; i++)
        {
            if (blackMarketList[i] != null && !blackMarketList[i].isActive)
            {


                CraftRecipe r = allRecipes[i];
                
                if (r != null)
                {
                    blackMarketList[i].setBlackMarketItemDetails(r,
                    r.img, r.craftRarity, r.outputProduct.outputType, calculatePrice(r));
                }
                blackMarketList[i].gameObject.SetActive(true);
            }
            else
            {
                blackMarketList.Remove(blackMarketList[i]);
            }


        }
    }



    public void buyModeBtnOnClick()
    {
        GameManager.Instance.soundManager.playBtnSound();

        for (int i = 0; i < contentPanel.transform.childCount; i++)
        {
            contentPanel.transform.GetChild(i).gameObject.SetActive(true);

        }

        for (int i = 0; i < blackMarketList.Count; i++)
        {
            blackMarketList[i].switchToBuyMode();
        }
    }

    public void sellModeBtnOnClick()
    {
        GameManager.Instance.soundManager.playBtnSound();

        for (int i = 0; i < contentPanel.transform.childCount; i++)
        {
            contentPanel.transform.GetChild(i).gameObject.SetActive(true);
            if (!GameManager.Instance.mothership.GetComponent<InventoryScript>().getInventory.ContainsKey(contentPanel.transform.GetChild(i).GetComponent<UI_BlackMarketListElement>().itemType))
            {
                contentPanel.transform.GetChild(i).gameObject.SetActive(false);

            }
            else
            {
                contentPanel.transform.GetChild(i).GetComponent<UI_BlackMarketListElement>().switchToSellMode();
            }
        }

    }

    public void updateList()
    {
       
    }
    private void sortContractsByActive()
    {

        blackMarketList.Sort((a, b) =>
        {
            bool boolA = a.GetComponent<UI_BlackMarketListElement>().isActive;
            bool boolB = b.GetComponent<UI_BlackMarketListElement>().isActive;

            int costA = a.GetComponent<UI_BlackMarketListElement>().itemCost;
            int costB = b.GetComponent<UI_BlackMarketListElement>().itemCost;

            if (boolA != boolB)
                return boolA ? -1 : 1;

            return costA.CompareTo(costB);
        });

        // Reorder elements in the layout

        foreach (UI_BlackMarketListElement element in blackMarketList)
        {
            element.transform.SetParent(layoutGroup.transform);
        }

        // Force layout update
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.transform as RectTransform);

    }

    private void UpdateUIPositions()
    {
        //sortContractsByMoney();
        sortContractsByActive();
        for (int i = 0; i < blackMarketList.Count; i++)
        {
            blackMarketList[i].transform.SetSiblingIndex(i); // Set the index to rearrange items
        }

        layoutGroup.enabled = false; // Temporarily disable layout group to force update
        layoutGroup.enabled = true;
    }
    private int calculatePrice(CraftRecipe recipe)
    {
        int baseCost = recipe.price;
        int multiplier = 1;
        int totalCost;

        

        switch (recipe.building)
        {
            case productionBuilding.Smelter:
                multiplier = 2;
                break;
            case productionBuilding.Refiner:
                multiplier = 8;
                break;
            case productionBuilding.Assembler:
                multiplier = 64;
                break;
            default:
                break;
        }

        totalCost = baseCost * multiplier;

        return totalCost;
    }
   
}
