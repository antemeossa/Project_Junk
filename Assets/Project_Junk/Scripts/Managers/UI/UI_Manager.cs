using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject InGameOverlay, MainMenuOverlay;
    [SerializeField]
    private GameObject buildPanel, smallDetailsPanel, recipeListPanel, storagePanel,
        deleteNotificationPanel, buildButtonPrefab, wreckPanel, contractsPanel, blackmarketPanel, mothershipPanel, settingsPanel,
        mainMenuPanel;

    [SerializeField]
    private GameObject notEnoughNotification, saveGameNotification, exitGameNotification;

    [SerializeField]
    private GameObject[] tutorialPanels;

    public TextMeshProUGUI currentMoneyText, droneAmountText;

    public BuildSystemManager BS_M;
    public GameManager gameManager;

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
        droneAmountText.text = GameManager.Instance.droneManager.maxDroneAmount - GameManager.Instance.droneManager.activeDroneAmount + "/" + GameManager.Instance.droneManager.maxDroneAmount;



    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (buildPanel.activeInHierarchy)
            {
                tutorialPanels[6].SetActive(true);
                showBuildModeHelp();
            }
            else if (smallDetailsPanel.activeInHierarchy)
            {
                tutorialPanels[6].SetActive(true);
                showSmallDetailsModeHelp();
            }
            else if (wreckPanel.activeInHierarchy)
            {
                tutorialPanels[6].SetActive(true);
                showCrashSiteHelp();
            }
            else if (blackmarketPanel.activeInHierarchy)
            {
                tutorialPanels[6].SetActive(true);
                showBlackmarketHelp();
            }
            else if (contractsPanel.activeInHierarchy)
            {
                tutorialPanels[6].SetActive(true);
                showContractsHelp();
            }
            else
            {
                tutorialPanels[6].SetActive(true);
                showBasicHelp();
            }

        }
        else if (Input.GetKeyUp(KeyCode.F1))
        {
            tutorialPanels[6].SetActive(false);
            hideBasicHelp();
            hideBlackmarketHelp();
            hideBuildModeHelp();
            hideContractsHelp();
            hideCrashSiteHelp();
        }
    }

    public void updateDroneText()
    {
        droneAmountText.text = GameManager.Instance.droneManager.maxDroneAmount - GameManager.Instance.droneManager.activeDroneAmount + "/" + GameManager.Instance.droneManager.maxDroneAmount;

    }
    public void newGameBtnOnClick()
    {
        GameManager.Instance.soundManager.playBtnSound();
        SceneManager.LoadScene("MainLevel");
    }


    public void switchMenuOverlay()
    {
        if (InGameOverlay.activeInHierarchy)
        {
            InGameOverlay.SetActive(false);
            settingsPanel.SetActive(false);
            
            MainMenuOverlay.SetActive(true);
            mainMenuPanel.SetActive(true);

        }
        else
        {
            InGameOverlay.SetActive(true);
            MainMenuOverlay.SetActive(false);
        }
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
        mothershipPanel.SetActive(false);



    }

    #region tutorial

    public void showBasicHelp()
    {
        tutorialPanels[0].SetActive(true);
    }

    public void hideBasicHelp()
    {
        tutorialPanels[0].SetActive(false);
    }

    public void showBuildModeHelp()
    {
        tutorialPanels[1].SetActive(true);
    }

    public void hideBuildModeHelp()
    {
        tutorialPanels[1].SetActive(false);
    }

    public void showSmallDetailsModeHelp()
    {
        tutorialPanels[2].SetActive(true);
    }
    public void hideSmallDetailsModeHelp()
    {
        tutorialPanels[2].SetActive(false);
    }

    public void showCrashSiteHelp()
    {
        tutorialPanels[3].SetActive(true);
    }

    public void hideCrashSiteHelp()
    {
        tutorialPanels[3].SetActive(false);
    }

    public void showBlackmarketHelp()
    {
        tutorialPanels[4].SetActive(true);
    }

    public void hideBlackmarketHelp()
    {
        tutorialPanels[4].SetActive(false);
    }

    public void showContractsHelp()
    {
        tutorialPanels[5].SetActive(true);
    }

    public void hideContractsHelp()
    {
        tutorialPanels[5].SetActive(false);
    }


    #endregion


    #region panelActions

    public void activateSaveGameNotification()
    {
        InGameOverlay.SetActive(true);
        MainMenuOverlay.SetActive(false);
        saveGameNotification.SetActive(true);
    }

    public void deactivateSaveGameNotification()
    {
        InGameOverlay.SetActive(false);
        MainMenuOverlay.SetActive(true);
        saveGameNotification.SetActive(false);
    }

    public void activateExitGameNotification()
    {
        InGameOverlay.SetActive(true);
        MainMenuOverlay.SetActive(false);
        exitGameNotification.SetActive(true);
    }

    public void deactivateExitGameNotification()
    {
        InGameOverlay.SetActive(false);
        MainMenuOverlay.SetActive(true);
        exitGameNotification.SetActive(false);
    }
    public void activateMothershipPanel()
    {
        GameManager.Instance.soundManager.playBtnSound();
        mothershipPanel.SetActive(true);
    }

    public void deactivateMothershipPanel()
    {
        mothershipPanel.SetActive(false);
    }


    public void activateSmallDetailsPanel()
    {

        GameManager.Instance.soundManager.playBtnSound();

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
        GameManager.Instance.soundManager.playBtnSound();

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
        if (buildPanel.activeInHierarchy)
        {
            buildPanel.SetActive(false);
            gameManager.currentMode = currentModeType.PlayMode;


        }
        else
        {
            GameManager.Instance.soundManager.playBtnSound();

            buildPanel.SetActive(true);
            deactivateSmallDetailsPanel();
            gameManager.currentMode = currentModeType.BuildMode;

        }
    }

    public void switchConnectionMode()
    {
        GameManager.Instance.soundManager.playBtnSound();


        if (gameManager.currentMode == currentModeType.BuildMode)
        {
            switchBuildPanel();
            BS_M.cancelBuildAction();
        }
        BS_M.buildConnector();
    }

    public void activateContractsPanel()
    {
        GameManager.Instance.soundManager.playBtnSound();

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
        contractsPanel.GetComponent<UI_ContractPanelScript>().updateCycleCounter(100 - (cycle % 100));
    }

    public void activateBlackMarketPanel()
    {
        GameManager.Instance.soundManager.playBtnSound();

        blackmarketPanel.SetActive(true);
    }

    public void updateBlackMarketPanel()
    {
        blackmarketPanel.GetComponent<UI_BlackMarketScript>().refreshList();
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
            
            buildButtonsList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Utils.enumToString(BS_M.buildableObjects[i].buildingType) + "\nCost: " + BS_M.buildableObjects[i].buildCost;

        }

        for (int i = 0; i < BS_M.buildableObjects.Count; i++)
        {
            GameObject bp = BS_M.buildableObjects[i].buildingBlueprint;
            buildButtonsList[i].GetComponent<Button>().onClick.AddListener(() => { BS_M.buildObjectOnClick(bp); });
        }
    }

    public void activateStoragePanel()
    {
        GameManager.Instance.soundManager.playBtnSound();

        storagePanel.SetActive(true);
        if(playerController.getSelectedBuilding.GetComponent<MainBuldingScript>() != null)
        {
            storagePanel.GetComponent<UI_StorageDetails>().setSelectedStorage(GameManager.Instance.mothership);

        }
        else
        {
            storagePanel.GetComponent<UI_StorageDetails>().setSelectedStorage(playerController.getSelectedBuilding);

        }
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
        GameManager.Instance.soundManager.playBtnSound();

        wreckPanel.SetActive(true);
        wreckPanel.GetComponent<UI_WreckDetails>().setSelectedWreckage(playerController.getSelectedBuilding);
        wreckPanel.GetComponent<UI_WreckDetails>().createGrid();
        wreckPanel.GetComponent<UI_WreckDetails>().updateGrid();
        focusedOnBuilding = true;
    }

    public void activateSettingsPanel()
    {
        if (MainMenuOverlay.activeInHierarchy)
        {
            mainMenuPanel.SetActive(false);
        }
       
        settingsPanel.SetActive(true);
    }

    public void deactivateSettingsPanel()
    {
        settingsPanel.SetActive(false);
        if (MainMenuOverlay.activeInHierarchy)
        {
            mainMenuPanel.SetActive(true);
        }
    }

    public void exitGameOnClick()
    {
        Application.Quit();
    }
    public void deactivateWreckPanel()
    {
        wreckPanel.SetActive(false);
    }
    #endregion

    #region deleteNotification
    public void activateDeleteNotification()
    {
        deleteNotificationPanel.SetActive(true);
    }

    public void confirmDelete()
    {
        if(playerController.getSelectedBuilding.GetComponent<BuildingScript>() != null)
        {
            GameManager.Instance.productionManager.allBuildings.Remove(playerController.getSelectedBuilding);
        }
        Destroy(playerController.getSelectedBuilding.gameObject);
        deleteNotificationPanel.SetActive(false);
    }

    public void declineDelete()
    {
        deleteNotificationPanel.SetActive(false);
    }

    public void notEnoughMonetNotification()
    {
        GameManager.Instance.soundManager.playDeclineSound();

        notEnoughNotification.SetActive(true);
    }

    #endregion


    public bool getIsFocusedOnBuilding { get { return focusedOnBuilding; } set { focusedOnBuilding = value; } }




}
