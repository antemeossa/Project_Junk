using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.UI;
using System.Linq;


#region craftItemDetails
[System.Serializable]
public class CraftRecipe
{
    public Sprite img;
    public Rarity craftRarity;                       // Rarity of the craft
    public productionBuilding building;                    // Type of building to craft
    public List<InputRequirement> inputRequirements; // List of input requirements for the craft
    public OutputProduct outputProduct;              // The resulting output product    
    public int productionTime;    
    public bool isUnlocked;                          //Check if is unlocked
}

[System.Serializable]
public class InputRequirement
{
    public itemTypes inputType;      // Type of input required
    public int requiredAmount;       // Amount of input required
}

[System.Serializable]
public class OutputProduct
{
    public itemTypes outputType;    // Type of output produced
    public int outputAmount;         // Amount of output produced
}


public enum productionBuilding
{
    
    Smelter,
    Refiner,
    Assembler,
    Storage
}
public enum Rarity
{
    Primitive,
    Standard,
    Advanced,
    Prototype,
    CuttingEdge
}

public enum itemTypes
{
    Junk,
    ScrapCopper,
    ScrapIron,
    ScrapGold,
    ScrapTitanium,
    ScrapPlatinum,
    CopperIngot,
    IronIngot,
    GoldIngot,
    TitaniumIngot,
    PlatinumIngot,
    CopperWire,
    CopperCoil,
    CopperConduit,
    IronPlate,
    ReinforcedIron,
    IronFramework,
    GoldPlatedCircuitBoard,
    GoldHeatSink,
    GoldSensor,
    TitaniumPlate,

}

public enum OutputType
{
    CopperIngot,
    IronIngot,
    GoldIngot,
    TitaniumIngot,
    PlatinumIngot,
    CopperWire,
    CopperCoil,
    CopperConduit,
    IronPlate,
    ReinforcedIron,
    IronFramework,
    GoldPlatedCircuitBoard,
    GoldHeatSink,
    GoldSensor,
}

#endregion


public class RecipeManager : MonoBehaviour
{
    
    public List<CraftRecipe> craftRecipes;   // List of craft recipes
    public List<CraftRecipe> smelterRecipesList;
    public List<CraftRecipe> refinerRecipesList;
    public List<CraftRecipe> assemblerRecipesList;



    private void Start()
    {
        sortRecipeLists();
        
    }
    public void sortRecipeLists()
    {
        for (int i = 0; i < craftRecipes.Count; i++)
        {
            if (craftRecipes[i].building == productionBuilding.Smelter)
            {
                smelterRecipesList.Add(craftRecipes[i]);
            }else if (craftRecipes[i].building == productionBuilding.Refiner)
            {
                refinerRecipesList.Add(craftRecipes[i]);
            }else if (craftRecipes[i].building == productionBuilding.Assembler)
            {
                assemblerRecipesList.Add(craftRecipes[i]);
            }
        }
    }

    public string FormatEnumWithSpaces<T>(T enumValue)
    {
        string enumString = enumValue.ToString();
        string formattedString = string.Empty;

        // Add spaces between words by checking for uppercase letters
        for (int i = 0; i < enumString.Length; i++)
        {
            if (i > 0 && char.IsUpper(enumString[i]))
            {
                formattedString += " ";
            }

            formattedString += enumString[i];
        }

        return formattedString;
    }
}
