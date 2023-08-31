using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConnectorSaveData : MonoBehaviour
{
    public ConnectorData connectorData = new ConnectorData();

    

    public void setNodesPosRot()
    {
        connectorData.connectorNodeCount = transform.childCount;

        if(GetComponent<TransferItemsScript>().getInFacility != null)
        {
            connectorData.inFacility = GetComponent<TransferItemsScript>().getInFacilityID();

        }

        if (GetComponent<TransferItemsScript>().getOutFacility != null)
        {
            connectorData.outFacility = GetComponent<TransferItemsScript>().getOutFacilityID();

        }

        for (int i = 0; i < transform.childCount; i++)
        {
            connectorData.nodePositions.Add(transform.GetChild(i).position);
            connectorData.nodeRotations.Add(transform.GetChild(i).rotation);
            connectorData.isTurnList.Add(transform.GetChild(i).GetComponent<improvedConnectorNodeScript>().isTurn);
        }
    }
}

[System.Serializable]
public struct ConnectorData
{
    //Building Details
    public int ID;
    public int connectorNodeCount;
    public int inFacility;
    public int outFacility;
    public List<Vector3> nodePositions;
    public List<Quaternion> nodeRotations;
    public List<bool> isTurnList;

}

