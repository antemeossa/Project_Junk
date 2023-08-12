using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchManager : MonoBehaviour
{
    public bool scrapDroneUnlocked, transportDroneUnlocked, fighterDroneUnlocked;
    
    public List<ResearchNode_Base> allResearchNodes = new List<ResearchNode_Base>();


    private void Start()
    {
        
    }

    private void setAllResearchNodes()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            allResearchNodes.Add(transform.GetChild(i).GetComponent<ResearchNode_Base>());
        }
    }
    public void researchNodeOnClick(ResearchNode_Base node)
    {
        if (node.GetComponent<ResearchNode_Base>().isUnlockable)
        {
            node.unlockNode();
            Debug.Log("okay");
        }
        else
        {
            GameManager.Instance.Utils.showErrorMessage("You can not unlock this research yet!" + "\n" + "Research parent nodes first.");

        }
    }


    
}
