using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionScript : MonoBehaviour
{
    private bool canProduce = false;

    
    public void produce(InventoryScript inventory, CraftRecipe recipe)
    {
        if(inventory != null && recipe != null)
        {
            for(int i = 0; i < recipe.inputRequirements.Count; i++)
            {
                if (inventory.getAmountOfSelectedProduct(recipe.inputRequirements[i].inputType) >= recipe.inputRequirements[i].requiredAmount)
                {
                    canProduce = true;
                }
                else
                {
                    canProduce = false; break;
                }
            }

            if(canProduce)
            {
                inventory.addProducedItem(recipe.outputProduct.outputType, recipe.outputProduct.outputAmount);

                for(int i = 0;i < recipe.inputRequirements.Count; i++)
                {
                    inventory.removeItem(recipe.inputRequirements[i].inputType, recipe.inputRequirements[i].requiredAmount);
                }
            }
        }
    }
}
