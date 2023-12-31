using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_RecipeListElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText, inputsText;

    private CraftRecipe recipe, previousRecipe;

    [SerializeField]
    private Image imag, rarityBorder;

    private AudioSource audioSrc;

    private PlayerController PC;

    private UI_SmallDetails smallDetails;



    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

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
        GameManager.Instance.soundManager.playBtnSound();

        PC = GameManager.Instance.playerController;
        if (PC.getSelectedBuilding.GetComponent<BuildingScript>().getSelectedRecipe == recipe)
        {
            
            PC.getSelectedBuilding.GetComponent<BuildingScript>().setSelectedRecipe(recipe);
            smallDetails = FindFirstObjectByType<UI_SmallDetails>();
            smallDetails.setSmallDetailsPanel(PC.getSelectedBuilding);
            transform.DOScale(1.4f, 0.2f);
            transform.DOScale(new Vector3(1, 1, 1), .2f);
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.getAllRecipes.Count; i++)
            {
                if (PC.getSelectedBuilding.GetComponent<InventoryScript>().getInventory.ContainsKey(GameManager.Instance.getAllRecipes[i].outputProduct.outputType))
                {
                    if (PC.getSelectedBuilding.GetComponent<InventoryScript>().getInventory[GameManager.Instance.getAllRecipes[i].outputProduct.outputType] > 0)
                    {
                        PC.getSelectedBuilding.GetComponent<InventoryScript>().transferItem(
                            GameManager.Instance.getAllRecipes[i].outputProduct.outputType,
                            PC.getSelectedBuilding.GetComponent<InventoryScript>().getInventory[GameManager.Instance.getAllRecipes[i].outputProduct.outputType],
                            GameManager.Instance.mothership.GetComponent<InventoryScript>());
                    }
                }
                
            }
            
            PC.getSelectedBuilding.GetComponent<BuildingScript>().setSelectedRecipe(recipe);
            smallDetails = FindFirstObjectByType<UI_SmallDetails>();
            smallDetails.setSmallDetailsPanel(PC.getSelectedBuilding);
            transform.DOScale(1.4f, 0.2f);
            audioSrc.Play();
            transform.DOScale(new Vector3(1, 1, 1), .2f);
        }
        
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

    
    public void OnPointerEnter(PointerEventData eventData)
    {
       
        transform.DOScale(1.1f, .5f);
        audioSrc.Play();

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, .5f);
    }

    public CraftRecipe getListElementRecipe { get { return recipe; }}

        
}
