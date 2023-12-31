using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ImprovedConnectorScript : MonoBehaviour
{
    private BuildSystemManager BS_M;

    [SerializeField]
    private GameObject connectorPointer, connectorPointerTmp, connectorPrefab, connectorParent, connectorParentTmp;
    [SerializeField]
    private float spacing;

    private Vector3 startPos, endPos, mousePosition;
    private GameObject startBuilding, endBuilding;
    private int connectorCount = 0;

    private bool connectorStarted, connectorFinished;

    [SerializeField]
    private List<GameObject> spawnedNodesTemp = new List<GameObject>();
    [SerializeField]
    private List<GameObject> spawnedNodesPerm = new List<GameObject>();
    private List<List<GameObject>> spawnedLines = new List<List<GameObject>>();
    [SerializeField]
    private List<Vector3> tempPositions = new List<Vector3>();

    private int lineRotation = 0;

    private void OnEnable()
    {
        BS_M = GetComponent<BuildSystemManager>();
        connectorPointerTmp = Instantiate(connectorPointer, new Vector3(0, 105, 0), Quaternion.identity);
        startPos = new Vector3(0,105, 0);
        mousePosition = connectorPointerTmp.GetComponent<PlaceableObject>().getMouseWorldPosition();
        connectorStarted = false;
        connectorFinished = false;        
        turnIndex = 0;

    }

    private void OnDisable()
    {
        clearTempNodes();
        BS_M.cancelConnector();
        Destroy(connectorPointerTmp.gameObject);
    }
    private void Start()
    {
    }

    private void Update()
    {
        connectorPointerTmp.GetComponent<PlaceableObject>().connectorMovement(connectorPointerTmp, startPos, checkMovementAxis(), startPos.y);
        mousePosition = connectorPointerTmp.GetComponent<PlaceableObject>().getMouseWorldPosition();

        if (connectorStarted && !connectorFinished)
        {

            endPos = new Vector3(connectorPointerTmp.transform.position.x, startPos.y, connectorPointerTmp.transform.position.z);
            setNodesPositions(spacing);
        }

        InputActions();
        
    }

    private void InputActions()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!connectorStarted && connectorPointerTmp.GetComponent<PlaceableObject>().getHoveredBuilding != null && connectorPointerTmp.GetComponent<PlaceableObject>().getHoveredBuilding.GetComponent<BuildingScript>().canConnectOut())
            {
                
                startPos = connectorPointerTmp.GetComponent<PlaceableObject>().getHoveredBuilding.GetComponent<BuildingScript>().getOuttakeConnectorTransform.transform.position;
                
                startBuilding = connectorPointerTmp.GetComponent<PlaceableObject>().getHoveredBuilding;
                connectorStarted = true;
                connectorFinished = false;
                connectorParentTmp = Instantiate(connectorParent);

            }
            else if (connectorStarted && !connectorFinished)
            {
                startPos = new Vector3(connectorPointerTmp.transform.position.x, startPos.y, connectorPointerTmp.transform.position.z);
                if (connectorPointerTmp.GetComponent<PlaceableObject>().getHoveredBuilding != null && connectorPointerTmp.GetComponent<PlaceableObject>().getHoveredBuilding.GetComponent<BuildingScript>().canConnectIn(startBuilding.GetComponent<BuildingScript>().getBuildingType))
                {

                    lineMousePos = mousePosition;
                    connectorPointerTmp.GetComponent<PlaceableObject>().getHoveredBuilding.GetComponent<BuildingScript>().addIntakeBuilding(startBuilding);
                    endBuilding = connectorPointerTmp.GetComponent<PlaceableObject>().getHoveredBuilding;
                    startBuilding.GetComponent<BuildingScript>().addOuttakeBuilding(endBuilding);
                    connectorParentTmp.GetComponent<TransferItemsScript>().setFacilities(startBuilding, endBuilding);
                    startBuilding.GetComponent<BuildingScript>().connectedConnector = connectorParentTmp;
                    endBuilding.GetComponent<BuildingScript>().connectedConnector = connectorParentTmp;
                    connectorParentTmp.transform.name = "Connector_" + connectorCount;
                    connectorParentTmp.transform.SetParent(BS_M.getCurrentFactory.transform.parent.GetComponent<FactoryScript>().getAllConnectorsTransform, true);
                    connectorFinished = true;
                    connectorStarted = false;
                    connectorCount++;
                    spawnLine();
                    makeTurns(connectorParentTmp, turnIndex);
                    clearTempNodes();
                    spawnedNodesPerm.Clear();
                    gameObject.GetComponent<ImprovedConnectorScript>().enabled = false;
                    GameManager.Instance.currentMode = currentModeType.BuildMode;
                    connectorParentTmp.GetComponent<ConnectorPlacementDropDown>().startPlacement();


                }
                else
                {

                    lineMousePos = mousePosition;
                    spawnLine();
                    makeTurns(connectorParentTmp, turnIndex);



                }

            }

        }

        if (Input.GetMouseButtonDown(1))
        {
            destroyPermNodes();
            gameObject.GetComponent<ImprovedConnectorScript>().enabled = false;
        }
    }
    private void clearTempNodes()
    {

        if (!connectorFinished)
        {
            for (int i = 0; i < spawnedNodesTemp.Count; i++)
            {
                Destroy(spawnedNodesTemp[i]);
            }
            spawnedNodesTemp.Clear();
            /*
            for (int i = 0; i < spawnedNodesPerm.Count; i++)
            {
                Destroy(spawnedNodesPerm[i]);
            }
            spawnedNodesPerm.Clear();*/
        }
        else if (connectorFinished)
        {
            for (int i = 0; i < spawnedNodesTemp.Count; i++)
            {
                Destroy(spawnedNodesTemp[i]);
            }
            spawnedNodesTemp.Clear();
        }
    }

    private void destroyPermNodes()
    {

        for (int i = 0; i < spawnedNodesPerm.Count; i++)
        {
            if (spawnedNodesPerm[i] != null)
            {
                Destroy(spawnedNodesPerm[i]);


            }
        }
        spawnedNodesPerm.Clear();


    }


    private int checkMovementAxis()
    {

        Vector3 dist = mousePosition - startPos;


        if (connectorStarted)
        {
            if (Mathf.Abs(dist.x) >= Mathf.Abs(dist.z))
            {
                return 1;
            }
            else
            {
                return 2;
            }



        }
        else
        {
            return 0;
        }
    }



    List<GameObject> tmpLine = new List<GameObject>();
    private Vector3 directionToObject, previousDirectionToObject;
    Vector3 lineMousePos;
    private void spawnLine()
    {
        
        tmpLine.Clear();
        previousDirectionToObject = directionToObject;
        for (int i = 0; i < tempPositions.Count; i++)
        {
            GameObject obj = Instantiate(connectorPointerTmp.GetComponent<PlaceableObject>().getObjectToBuild, tempPositions[i], Quaternion.identity);
            tmpLine.Add(obj);
            if (lineRotation == 0)
            {
                obj.transform.rotation = Quaternion.identity;
            }
            else if (lineRotation == 90)
            {
                obj.transform.Rotate(new Vector3(0, 90, 0));
            }
            obj.transform.SetParent(connectorParentTmp.transform, true);
            spawnedNodesPerm.Add(obj);
            
        }
        
        directionToObject = tmpLine[tmpLine.Count - 1].transform.position - tmpLine[0].transform.position;
        startPos = spawnedNodesPerm[spawnedNodesPerm.Count - 1].transform.position;
        
    }

    private int turnIndex = 0;
    private void makeTurns(GameObject obj, int index)
    {
        if (directionToObject.x < 0 && previousDirectionToObject.z < 0)
        {
            obj.transform.GetChild(turnIndex).GetComponent<improvedConnectorNodeScript>().switchToMerger();
            obj.transform.GetChild(turnIndex).Rotate(0, 180, 0);
        }
        else if (directionToObject.x < 0 && previousDirectionToObject.z > 0)
        {
            obj.transform.GetChild(turnIndex).GetComponent<improvedConnectorNodeScript>().switchToMerger();
            obj.transform.GetChild(turnIndex).Rotate(0, 90, 0);
        }
        else if (directionToObject.x > 0 && previousDirectionToObject.z < 0)
        {
            obj.transform.GetChild(turnIndex).GetComponent<improvedConnectorNodeScript>().switchToMerger();
            obj.transform.GetChild(turnIndex).Rotate(0, -90, 0);
        }
        else if (directionToObject.x > 0 && previousDirectionToObject.z > 0)
        {
            obj.transform.GetChild(turnIndex).GetComponent<improvedConnectorNodeScript>().switchToMerger();
            obj.transform.GetChild(turnIndex).Rotate(0, 0, 0);
        }
        if (directionToObject.z < 0 && previousDirectionToObject.x < 0)
        {
            obj.transform.GetChild(turnIndex).GetComponent<improvedConnectorNodeScript>().switchToMerger();
        }
        else if (directionToObject.z < 0 && previousDirectionToObject.x > 0)
        {
            obj.transform.GetChild(turnIndex).GetComponent<improvedConnectorNodeScript>().switchToMerger();
            obj.transform.GetChild(turnIndex).Rotate(0, 90, 0); //1
        }
        else if (directionToObject.z > 0 && previousDirectionToObject.x < 0)
        {
            obj.transform.GetChild(turnIndex).GetComponent<improvedConnectorNodeScript>().switchToMerger();
            obj.transform.GetChild(turnIndex).Rotate(0, -90, 0);
        }
        else if (directionToObject.z > 0 && previousDirectionToObject.x > 0)
        {
            obj.transform.GetChild(turnIndex).GetComponent<improvedConnectorNodeScript>().switchToMerger();
            obj.transform.GetChild(turnIndex).Rotate(0, -180, 0);
        }


        turnIndex = obj.transform.childCount - 1;
    }


   
    private void setNodesPositions(float space)
    {
        float dist = Vector3.Distance(connectorPointerTmp.transform.position, startPos);
        int nodeCount = Mathf.CeilToInt(dist / connectorPointerTmp.transform.localScale.sqrMagnitude) / 2;

        Vector3 nextNodePos = startPos;
        Vector3 vec = (endPos - startPos).normalized * 6;

        if (spawnedNodesTemp.Count != nodeCount && spawnedNodesTemp.Count == 0)
        {
            for (int i = 0; i < nodeCount; i++)
            {
                GameObject obj = Instantiate(connectorPointerTmp.GetComponent<PlaceableObject>().getObjectToBuild, nextNodePos, Quaternion.identity);

                if (checkMovementAxis() == 2)
                {
                    if (directionToObject.z < 0)
                    {
                        obj.transform.Rotate(new Vector3(0, 90, 0));
                        lineRotation = 90;
                    }
                    else if (directionToObject.z > 0)
                    {
                        obj.transform.Rotate(new Vector3(0, 270, 0));
                        lineRotation = 270;
                    }

                }
                else if (checkMovementAxis() == 1)
                {
                    if (directionToObject.x < 0)
                    {
                        obj.transform.rotation = Quaternion.identity;
                        lineRotation = 0;
                    }
                    else if (directionToObject.x > 0)
                    {
                        obj.transform.Rotate(new Vector3(0, 180, 0));
                        lineRotation = 180;
                    }


                }
                spawnedNodesTemp.Add(obj);
                tempPositions.Add(obj.transform.position);

                nextNodePos += vec;
            }

        }
        else if (spawnedNodesTemp.Count != 0 && spawnedNodesTemp.Count != 0)
        {
            for (int i = 0; i < spawnedNodesTemp.Count; i++)
            {
                Destroy(spawnedNodesTemp[i]);
            }
            tempPositions.Clear();
            spawnedNodesTemp.Clear();
            for (int i = 0; i < nodeCount; i++)
            {
                nextNodePos += vec;
                GameObject obj = Instantiate(connectorPointerTmp.GetComponent<PlaceableObject>().getObjectToBuild, nextNodePos, Quaternion.identity);


               
                
                if (checkMovementAxis() == 2)
                {
                    obj.transform.rotation = Quaternion.identity;
                    lineRotation = 0;
                }
                else if (checkMovementAxis() == 1)
                {
                    obj.transform.Rotate(new Vector3(0, 90, 0));
                    lineRotation = 90;
                }
                spawnedNodesTemp.Add(obj);
                tempPositions.Add(obj.transform.position);
            }
        }
    }
}
