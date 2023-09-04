using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_ContractPanelScript : MonoBehaviour
{

    [SerializeField] private GameObject contentPanel;
    [SerializeField] private GameObject contractPrefab;
    [SerializeField] private VerticalLayoutGroup layoutGroup;
    [SerializeField] private TextMeshProUGUI cycleText;
    private List<Faction> factions = new List<Faction>();
    [SerializeField] public List<UI_ContractElemetnScript> contractsList = new List<UI_ContractElemetnScript>();
    private List<CraftRecipe> allRecipes = new List<CraftRecipe>();



    private void Start()
    {
        factions = GameManager.Instance.factionManager.Factions;
        allRecipes = GameManager.Instance.getAllRecipes;

        createContractElemets();
        setContractDetailsForList();
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

    public void updateCycleCounter(int counter)
    {
        cycleText.text = "CYCLES TO REFRESH: " + counter;
    }
    public void refreshList()
    {

        setContractDetailsForList();
        UpdateUIPositions();
    }
    private void createContractElemets()
    {


        for (int i = 0; i < 20; i++)
        {
            if (contractsList.Count < 20)
            {
                GameObject obj = Instantiate(contractPrefab, contentPanel.transform, false);
                contractsList.Add(obj.GetComponent<UI_ContractElemetnScript>());
            }

        }


    }

    private void setContractDetailsForList()
    {
        createContractElemets();

        for (int i = 0; i < contractsList.Count; i++)
        {
            if (contractsList[i] != null && !contractsList[i].isActive)
            {

                int rnd = Random.Range(0, factions.Count);
                Faction f = factions[rnd];
                CraftRecipe r = getRandomUnlockedRecipe();
                int rndAmount = Random.Range(10, 201);
                if (r != null)
                {
                    contractsList[i].setContractElementDetails(r, f.factionLogo, f.factionTitle, f.factionRelations,
                    r.img, r.craftRarity, r.outputProduct.outputType, rndAmount, calculateReward(rndAmount, r));
                }
                contractsList[i].gameObject.SetActive(true);
            }
            else
            {
                contractsList.Remove(contractsList[i]);
            }


        }
    }


    private void sortContractsByActive()
    {

        contractsList.Sort((a, b) =>
        {
            bool boolA = a.GetComponent<UI_ContractElemetnScript>().isActive;
            bool boolB = b.GetComponent<UI_ContractElemetnScript>().isActive;

            int costA = a.GetComponent<UI_ContractElemetnScript>().rewardMoney;
            int costB = b.GetComponent<UI_ContractElemetnScript>().rewardMoney;

            if (boolA != boolB)
                return boolA ? -1 : 1;

            return costA.CompareTo(costB);
        });

        // Reorder elements in the layout

        foreach (UI_ContractElemetnScript element in contractsList)
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
        for (int i = 0; i < contractsList.Count; i++)
        {
            contractsList[i].transform.SetSiblingIndex(i); // Set the index to rearrange items
        }

        layoutGroup.enabled = false; // Temporarily disable layout group to force update
        layoutGroup.enabled = true;
    }
    private int calculateReward(int x, CraftRecipe recipe)
    {


        return x * recipe.price;
    }
    private CraftRecipe getRandomUnlockedRecipe()
    {

        List<CraftRecipe> tmpRecipes = new List<CraftRecipe>();
        for (int i = 0; i < allRecipes.Count; i++)
        {
            if (allRecipes[i].isUnlocked)
            {
                tmpRecipes.Add(allRecipes[i]);
            }

        }



        return tmpRecipes[Random.Range(0, tmpRecipes.Count)];
    }
}
