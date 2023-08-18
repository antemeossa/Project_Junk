using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorPlacementDropDown : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> connectorsList = new List<GameObject>();

    [SerializeField]
    private float t, height;

    public void setPlaceableConnectorList() { startPlacement(); }

    private void Update()
    {
    }
    int index = 0;


    public void startPlacement()
    {
        
        connectorsList.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<improvedConnectorNodeScript>() != null)
            {
                connectorsList.Add(transform.GetChild(i).transform.gameObject);
            }


        }

        for (int i = index; i < connectorsList.Count; i++)
        {
            for (int j = 0; j < connectorsList[i].transform.childCount; j++)
            {
                connectorsList[i].transform.GetChild(j).GetComponent<MeshRenderer>().enabled = false;
            }
        }

        if (connectorsList.Count != 0)
        {
            StartCoroutine("placeConnectors");

        }

        index = connectorsList.Count - 1;

    }
    private IEnumerator placeConnectors()
    {
        float currentT = 0f;



        for (int i = index; i < connectorsList.Count; i++)
        {
            if (connectorsList[i] != null && !connectorsList[i].GetComponent<improvedConnectorNodeScript>().getHasPlaced)
            {
                for (int j = 0; j < connectorsList[i].transform.childCount; j++)
                {
                    connectorsList[i].transform.GetChild(j).GetComponent<MeshRenderer>().enabled = true;

                }
                //connectorsList[i].GetComponent<MeshRenderer>().enabled = true;
                connectorsList[i].transform.position = new Vector3(connectorsList[i].transform.position.x, connectorsList[i].transform.position.y + height, connectorsList[i].transform.position.z);
                connectorsList[i].transform.DOMoveY(connectorsList[i].transform.position.y - height, t);
                transform.DOComplete();
                connectorsList[i].GetComponent<improvedConnectorNodeScript>().setHasPlaced(true);
                while (currentT < t)
                {
                    currentT += Time.deltaTime;
                    yield return null;
                }
                connectorsList[i].transform.DOComplete();
                currentT = 0f;
            }

        }

    }

    

    
}
