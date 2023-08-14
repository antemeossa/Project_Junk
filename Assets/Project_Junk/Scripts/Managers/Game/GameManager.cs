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
    public RecipeManager RM;
    public PlayerController PC;
    public UI_Manager UI_M;
    public EconomyManager EconomyManager;
    public DroneManager DroneManager;

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

    

    public List<CraftRecipe> getAllRecipes { get { return RM.craftRecipes; } }
    public GameObject getSelectedBuilding { get { return PC.getSelectedBuilding; } }






}
