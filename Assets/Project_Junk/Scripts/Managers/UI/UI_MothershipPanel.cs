using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_MothershipPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nextLevelCostText, currentLvText, droneRewardsText, storageRewardsText, techLevelText, maxLevelText, nextLevelText;
    [SerializeField]
    private MothershipLevelScript mothershipLv;

    private void Start()
    {
        updateLevelRewards(mothershipLv.motherShipLevel, mothershipLv.droneRewards[mothershipLv.motherShipLevel + 1],
            mothershipLv.storageRewards[mothershipLv.motherShipLevel + 1], mothershipLv.nextLevelCost[mothershipLv.motherShipLevel + 1],
            mothershipLv.techlevels[mothershipLv.motherShipLevel + 1]);
    }
    public void levelUpBtnOnClick()
    {
        GameManager.Instance.soundManager.playBtnSound();

        mothershipLv.levelUp();
        if(mothershipLv.motherShipLevel <= 5)
        {
            updateLevelRewards(mothershipLv.motherShipLevel, mothershipLv.droneRewards[mothershipLv.motherShipLevel + 1],
            mothershipLv.storageRewards[mothershipLv.motherShipLevel + 1], mothershipLv.nextLevelCost[mothershipLv.motherShipLevel + 1],
            mothershipLv.techlevels[mothershipLv.motherShipLevel + 1]);
        }
        else {
            updateLevelRewards(mothershipLv.motherShipLevel,0 ,0,0,"");
        }
        
    }

    public void updateLevelRewards(int x, int drone, int storage, int cost, string tech)
    {
        if (x + 1 == 6)
        {
            currentLvText.text = "LEVEL " + x;
            nextLevelCostText.gameObject.SetActive(false);
            droneRewardsText.gameObject.SetActive(false);
            storageRewardsText.gameObject.SetActive(false);
            techLevelText.gameObject.SetActive(false);
            nextLevelText.gameObject.SetActive(false);
            maxLevelText.gameObject.SetActive(true);
        }
        else
        {
            
            
                currentLvText.text = "LEVEL " + x;
                nextLevelCostText.text = "COST: " + cost;
                droneRewardsText.text = "+" + drone + " DRONES";
                storageRewardsText.text = "+" + storage + "STORAGE SPACE";
                techLevelText.text = "NEXT TECH LEVEL: " + tech;
            
            

        }
    }

}
