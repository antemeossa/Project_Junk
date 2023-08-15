using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.Splines;

public enum CameraModeEnum
{
    Cinematic,
    Factory,
    Explore,
}
public class CamManager : MonoBehaviour
{






    [Header("Main Variables")]
    public Transform camHolder;
    public Camera MainCamera;
    public CameraModeEnum currentMode = CameraModeEnum.Factory;

    [Header("Factory Mode Variables")]
    public float movementSpeed_F, zoomSpeed_F;
    public Vector2 movementBounds_F = new Vector2(0, 0);
    public Vector2 zoomBounds_F = new Vector2(0, 0);
    public Vector3 defaultFactoryPosition = new Vector3(0, 0, 0);

    [Header("Explore Mode Variables")]
    public float movementSpeed_E, zoomSpeed_E;
    public Vector2 movementBounds_E = new Vector2(0, 0);
    public Vector2 zoomBounds_E = new Vector2(0, 0);
    public Vector3 defaultExplorePosition = new Vector3(0, 0, 0);

    private float currentMovementSpeed, currentZoomSpeed;
    private Vector2 currentMovementBounds;
    private Vector2 currentZoomBounds;


    private void Update()
    {
        camInputActions();
        camMovement();
    }

    private void camInputActions()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            camTransition(CameraModeEnum.Factory);
            changeVariables(CameraModeEnum.Factory);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            camTransition(CameraModeEnum.Explore);
            changeVariables(CameraModeEnum.Explore);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {

        }
    }
    private void changeVariables(CameraModeEnum mode)
    {
        currentMode = mode;

        if (mode == CameraModeEnum.Cinematic)
        {

        }
        else if (mode == CameraModeEnum.Factory)
        {
            currentMovementSpeed = movementSpeed_F;
            currentZoomSpeed = zoomSpeed_F;
            currentMovementBounds = movementBounds_F;
            currentZoomBounds = zoomBounds_F;
        }
        else if (mode == CameraModeEnum.Explore)
        {
            currentMovementSpeed = movementSpeed_E;
            currentZoomSpeed = zoomSpeed_E;
            currentMovementBounds = movementBounds_E;
            currentZoomBounds = zoomBounds_E;
        }
    }

    private void camTransition(CameraModeEnum nextMode)
    {
        CameraModeEnum previousMode = currentMode;
        

        if (previousMode == CameraModeEnum.Factory && nextMode == CameraModeEnum.Explore)
        {
            camHolder.DOKill();
            camHolder.DOMove(defaultExplorePosition, 2);
        }
        else if (previousMode == CameraModeEnum.Explore && nextMode == CameraModeEnum.Factory)
        {
            camHolder.DOKill();
            camHolder.DOMove(defaultFactoryPosition, 2);
        }


    }

    private void camMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 nextPosition = camHolder.transform.position + new Vector3(-horizontalMovement, verticalMovement) * Time.deltaTime * currentMovementSpeed;
        Vector3 clampedPosition = setBounds(nextPosition);
        camHolder.transform.position = clampedPosition;
    }


    private Vector3 setBounds(Vector3 position)
    {
        
        float clampedX = Mathf.Clamp(position.x, currentMovementBounds.x, currentMovementBounds.y);
        float clampedY = Mathf.Clamp(position.x, currentMovementBounds.x, currentMovementBounds.y);
        return new Vector3(clampedX, clampedY, position.z);
    }

    /*
    [Header("Movement Settings")]
    [SerializeField]
    private float movementSpeed = 1.0f, movementSmoothness = 5f, rangeX, rangeY;

    [Header("Rotation Settings")]
    [SerializeField]
    private float rotationSpeed, rotationSmoothness;

    [Header("Zoom Settings")]
    [SerializeField]
    private float zoomSpeed, zoomSmoothness, zoomRangeMin, zoomRangeMax;

    private float defaultZoomSpeed, defaultMinZoomRange, defaultMaxZoomRange, defaultMovementSpeed, defaultRangeX, defaultRangeY;

    [Header("Salvage Values")]
    [SerializeField]
    private float salvageZoomSpeed, salvageMinZoomRange, salvageMaxZoomRange, salvageMovementSpeed, salvageRangeX, salvageRangeY;
    private float targetAngle, currentAngle, zoomInput;

    [SerializeField]
    private Transform camHolder;

    private Vector2 movementRange, zoomRange;

    private Vector3 movementTargetPos, movementInput, zoomTargetPos;

    private Vector3 cameraDirection => transform.InverseTransformDirection(camHolder.forward);

    private CameraModeEnum currentCamMode;

    

    private void Awake()
    {
        movementTargetPos = transform.position;
        movementRange = new Vector2(rangeX, rangeY);

        targetAngle = transform.eulerAngles.y;
        currentAngle = targetAngle;

        zoomRange = new Vector2(zoomRangeMin, zoomRangeMax);
        zoomTargetPos = camHolder.localPosition;

        setDefaultSettings();
        //transform.position = new Vector3(0, transform.position.y, 0);
    }

    private void setDefaultSettings()
    {
        defaultMaxZoomRange = zoomRangeMax;
        defaultMinZoomRange = zoomRangeMin;
        defaultZoomSpeed = zoomSpeed;
        defaultMovementSpeed = movementSpeed;
        defaultRangeX = rangeX;
        defaultRangeY = rangeY;
    }
    private void Update()
    {
        handleMovementInput();
        handleRotationInput();
        handleZoomInput();
        moveCam();
        rotateCam();
        zoomCam(currentCamMode);

        
    }


    private void progressionBetweenModes(CameraModeEnum nextCamMode)
    {
        CameraModeEnum previousCamMode = currentCamMode;
        if(nextCamMode != previousCamMode)
        {
            if(nextCamMode == CameraModeEnum.explore && previousCamMode == CameraModeEnum.factory)
            {

            }else if(nextCamMode == CameraModeEnum.factory && previousCamMode == CameraModeEnum.explore)
            {

            }
        }
    }
    private void handleMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 right = transform.right * x;
        Vector3 forward = transform.forward * z;

        movementInput = (forward + right).normalized;
    }

    private void handleRotationInput()
    {
        if (Input.GetMouseButton(2))
        {
            targetAngle += Input.GetAxisRaw("Mouse X") * rotationSpeed;
        }
    }

    private void handleZoomInput()
    {
        zoomInput = Input.GetAxisRaw("Mouse ScrollWheel");
    }

    private Vector3 nextTargetPos;

    
    private void zoomCam(CameraModeEnum mode)
    {
        if(mode == CameraModeEnum.factory)
        {
            nextTargetPos = zoomTargetPos + cameraDirection * (zoomInput * zoomSpeed);
            if (zoomIsInBounds(nextTargetPos))
            {
                zoomTargetPos = nextTargetPos;
            }
            //camHolder.localPosition = Vector3.Lerp(camHolder.localPosition, nextTargetPos, Time.deltaTime * zoomSmoothness);
            camHolder.localPosition = nextTargetPos;
        }else if(mode == CameraModeEnum.explore)
        {

        }
        
    }

    private bool zoomIsInBounds(Vector3 pos)
    {
        return pos.magnitude > zoomRange.x && pos.magnitude < zoomRange.y;
    }

    private void rotateCam()
    {
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * rotationSmoothness);
        transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
    }

    private void moveCam()
    {
        Vector3 nextTargetPos = movementTargetPos + movementInput * movementSpeed;
        if(movementIsInBounds(nextTargetPos))
        {
            movementTargetPos = nextTargetPos;
        }

        transform.position = Vector3.Lerp(transform.position, movementTargetPos, Time.deltaTime * movementSmoothness);
    }

    private bool movementIsInBounds(Vector3 pos)
    {
        return pos.x > -movementRange.x &&
            pos.x < movementRange.x &&
            pos.z > -movementRange.y &&
            pos.z < movementRange.y;
    }

    private void setCamMode()
    {
        
        if(GameManager.Instance.currentMode == currentModeType.PlayMode)
        {
            zoomRangeMin = defaultMinZoomRange;
            zoomRange.x = zoomRangeMin;
            
            movementSpeed = defaultMovementSpeed;
            zoomRangeMax = defaultMaxZoomRange;   
            zoomRange.y = zoomRangeMax;
            zoomSpeed = defaultZoomSpeed;
            rangeX = defaultRangeX; rangeY = defaultRangeY;
            movementRange.x = rangeX; movementRange.y = rangeY;
        }else if(GameManager.Instance.currentMode == currentModeType.SalvageMode)
        {
            zoomRangeMax = salvageMaxZoomRange;
            zoomRange.y = zoomRangeMax;
            
            movementSpeed = salvageMovementSpeed;            
            zoomRangeMin = salvageMinZoomRange;
            zoomRange.x = zoomRangeMin;
            zoomSpeed = salvageZoomSpeed;
            rangeX = salvageRangeX; rangeY = salvageRangeY;
            movementRange.x = rangeX; movementRange.y = rangeY;

        }
    }
    */
}
