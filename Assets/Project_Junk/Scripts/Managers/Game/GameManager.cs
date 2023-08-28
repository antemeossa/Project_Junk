using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum currentModeType
{
    PlayMode,
    BuildMode,
    UI_Mode,
    SalvageMode,
    ConnectionMode
        
};

public class GameManager : MonoBehaviour
{
    
    public Utils Utils;
    public RecipeManager recipeManager;
    public PlayerController playerController;
    public UI_Manager UI_M;
    public ProductionManager productionManager;
    public EconomyManager economyManager;
    public DroneManager droneManager;
    public FactionManager factionManager;
    public WreckageManager wreckageManager;
    public GameObject mothership;
    

    public currentModeType currentMode;
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (SaveGameManager.currentSaveData.isSaved)
        {
            GetComponent<SaveGameMono>().loadOperationsBuildings();
            productionManager.setAllBuildingsInWorld(mothership.transform.GetChild(1).GetChild(2).GetChild(3));
            GetComponent<SaveGameMono>().loadOperationsConnectors();
            GetComponent<SaveGameMono>().loadOperationsWreckages();
        }
        
    }
    #region load and save actions

    



    //Load and Save Events!
    public event Action onLoadEvent;
    public event Action onSaveEvent;
    public void loadGame()
    {
        if (onLoadEvent != null)
        {
            onLoadEvent();
        }
    }

    public void saveGame()
    {
        if (onSaveEvent != null)
        {
            onSaveEvent();
        }
    }
    #endregion

    public List<CraftRecipe> getAllRecipes { get { return recipeManager.craftRecipes; } }
    public GameObject getSelectedBuilding { get { return playerController.getSelectedBuilding; } }






}
