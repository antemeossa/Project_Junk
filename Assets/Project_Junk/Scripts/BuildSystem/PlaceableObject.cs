using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{


    [SerializeField]
    private buildingTypesEnum buildingType;
    [SerializeField]
    private GameObject objectToBuild;
    [SerializeField]
    private BuildSystemManager BS_M;
    [SerializeField]

    private int layerNumber = 3, layerMask;

    private Renderer rnd;
    private BoxCollider col;
    private GameObject hoveredObject;
    private bool canBuild, colliding, isOnFactory, isConnecting, outOfBorder;

    private Ray ray;

    private Vector3 connectionPoint = new Vector3();
    private float placementHeight;


    

    private void Start()
    {
        BS_M = FindFirstObjectByType<BuildSystemManager>();
        col = GetComponent<BoxCollider>();
        if (buildingType != buildingTypesEnum.Connector)
        {
            rnd = transform.GetChild(0).GetComponent<Renderer>();
        }
        else
        {
            rnd = transform.GetComponent<Renderer>();
        }

        rnd.material.color = Color.green;

        canBuild = true;
        layerMask = 1 << layerNumber;
        placementHeight = BS_M.placementHeight;



    }
    private void Update()
    {
        if (buildingType == buildingTypesEnum.Connector)
        {

        }
        else
        {
            normalMovement();
        }
        setColors();
        rotateObject();
        

    }


    public bool checkCanBuild()
    {
        if (buildingType.Equals(buildingTypesEnum.Connector))
        {
            if (isOnFactory && hoveredObject != null && !colliding)
            {
                canBuild = true;
                return canBuild;
            }
            else
            {
                canBuild = false;
                return canBuild;
            }
        }
        else
        {
            if (isOnFactory && !colliding && !outOfBorder)
            {
                canBuild = true;
                return canBuild;
            }
            else
            {
                canBuild = false;
                return canBuild;
            }
        }


    }
    #region movement

    public void connectorMovement(GameObject obj, Vector3 endpointPos, int x, float height)
    {
        if (x == 1)
        {
            Vector3 pos = getMouseWorldPosition();
            Vector3 snapPos;
            if (!isConnecting)
            {
                snapPos = new Vector3(setFloor(pos.x, BS_M.gridSizeX), height, endpointPos.z);
            }
            else
            {
                snapPos = connectionPoint;
            }

            obj.transform.position = snapPos;
        }
        else if (x == 2)
        {

            Vector3 pos = getMouseWorldPosition();
            Vector3 snapPos = new Vector3(endpointPos.x, height, setFloor(pos.z, BS_M.gridSizeY));
            obj.transform.position = snapPos;
        }
        else if (x == 0)
        {

            Vector3 pos = getMouseWorldPosition();
            Vector3 snapPos = new Vector3(setFloor(pos.x, BS_M.gridSizeX), height, setFloor(pos.z, BS_M.gridSizeY));
            obj.transform.position = snapPos;
        }

    }

    private void normalMovement()
    {
        Vector3 pos = getMouseWorldPosition();
        Vector3 snapPos = new Vector3(setFloor(pos.x, BS_M.gridSizeX), placementHeight, setFloor(pos.z, BS_M.gridSizeY));
        transform.position = snapPos;
    }
    public Vector3 getMouseWorldPosition()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 50000.0f, layerMask))
        {
            if (raycastHit.transform.gameObject.Equals(BS_M.getCurrentFactory))
            {
                isOnFactory = true;
                BS_M.setCurrentFactory(raycastHit.transform.gameObject);
                return raycastHit.point;
            }
            else
            {
                canBuild = false;
                return Vector3.zero;
            }
        }
        else
        {
            return Vector3.zero;
        }
    }

    public float setFloor(float value, float gridSize)
    {
        float flooredValue = 0;
        if (value % gridSize != 0)
        {
            flooredValue = gridSize * Mathf.Floor(value / gridSize);
            return flooredValue;
        }
        else
        {
            return 0f;
        }
    }

    private void rotateObject()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(0, 90, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(0, -90, 0);
        }
    }

    #endregion

    #region triggers

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Selectable"))
        {
            if(other.gameObject.GetComponent<BuildingScript>() != null || other.gameObject.GetComponent<StorageScript>() != null)
            {
                colliding = true;
            }
            
            hoveredObject = other.gameObject;

            connectionPoint = transform.position;
            if (other.GetComponent<BuildingScript>() != null)
            {
                if (other.GetComponent<BuildingScript>().getBuildingType.Equals(buildingType))
                {
                    isConnecting = true;
                }
            }

        }

        if (other.gameObject.CompareTag("Border"))
        {
            outOfBorder = true;

        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Selectable"))
        {
            colliding = false;
            if (other.GetComponent<BuildingScript>() != null)
            {
                if (other.GetComponent<BuildingScript>().getBuildingType.Equals(buildingType))
                {
                    isConnecting = false;
                }
            }
            if (hoveredObject != null)
            {
                // BS_M.resetHighlights();
                //hoveredObject.GetComponent<Renderer>().material.color = Color.white;
                hoveredObject = null;
            }
        }



        if (other.gameObject.CompareTag("Border"))
        {
            outOfBorder = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Selectable"))
        {
            colliding = true;
            hoveredObject = other.gameObject;
        }

        if (other.gameObject.CompareTag("Border"))
        {
            outOfBorder = true;
        }


    }

    #endregion

    private void setColors()
    {
        if (buildingType != buildingTypesEnum.Connector)
        {
            if (checkCanBuild())
            {
                rnd.material.color = Color.green;
            }
            else
            {
                rnd.material.color = Color.red;
            }
        }
        else
        {
            if (hoveredObject != null && hoveredObject.GetComponent<BuildingScript>() != null)
            {
                //hoveredObject.GetComponent<Renderer>().material.color = Color.yellow;
                //BS_M.highlighConnectableBuildings(hoveredObject);
                rnd.material.color = Color.green;
            }

        }

    }

    #region getters and setters

    public GameObject getObjectToBuild { get { return objectToBuild; } }
    public bool getCanBuild { get { return canBuild; } }

    public GameObject getHoveredBuilding { get { return hoveredObject; } }

    public bool getIsColliding { get { return colliding; } }

    public Vector3 getConnectionPoint { get { return connectionPoint; } }
    #endregion




























    /*
    #region detectConnectionObject
    
    private BuildingScript buildingToConnect;
    private Ray rayForward;
    private GameObject previousHitObject;

    public GameObject getPreviousObject
    {
        get { return previousHitObject; }
    }

    private void setColors()
    {
        if (canBuild && isOnFactory && !colliding)
        {
            rnd.material.color = Color.green;
        }
        else
        {
            rnd.material.color = Color.red;
        }
    }

    
    private void checkBuildingToConnectFront()
    {
        rayForward = new Ray(transform.position, transform.forward);
        int layerMask = ~(1 << gameObject.layer);

        if (buildingType.ToString() == "Smelter")
        {
            connectionRayLength = 1.5f;
        }
        else if (buildingType.ToString() == "Refiner")
        {
            connectionRayLength = 2.5f;
        }
        else if (buildingType.ToString() == "Assembler")
        {
            connectionRayLength = 5.0f;
        }else if(buildingType.ToString() == "Storage")
        {

        }
        if (Physics.Raycast(rayForward, out RaycastHit hit, connectionRayLength, layerMask))
        {

            if (hit.transform.GetComponent<BuildingScript>() != null)
            {
                buildingToConnect = hit.transform.GetComponent<BuildingScript>();

                if (hit.transform.gameObject != previousHitObject)
                {
                    // Reset the color of the previous object (if any)
                    if (previousHitObject != null)
                    {
                        ResetObjectColor(previousHitObject);
                    }



                    // Set the new hit object as the previous object
                    previousHitObject = hit.transform.gameObject;

                    // Change the color of the new hit object
                    changeObjectColor(hit.transform.gameObject);
                    
                }
            }
            else
            {
                // Reset the color of the previous object (if any) when no object is hit

                if (previousHitObject != null)
                {
                    ResetObjectColor(previousHitObject);
                    previousHitObject = null;
                }

            }

        }
        else
        {
            // Reset the color of the previous object (if any) when no object is hit
            if (previousHitObject != null)
            {
                ResetObjectColor(previousHitObject);
                previousHitObject = null;
            }


        }
    }

    private void checkBuildingToConnectBack()
    {
        rayForward = new Ray(transform.position, transform.forward);
        int layerMask = ~(1 << gameObject.layer);

        if (buildingType.ToString() == "Smelter")
        {
            connectionRayLength = 1.5f;
        }
        else if (buildingType.ToString() == "Refiner")
        {
            connectionRayLength = 2.5f;
        }
        else if (buildingType.ToString() == "Assembler")
        {
            connectionRayLength = 5.0f;
        }
        else if (buildingType.ToString() == "Storage")
        {

        }
        if (Physics.Raycast(rayForward, out RaycastHit hit, connectionRayLength, layerMask))
        {

            if (hit.transform.GetComponent<BuildingScript>() != null)
            {
                buildingToConnect = hit.transform.GetComponent<BuildingScript>();

                if (hit.transform.gameObject != previousHitObject)
                {
                    // Reset the color of the previous object (if any)
                    if (previousHitObject != null)
                    {
                        ResetObjectColor(previousHitObject);
                    }



                    // Set the new hit object as the previous object
                    previousHitObject = hit.transform.gameObject;

                    // Change the color of the new hit object
                    changeObjectColor(hit.transform.gameObject);

                }
            }
            else
            {
                // Reset the color of the previous object (if any) when no object is hit

                if (previousHitObject != null)
                {
                    ResetObjectColor(previousHitObject);
                    previousHitObject = null;
                }

            }

        }
        else
        {
            // Reset the color of the previous object (if any) when no object is hit
            if (previousHitObject != null)
            {
                ResetObjectColor(previousHitObject);
                previousHitObject = null;
            }


        }
    }

    private float setRayLength(float rayLength)
    {
        if (buildingType.ToString() == "Smelter")
        {
            rayLength = 1.5f;
        }
        else if (buildingType.ToString() == "Refiner")
        {
            rayLength = 2.5f;
        }
        else if (buildingType.ToString() == "Assembler")
        {
            rayLength = 5.0f;
        }
        else if (buildingType.ToString() == "Storage")
        {
            rayLength = 5.0f;
        }
        else
        {
            return 0f;
        }
        return rayLength;
    }
   
    private void changeObjectColor(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        // Change the color of the object to highlight it
        if (previousHitObject != null)
        {
            if (buildingType.ToString() == "Smelter")
            {
                if (previousHitObject.GetComponent<BuildingScript>().getBuildingType == "Refiner")
                {
                    if (renderer != null)
                    {
                        renderer.material.color = Color.yellow;
                    }
                }
            }
            else if (buildingType.ToString() == "Refiner")
            {
                if (previousHitObject.GetComponent<BuildingScript>().getBuildingType == "Assembler")
                {
                    if (renderer != null)
                    {
                        renderer.material.color = Color.yellow;
                    }
                }
            }
            else if (buildingType.ToString() == "Assembler")
            {

            }
        }





    }

    private void ResetObjectColor(GameObject obj)
    {
        // Reset the color of the object to its normal color
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white;
        }
    }

    #endregion

    

    #region triggers

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Selectable"))
        {
            colliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Selectable"))
        {
            colliding = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Selectable"))
        {
            colliding = true;
        }
    }

    #endregion

    public bool getCanBuild { get { return canBuild; } }

    public GameObject getObjectToBuild { get { return objectToBuild; } }

    public string getPlaceableBuildingType { get { return buildingType.ToString(); } }



    


    private void OnDestroy()
    {
        if (previousHitObject != null)
        {
            ResetObjectColor(previousHitObject);
        }
    }*/


}
