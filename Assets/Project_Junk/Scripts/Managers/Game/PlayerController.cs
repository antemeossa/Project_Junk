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
                    
                    
                    if (selectedBuilding.GetComponent<BuildingScript>() != null)
                    {
                        BS_M.setCurrentFactory(selectedBuilding.transform.parent.gameObject);
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
                        if(selectedBuilding.GetComponent<SelectableObject>() != null)
                        {
                            selectedBuilding.GetComponent<SelectableObject>().selectIt();
                        }
                        hit.transform.gameObject.GetComponent<SelectableObject>().selectIt();
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

            if (selectedBuilding.GetComponent<SelectableObject>() != null)
            {
                selectedBuilding.GetComponent<SelectableObject>().deselectIt();
            }
            selectedBuilding = null;
            UI_M.deactivateStoragePanel();
            UI_M.deactivateSmallDetailsPanel();

        }

        if(BS_M.getCurrentFactory != null)
        {
            BS_M.getCurrentFactory.transform.parent.GetComponent<SelectableObject>().deselectIt();
            BS_M.setCurrentFactory(null);
        }



    }

    

   

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
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GM.currentMode != currentModeType.BuildMode && GM.currentMode != currentModeType.SalvageMode)
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
    

    public GameObject getSelectedBuilding { get { return selectedBuilding; } }
}
