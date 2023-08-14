using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{

    #region variables


    [SerializeField]
    private buildingTypesEnum buildingType;
    [SerializeField]
    private List<buildingTypesEnum> connectableIntakeTypes;
    [SerializeField]
    private int powerConsumption, intakeCount, outtakeCount;    
    [SerializeField]
    private GameObject connectedOuttake;
    [SerializeField]
    private VFXPlayer vfxplayer;
    
    private int intakeOccupied = 0, outtakeOccupied = 0;

    [SerializeField]
    private GameObject factory;
    private CraftRecipe selectedRecipe;

    [SerializeField]
    private List<GameObject> connectedIntakes = new List<GameObject>();
    
    

    #endregion

    private void Start()
    {
        factory = transform.parent.gameObject;
       

    }

    
    public void addIntakeBuilding(GameObject obj)
    {
        connectedIntakes.Add(obj);
        intakeOccupied++;
    }

    public void addOuttakeBuilding(GameObject obj)
    {
        connectedOuttake = obj;
        outtakeOccupied++;
    }
   public bool canConnectIn(buildingTypesEnum connectingType)
    {
        bool connect = false;
        if (intakeOccupied + 1 <= intakeCount)
        {
            for (int i = 0; i < connectableIntakeTypes.Count; i++)
            {
                if (connectingType.Equals(connectableIntakeTypes[i]))
                {
                    connect = true;
                    break;
                }
                else
                {
                    connect = false;
                }
            }
            
        }else 
        {
            connect = false;
        }

        return connect;
    }

    public bool canConnectOut()
    {
        if(outtakeOccupied == outtakeCount)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    



    #region getters and setter
   
    public buildingTypesEnum getBuildingType { get { return buildingType; } }
    public string getCurrentProduction()
    {
        if(selectedRecipe != null)
        {
            return GameManager.Instance.Utils.enumToString(selectedRecipe.outputProduct.outputType) + "\n" + " x" + selectedRecipe.outputProduct.outputAmount;
        }
        else
        {
            return "None";
        }
    }

    public int getPowerConsumption { get { return powerConsumption; } }

    public int getOutputAmount { get { return selectedRecipe.outputProduct.outputAmount; } }

    public CraftRecipe getSelectedRecipe { get { return selectedRecipe; } }

    public int getIntakeCount { get { return intakeCount; } }
    public int getOuttakeCount { get { return outtakeCount; } }

    public void setIntakeOccupied(int value) { intakeOccupied += value; }
    public int getIntakeOccupied { get { return intakeOccupied; } }
    public void setOuttakeOccupied(int value) { outtakeOccupied += value; }
    public void setSelectedRecipe(CraftRecipe recipeToSelect) { selectedRecipe = recipeToSelect; }
    public void setBuildingType(buildingTypesEnum type) { buildingType = type; }

    public void playPlacementVFX() { vfxplayer.PlayPlacementVFX(); }
    public GameObject getIntakeConnectorTransform { get { return connectedIntakes[getIntakeOccupied]; } }
    public GameObject getOuttakeConnectorTransform { get { return connectedOuttake; } }

    public GameObject getBuildingFactory { get { return factory; } }
    #endregion

}
