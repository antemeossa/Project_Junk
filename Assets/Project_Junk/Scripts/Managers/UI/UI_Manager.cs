using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject buildPanel, smallDetailsPanel, recipeListPanel, storagePanel,
        deleteNotificationPanel, buildButtonPrefab, wreckPanel, contractsPanel, blackmarketPanel;

    public TextMeshProUGUI currentMoneyText;

    public BuildSystemManager BS_M;
    public GameManager GameManager;

    private PlayerController playerController;

    private bool focusedOnBuilding;

    private BuildingScript buildingScr;
    private StorageScript storageScr;

    public List<GameObject> buildButtonsList = new List<GameObject>();

    public int cycle;
    private void Start()
    {
        deactivateAllPanels();
        playerController = FindFirstObjectByType<PlayerController>();
        setBuildPanel(buildButtonPrefab);

    }

    public void deactivateAllPanels()
    {
        smallDetailsPanel.SetActive(false);
        storagePanel.SetActive(false);
        recipeListPanel.SetActive(false);
        buildPanel.SetActive(false);
        wreckPanel.SetActive(false);
        contractsPanel.SetActive(false);
        blackmarketPanel.SetActive(false);



    }
    public void activateSmallDetailsPanel()
    {


        recipeListPanel.GetComponent<UI_RecipeList>().setList(playerController.getSelectedBuilding);
        smallDetailsPanel.GetComponent<UI_SmallDetails>().setSmallDetailsPanel(playerController.getSelectedBuilding);
        smallDetailsPanel.SetActive(true);
        focusedOnBuilding = true;

    }

    public void updateSmallDetailsPanel()
    {
        if (playerController.getSelectedBuilding != null && smallDetailsPanel.activeInHierarchy)
        {

            smallDetailsPanel.GetComponent<UI_SmallDetails>().setSmallDetailsPanel(playerController.getSelectedBuilding);

        }

    }

    public void deactivateSmallDetailsPanel()
    {
        smallDetailsPanel.SetActive(false);
        focusedOnBuilding = false;

    }

    public void recipeListBtnOnClick()
    {
        buildingScr = playerController.getSelectedBuilding.GetComponent<BuildingScript>();
        if (!recipeListPanel.activeSelf)
        {
            recipeListPanel.GetComponent<UI_RecipeList>().setList(playerController.getSelectedBuilding);
            smallDetailsPanel.GetComponent<UI_SmallDetails>().setSmallDetailsPanel(playerController.getSelectedBuilding);
            recipeListPanel.SetActive(true);
        }
        else
        {
            recipeListPanel.GetComponent<UI_RecipeList>().setList(playerController.getSelectedBuilding);
            smallDetailsPanel.GetComponent<UI_SmallDetails>().setSmallDetailsPanel(playerController.getSelectedBuilding);

            recipeListPanel.SetActive(false);
        }

    }

    public void switchBuildPanel()
    {
        if (buildPanel.activeSelf)
        {
            buildPanel.SetActive(false);
            GameManager.currentMode = currentModeType.PlayMode;


        }
        else
        {
            buildPanel.SetActive(true);
            deactivateSmallDetailsPanel();
            GameManager.currentMode = currentModeType.BuildMode;

        }
    }

    public void switchConnectionMode()
    {
        if (GameManager.currentMode == currentModeType.BuildMode)
        {
            switchBuildPanel();
            BS_M.cancelBuildAction();
        }
        BS_M.buildConnector();
    }

    public void activateContractsPanel()
    {
        contractsPanel.SetActive(true);
    }

    public void deactivateContractsPanel()
    {
        contractsPanel.SetActive(false);
    }

    public void updateContractsPanel()
    {
        contractsPanel.GetComponent<UI_ContractPanelScript>().refreshList();
    }

    public void updateContractsCycle()
    {
        contractsPanel.GetComponent<UI_ContractPanelScript>().updateCycleCounter(20 - (cycle % 20));
    }

    public void activateBlackMarketPanel()
    {
        blackmarketPanel.SetActive(true);
    }

    public void deactivateBlackMarketPanel()
    {
        blackmarketPanel.SetActive(false);
    }

    public void setBuildPanel(GameObject btn)
    {
        GameObject obj;


        for (int i = 0; i < BS_M.buildableObjects.Count; i++)
        {
            obj = Instantiate(btn);
            obj.transform.SetParent(buildPanel.transform, false);
            buildButtonsList.Add(obj);

        }

        for (int i = 0; i < buildButtonsList.Count; i++)
        {

            buildButtonsList[i].name = Utils.enumToString(BS_M.buildableObjects[i].buildingType) + "Button";
            buildButtonsList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Utils.enumToString(BS_M.buildableObjects[i].buildingType);

        }

        for (int i = 0; i < BS_M.buildableObjects.Count; i++)
        {
            GameObject bp = BS_M.buildableObjects[i].buildingBlueprint;
            buildButtonsList[i].GetComponent<Button>().onClick.AddListener(() => { BS_M.buildObjectOnClick(bp); });
        }
    }

    public void activateStoragePanel()
    {

        storagePanel.SetActive(true);
        storagePanel.GetComponent<UI_StorageDetails>().setSelectedStorage(playerController.getSelectedBuilding.transform.parent.transform.parent.transform.parent.gameObject);
        storagePanel.GetComponent<UI_StorageDetails>().createGrid();
        storagePanel.GetComponent<UI_StorageDetails>().updateGrid();
        focusedOnBuilding = true;
    }

    public void deactivateStoragePanel()
    {
        storagePanel.SetActive(false);
    }

    public void activateWreckPanel()
    {
        wreckPanel.SetActive(true);
        wreckPanel.GetComponent<UI_WreckDetails>().setSelectedWreckage(playerController.getSelectedBuilding);
        wreckPanel.GetComponent<UI_WreckDetails>().createGrid();
        wreckPanel.GetComponent<UI_WreckDetails>().updateGrid();
        focusedOnBuilding = true;
    }

    public void deactivateWreckPanel()
    {
        wreckPanel.SetActive(false);
    }


    #region deleteNotification
    public void activateDeleteNotification()
    {
        deleteNotificationPanel.SetActive(true);
    }

    public void confirmDelete()
    {
        Destroy(playerController.getSelectedBuilding.gameObject);
        deleteNotificationPanel.SetActive(false);
    }

    public void declineDelete()
    {
        deleteNotificationPanel.SetActive(false);
    }

    #endregion


    public bool getIsFocusedOnBuilding { get { return focusedOnBuilding; } set { focusedOnBuilding = value; } }




}
