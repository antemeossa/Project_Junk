using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;



public class PlayerController : MonoBehaviour
{
    //Managers Vats
    public GameManager GM;
    public UI_Manager UI_M;
    public BuildSystemManager BS_M;

    //Building Vars
    private GameObject selectedBuilding;

    //Ray Vars
    private Ray ray;
    private RaycastHit hit;

    //Operation Vars
    private currentModeType previousMode;


    private void Start()
    {

        GM.currentMode = currentModeType.PlayMode;
        previousMode = currentModeType.PlayMode;




    }

    private void Update()
    {
        inputActions();

    }


    private void selectBuilding()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Selectable"))
            {


                if (selectedBuilding != null && !EventSystem.current.IsPointerOverGameObject() && GM.currentMode.Equals(currentModeType.PlayMode))
                {

                    unselectBuilding();
                    selectedBuilding = hit.transform.gameObject;
                    BS_M.setCurrentFactory(selectedBuilding.GetComponent<BuildingScript>().getBuildingFactory);
                    selectedBuilding.GetComponent<Renderer>().material.color = Color.cyan;
                    if (selectedBuilding.GetComponent<BuildingScript>() != null)
                    {
                        if (selectedBuilding.GetComponent<StorageScript>() == null)
                        {
                            UI_M.activateSmallDetailsPanel();
                        }
                        else
                        {
                            UI_M.activateStoragePanel();
                        }


                    }




                }
                else
                {
                    if (selectedBuilding == null && !EventSystem.current.IsPointerOverGameObject())
                    {
                        selectedBuilding = hit.transform.gameObject;


                        selectedBuilding.GetComponent<Renderer>().material.color = Color.cyan;


                        if (selectedBuilding.GetComponent<StorageScript>() == null)
                        {
                            UI_M.activateSmallDetailsPanel();
                        }
                        else
                        {
                            UI_M.activateStoragePanel();
                        }
                    }


                }




            }
            else if (hit.transform.gameObject.CompareTag("Factory"))
            {
                BS_M.setCurrentFactory(hit.transform.gameObject);
            }
            else
            {
                if (selectedBuilding != null && !EventSystem.current.IsPointerOverGameObject() && !UI_M.getIsFocusedOnBuilding)
                {
                    unselectBuilding();
                }
            }
        }
    }

    private void unselectBuilding()
    {
        if (selectedBuilding != null)
        {

            selectedBuilding.GetComponent<Renderer>().material.color = Color.white;
            selectedBuilding = null;
            UI_M.deactivateStoragePanel();
            UI_M.deactivateSmallDetailsPanel();

        }



    }

    public void changeCurrentMode(int i)
    {
        switch (i)
        {
            case 0:
                GM.currentMode = currentModeType.PlayMode;
                break;
            case 1:
                GM.currentMode = currentModeType.BuildMode;
                break;
            case 2:
                GM.currentMode = currentModeType.UI_Mode;
                break;
            case 3:
                GM.currentMode = currentModeType.ConnectionMode;
                break;
            default:
                break;

        }
    }

    public string getCurrentModeString { get { return GM.currentMode.ToString(); } }

    private void inputActions()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GM.currentMode.Equals(currentModeType.PlayMode))
            {

                selectBuilding();
            }
            else if (GM.currentMode.Equals(currentModeType.BuildMode))
            {
                BS_M.buildObject();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (GM.currentMode == currentModeType.PlayMode)
            {
                unselectBuilding();
                UI_M.deactivateSmallDetailsPanel();
                UI_M.deactivateStoragePanel();
                GM.currentMode = currentModeType.PlayMode;

            }
            else if (GM.currentMode.Equals(currentModeType.BuildMode))
            {
                BS_M.cancelBuildAction();
            }
            else if (GM.currentMode.Equals(currentModeType.ConnectionMode))
            {
                GM.currentMode = currentModeType.BuildMode;
                //UI_M.switchBuildPanel();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GM.currentMode != currentModeType.BuildMode)
            {
                if (BS_M.getCurrentFactory != null)
                {
                    GM.currentMode = currentModeType.BuildMode;
                    BS_M.activateCurrentGrid(true);
                    UI_M.switchBuildPanel();
                }

            }
            else
            {
                UI_M.switchBuildPanel();
                BS_M.activateCurrentGrid(false);
                GM.currentMode = currentModeType.PlayMode;
                BS_M.cancelBuildAction();
                
            }

        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (selectedBuilding != null)
            {
                UI_M.activateDeleteNotification();
            }
        }
    }
    private void inputActions1()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (GM.currentMode == currentModeType.BuildMode)
            {
                UI_M.switchBuildPanel();
                BS_M.cancelBuildAction();
            }
            BS_M.buildConnector();


        }

       
        

       


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UI_M.switchBuildPanel();
            if (GM.currentMode != currentModeType.BuildMode)
            {
                if (GM.currentMode.Equals(currentModeType.ConnectionMode))
                {
                    BS_M.buildConnector();
                }
                GM.currentMode = currentModeType.BuildMode;
                unselectBuilding();
            }
            else
            {
                BS_M.cancelBuildAction();
                GM.currentMode = currentModeType.PlayMode;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (GM.currentMode == currentModeType.BuildMode)
            {
                if (BS_M.isBuilding)
                {
                    BS_M.buildObject();
                }
            }
            else if (GM.currentMode == currentModeType.PlayMode)
            {
                selectBuilding();
            }
            else if (GM.currentMode == currentModeType.UI_Mode)
            {

            }
            else if (GM.currentMode == currentModeType.ConnectionMode)
            {

            }

        }

        if (Input.GetMouseButtonDown(1))
        {
            if (GM.currentMode == currentModeType.BuildMode)
            {
                BS_M.cancelBuildAction();

            }
            else if (GM.currentMode == currentModeType.ConnectionMode)
            {
                GM.currentMode = currentModeType.PlayMode;
            }

            if (selectedBuilding != null)
            {
                if (selectedBuilding.GetComponent<StorageScript>() == null)
                {
                    UI_M.deactivateSmallDetailsPanel();
                    unselectBuilding();
                }
                else
                {
                    UI_M.deactivateStoragePanel();
                    unselectBuilding();
                }

            }
        }



    }

    public GameObject getSelectedBuilding { get { return selectedBuilding; } }
}
