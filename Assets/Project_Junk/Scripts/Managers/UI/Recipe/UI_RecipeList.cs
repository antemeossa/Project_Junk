using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_RecipeList : MonoBehaviour
{
    private UI_Manager UI_M;
    public Transform listContainer; // Reference to the container object that holds the list
    public GameObject listItemPrefab; // Prefab for the list item

    [SerializeField]
    private List<GameObject> spawnedElements = new List<GameObject>(); // List to store the items
    private RecipeManager RM;

    private int buildingIndex;
    private void Awake()
    {
        UI_M = FindFirstObjectByType<UI_Manager>();
        RM = FindFirstObjectByType<RecipeManager>();
    }

    
    private void getClickedBuildingType(GameObject obj)
    {


        if (obj.GetComponent<BuildingScript>() != null)
        {
            if (obj.GetComponent<BuildingScript>().getBuildingType.ToString() == "Smelter")
            {
                buildingIndex = 1;
            }
            else if (obj.GetComponent<BuildingScript>().getBuildingType.ToString() == "Refiner")
            {
                buildingIndex = 2;
            }
            else if (obj.GetComponent<BuildingScript>().getBuildingType.ToString() == "Assembler")
            {
                buildingIndex = 3;
            }
            else
            {
                buildingIndex = 0;
            }


        }
        else
        {
            buildingIndex = 0;
        }
    }

    private void createListElements(List<CraftRecipe> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            spawnedElements.Add(Instantiate(listItemPrefab, listContainer, false));
        }

        for (int i = 0; i < spawnedElements.Count; i++)
        {
            spawnedElements[i].transform.SetParent(listContainer.transform, false);
            spawnedElements[i].GetComponent<UI_RecipeListElement>().SetListItem(RM.FormatEnumWithSpaces(list[i].outputProduct.outputType), list[i].inputRequirements, list[i], list[i].img);
        }
    }

    public void setList(GameObject clickedObject)
    {
        destroyRecipeList();
        getClickedBuildingType(clickedObject);
        if(buildingIndex == 1)
        {
            createListElements(RM.smelterRecipesList); 

        }
        else if (buildingIndex == 2)
        {
            createListElements(RM.refinerRecipesList); 

        }
        else if(buildingIndex == 3)
        {
            createListElements(RM.assemblerRecipesList); 

        }
        else
        {
            return;
        }
       
    }

    public void destroyRecipeList()
    {
        if(spawnedElements.Count > 0)
        {
            for (int i = 0; i < spawnedElements.Count; i++)
            {
                spawnedElements[i].transform.SetParent(null, false);
                Destroy(spawnedElements[i].gameObject);
            }
        }

        spawnedElements.Clear();
        
    }

   
}
