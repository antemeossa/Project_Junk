using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SmallDetails : MonoBehaviour
{

    public TextMeshProUGUI buildingName;
    public TextMeshProUGUI currentProduction;
    public TextMeshProUGUI currentProductionStorageAmount;
    public Image itemImg, SDRarityBorder;
    public Sprite noneSprite;
    public Button recipeMenuBtn;
    public GameObject IndexParent;
    public GameObject indexPrefab;


    private List<GameObject> indexList = new List<GameObject>();


    private void Start()
    {
    }

    public void setSmallDetailsPanel(GameObject bld)
    {
        BuildingScript building = bld.GetComponent<BuildingScript>();
        buildingName.text = building.getBuildingType.ToString();
        currentProduction.text = building.getCurrentProduction();
        createConnectedBuildingIndex(building.getIntakeCount);
        setIndexColors(building);
        if (building.getSelectedRecipe != null)
        {
            currentProductionStorageAmount.text = bld.GetComponent<InventoryScript>().getItemCount(building.getSelectedRecipe.outputProduct.outputType) + "";
            setCurrentMaterials(building.getSelectedRecipe, building);
        }
        else
        {
            currentProductionStorageAmount.text = "";
        }

        setRarityBorder(building.getSelectedRecipe);
        if (building.getSelectedRecipe != null)
        {
            itemImg.sprite = building.getSelectedRecipe.img;
        }
        else
        {
            itemImg.sprite = noneSprite;
        }

        

    }

    private void setRarityBorder(CraftRecipe obj)
    {
        if (obj != null)
        {
            if (obj.craftRarity.Equals(Rarity.Primitive))
            {
                SDRarityBorder.color = Color.gray;
            }
            else if (obj.craftRarity.Equals(Rarity.Standard))
            {
                SDRarityBorder.color = Color.yellow;
            }
            else if (obj.craftRarity.Equals(Rarity.Advanced))
            {
                SDRarityBorder.color = Color.green;
            }
            else if (obj.craftRarity.Equals(Rarity.Prototype))
            {
                SDRarityBorder.color = Color.cyan;
            }
            else if (obj.craftRarity.Equals(Rarity.CuttingEdge))
            {
                SDRarityBorder.color = Color.magenta;
            }
        }
        else
        {
            SDRarityBorder.color = Color.black;
        }

    }
    private void setIndexColors(BuildingScript obj)
    {
        for (int i = 0; i < indexList.Count; i++)
        {
            indexList[i].GetComponent<Image>().color = Color.white;
        }
        for (int i = 0; i < obj.getIntakeOccupied; i++)
        {
            indexList[i].GetComponent<Image>().color = Color.green;
        }
    }
    private void createConnectedBuildingIndex(int x)
    {
        for (int i = 0; i < indexList.Count; i++)
        {
            Destroy(indexList[i]);
        }

        indexList.Clear();

        GameObject tmp;
        for (int i = 0; i < x; i++)
        {
            tmp = Instantiate(indexPrefab);
            tmp.transform.SetParent(IndexParent.transform, false);
            indexList.Add(tmp);
        }

    }

    [SerializeField]
    private GameObject currentCraftMaterial, currentCraftTransform;
    private List<CraftRecipe> currentCraftMaterialsList = new List<CraftRecipe>();
    private List<GameObject> spawnedRequirementsUIlist = new List<GameObject>();

    private void setCurrentMaterials(CraftRecipe recipe, BuildingScript building)
    {
        currentCraftMaterialsList.Clear();
        
        if(spawnedRequirementsUIlist.Count > 0 )
        {
            for (int i = 0; i < spawnedRequirementsUIlist.Count; i++)
            {
                Destroy(spawnedRequirementsUIlist[i]);
            }
        }

        for (int i = 0; i < GameManager.Instance.getAllRecipes.Count; i++)
        {
            for (int j = 0; j < recipe.inputRequirements.Count; j++)
            {
                if (GameManager.Instance.getAllRecipes[i].outputProduct.outputType.Equals(recipe.inputRequirements[j].inputType))
                {
                    currentCraftMaterialsList.Add(GameManager.Instance.getAllRecipes[i]);
                }
            }
        }

        for (int i = 0; i < currentCraftMaterialsList.Count; i++)
        {
            GameObject obj = Instantiate(currentCraftMaterial, currentCraftTransform.transform, false);
            spawnedRequirementsUIlist.Add(obj);
            
            obj.GetComponent<UI_StorageListElement>().setStorageListElement(GameManager.Instance.Utils.enumToString(currentCraftMaterialsList[i].outputProduct.outputType),
                building.GetComponent<InventoryScript>().getAmountOfSelectedProduct(currentCraftMaterialsList[i].outputProduct.outputType),
                currentCraftMaterialsList[i].img, currentCraftMaterialsList[i]);
        }
    }

}
