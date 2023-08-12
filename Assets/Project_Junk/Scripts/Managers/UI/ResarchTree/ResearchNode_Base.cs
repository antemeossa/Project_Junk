using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class ResearchNode_Base : MonoBehaviour
{
    public string researchName, researchDescription;
    public int researchCost;
    public float researchTime, researchPerc;
    public bool isUnlockable, isUnlocked = false;
    public Sprite researchImg;
    public int researchOrder, researchLvl;

    


    
    public void unlockNode()
    {
        if(GameManager.Instance.EconomyManager.currentMoney >= researchCost)
        {
            StartCoroutine(researchTimeCounter());
        }
    }

    IEnumerator researchTimeCounter()
    {
        float currenT = 0;
        GetComponent<ResearchNode_UI>().getResearchSlider.SetActive(true);        
        while(currenT < researchTime)
        {
            currenT += Time.deltaTime;
            calculateResearchPerc(currenT);
            GetComponent<ResearchNode_UI>().updateResearchPercUI();
            yield return null;
        }
        GetComponent<ResearchNode_UI>().getResearchSlider.SetActive(false);
        GetComponent<ResearchNode_UI>().unlockMaterial();
        isUnlocked = true;
    }

    private float calculateResearchPerc(float currentT)
    {
        researchPerc = Mathf.Clamp01(currentT / researchTime);
        return researchPerc;
    }

    public void setUIDetails()
    {

    }
}
