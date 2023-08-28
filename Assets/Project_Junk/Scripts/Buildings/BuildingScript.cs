using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{

    #region variables


    [SerializeField]
    private buildingTypesEnum buildingType;
    [SerializeField]
    private List<buildingTypesEnum> connectableIntakeTypes;
    [SerializeField]
    private int powerConsumption, intakeCount, outputCount;
    [SerializeField]
    private List<GameObject> connectedOutputs = new List<GameObject>();
    [SerializeField]
    private VFXPlayer vfxplayer;

    private int intakeOccupied = 0, outputOccupied = 0;

    [SerializeField]
    private GameObject factory;
    private CraftRecipe selectedRecipe;

    [SerializeField]
    private List<GameObject> connectedIntakes = new List<GameObject>();

    public int uniqueID;
    public bool placedDown = false;


    #endregion

    private void Awake()
    {
        
    }

    private void Start()
    {
        factory = transform.parent.gameObject;

        if(uniqueID == 0)
        {
            int timestamp = (int)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalSeconds;
            int randomNum = Random.Range(0, 1000); // You can adjust the range as needed
            uniqueID = timestamp * 1000 + randomNum;
        }
        
        
        //Data Operations

        //if there is no id



    }


    private void selfDestruct()
    {
        Destroy(gameObject);
    }

    public void addIntakeBuilding(GameObject obj)
    {
        connectedIntakes.Add(obj);
        intakeOccupied++;
    }

    public void addOuttakeBuilding(GameObject obj)
    {
        connectedOutputs.Add(obj);
        outputOccupied++;
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

        }
        else
        {
            connect = false;
        }

        return connect;
    }

    public bool canConnectOut()
    {
        if (outputOccupied == outputCount)
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
        if (selectedRecipe != null)
        {
            return Utils.enumToString(selectedRecipe.outputProduct.outputType) + "\n" + " x" + selectedRecipe.outputProduct.outputAmount;
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
    public int getOuttakeCount { get { return outputCount; } }

    public void setIntakeOccupied(int value) { intakeOccupied += value; }
    public int getIntakeOccupied { get { return intakeOccupied; } }
    public void setOuttakeOccupied(int value) { outputOccupied += value; }
    public void setSelectedRecipe(CraftRecipe recipeToSelect) {

        selectedRecipe = recipeToSelect;
        GetComponent<BuildingSaveData>().bldData.currentRecipe = Utils.enumToString(recipeToSelect.outputProduct.outputType);

            }
    public void setBuildingType(buildingTypesEnum type) { buildingType = type; }

    public void playPlacementVFX() { vfxplayer.PlayPlacementVFX(); }
    public GameObject getIntakeConnectorTransform { get { return connectedIntakes[getIntakeOccupied]; } }
    public GameObject getOuttakeConnectorTransform { get { return connectedOutputs[outputOccupied]; } }

    public GameObject getBuildingFactory { get { return factory; } }
    #endregion

}
