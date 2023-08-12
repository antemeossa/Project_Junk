using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class connectorParentScript : MonoBehaviour
{
    private List<int> turnPointsList = new List<int>(); 
    private List<GameObject> allChildren = new List<GameObject>();




    public void setTurns()
    {
        for(int i = 0; i < turnPointsList.Count; i++)
        {
           
        }
    }
    public void setTurnPointsList(List<int> list) { turnPointsList = list; }

}
