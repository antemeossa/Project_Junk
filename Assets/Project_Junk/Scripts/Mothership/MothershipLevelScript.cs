using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothershipLevelScript : MonoBehaviour
{
    public int motherShipLevel, maxLevel = 5;
    public int[] droneRewards = new int[7];
    public int[] storageRewards = new int[7];
    public string[] techlevels = new string[7];
    public int[] nextLevelCost = new int[7];
    public GameObject mainBuildingLv1, mainBuildingLv2, mainBuildingLv3, circle1, circle2;

    private void Start()
    {
        motherShipLevel = 1;
        GameManager.Instance.recipeManager.unlockRecipes(motherShipLevel);
        GameManager.Instance.wreckageManager.unlockCrashSites(1);
        GameManager.Instance.droneManager.maxDroneAmount = droneRewards[motherShipLevel];
        GameManager.Instance.UI_M.updateDroneText();
        if (motherShipLevel == 2)
        {
            mainBuildingLv1.SetActive(true);
            circle1.SetActive(true);
        }
        else if (motherShipLevel == 3)
        {
            circle1.SetActive(true);
            mainBuildingLv1.SetActive(false);
            mainBuildingLv2.SetActive(true);            
        }
        else if (motherShipLevel == 4)
        {
            circle1.SetActive(true);
            mainBuildingLv1.SetActive(false);
            mainBuildingLv2.SetActive(true);
            circle2.SetActive(true);
            GameManager.Instance.wreckageManager.unlockCrashSites(3);
        }
        else if (motherShipLevel == 5)
        {
            circle1.SetActive(true);
            mainBuildingLv1.SetActive(false);
            mainBuildingLv2.SetActive(true);
            circle2.SetActive(true);
            mainBuildingLv2.SetActive(false);
            mainBuildingLv3.SetActive(true);
            
        }
    }
    public void checkLevels()
    {
        GameManager.Instance.recipeManager.unlockRecipes(motherShipLevel);

        if (motherShipLevel == 2)
        {
            circle1.SetActive(true);
        }
        else if (motherShipLevel == 3)
        {

            mainBuildingLv1.SetActive(false);
            mainBuildingLv2.SetActive(true);
            GameManager.Instance.wreckageManager.unlockCrashSites(2);
        }
        else if (motherShipLevel == 4)
        {
            circle2.SetActive(true);
        }
        else if (motherShipLevel == 5)
        {
            mainBuildingLv2.SetActive(false);
            mainBuildingLv3.SetActive(true);
            GameManager.Instance.wreckageManager.unlockCrashSites(3);
        }
    }

    public void levelUp()
    {
        if(motherShipLevel + 1 <= maxLevel && GameManager.Instance.economyManager.currentMoney >= nextLevelCost[motherShipLevel])
        {
            GameManager.Instance.soundManager.playLevelUpSound();
            motherShipLevel = Mathf.Clamp(motherShipLevel + 1, 1, 5);
            if (motherShipLevel == 2)
            {
                circle1.SetActive(true);
            }
            else if (motherShipLevel == 3)
            {

                mainBuildingLv1.SetActive(false);
                mainBuildingLv2.SetActive(true);
            }
            else if (motherShipLevel == 4)
            {
                circle2.SetActive(true);
            }
            else if (motherShipLevel == 5)
            {
                mainBuildingLv2.SetActive(false);
                mainBuildingLv3.SetActive(true);
            }
            GameManager.Instance.economyManager.removeMoney(nextLevelCost[motherShipLevel ]);
            GameManager.Instance.droneManager.maxDroneAmount = droneRewards[motherShipLevel];
            GameManager.Instance.UI_M.updateDroneText();
            //GameManager.Instance.mothership.GetComponent<InventoryScript>().setMaxStorage(storageRewards[motherShipLevel]);
            checkLevels();
        }
    }
        
    
}
