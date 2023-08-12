using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_RecipeListElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText, inputsText;

    private CraftRecipe recipe;

    [SerializeField]
    private Image imag, rarityBorder;


    private PlayerController PC;

    private UI_SmallDetails smallDetails;

    


    
    public void SetListItem(string name, List<InputRequirement> inputs, CraftRecipe recipeToSelect, Sprite img)
    {
        nameText.text = name;
        if (inputs.Count == 1)
        {
            inputsText.text = "x" + inputs[0].requiredAmount + " " + enumToString(inputs[0].inputType);
        }
        else if (inputs.Count == 2)
        {
            inputsText.text = "x" + inputs[0].requiredAmount + " " + enumToString(inputs[0].inputType) + "\n" + "x" + inputs[1].requiredAmount + " " + enumToString(inputs[1].inputType);
        }
        else if (inputs.Count == 3)
        {
            inputsText.text = "x" + inputs[0].requiredAmount + " " + enumToString(inputs[0].inputType) + "\n" + "x" + inputs[1].requiredAmount + " " + enumToString(inputs[1].inputType) + "\n" + "x" + inputs[2].requiredAmount + " " + enumToString(inputs[2].inputType);
        }
        else
        {
            nameText.text = "null"; 
            inputsText.text = "null";
        }

        setRarityBorder(recipeToSelect);
        recipe = recipeToSelect;

        imag.sprite = img;
    }

    private void setRarityBorder(CraftRecipe obj)
    {
        if(obj.craftRarity.Equals(Rarity.Primitive)) 
        {
            rarityBorder.color = Color.gray;
        }
        else if (obj.craftRarity.Equals(Rarity.Standard))
        {
            rarityBorder.color = Color.yellow;
        }
        else if (obj.craftRarity.Equals(Rarity.Advanced))
        {
            rarityBorder.color = Color.green;
        }
        else if (obj.craftRarity.Equals(Rarity.Prototype))
        {
            rarityBorder.color = Color.cyan;
        }
        else if (obj.craftRarity.Equals(Rarity.CuttingEdge))
        {
            rarityBorder.color = Color.magenta;
        }
    }
    public void selectProductionOnClick()
    {
        PC = FindFirstObjectByType<PlayerController>();
        PC.getSelectedBuilding.GetComponent<BuildingScript>().setSelectedRecipe(recipe);
        smallDetails = FindFirstObjectByType<UI_SmallDetails>();
        smallDetails.setSmallDetailsPanel(PC.getSelectedBuilding);
    }


    private string enumToString<T>(T enumValue)
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
