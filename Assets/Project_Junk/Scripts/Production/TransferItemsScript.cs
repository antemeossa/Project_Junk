using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransferItemsScript : MonoBehaviour
{
    [SerializeField]
    private GameObject inFacility, outFacility;
    public int transferRate;
    private bool facilitiesSet;
    public CraftRecipe selectedRecipe;

    private void Start()
    {
        InvokeRepeating("process", 1, 1);
        facilitiesSet = false;
    }

    private void process()
    {
        if (inFacility != null)
        {
            transferItems();
            GameManager.Instance.UI_M.updateSmallDetailsPanel();
        }
    }
    public void setFacilities(GameObject outFac, GameObject inFac)
    {
        outFacility = outFac;
        inFacility = inFac;
        SetSelectedRecipe(inFac.GetComponent<BuildingScript>());
        facilitiesSet = true;
    }
    public void transferItems()
    {

        SetSelectedRecipe(inFacility.GetComponent<BuildingScript>());
        if (selectedRecipe != null && facilitiesSet && inFacility.GetComponent<InventoryScript>().canTakeMore())
        {
            for (int i = 0; i < selectedRecipe.inputRequirements.Count; i++)
            {
                if (outFacility.GetComponent<InventoryScript>().getInventory.ContainsKey(selectedRecipe.inputRequirements[i].inputType))
                {
                    if (outFacility.GetComponent<BuildingScript>().getBuildingType == buildingTypesEnum.Storage)
                    {
                        outFacility.GetComponent<StorageScript>().setContainersActive();
                    }
                    outFacility.GetComponent<InventoryScript>().transferItem(selectedRecipe.inputRequirements[i].inputType,
                        selectedRecipe.inputRequirements[i].requiredAmount,
                        inFacility.GetComponent<InventoryScript>());

                }


            }
        }
    }

    private void SetSelectedRecipe(BuildingScript facility)
    {
        selectedRecipe = facility.getSelectedRecipe;

    }
}
