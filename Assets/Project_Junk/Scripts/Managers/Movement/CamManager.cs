using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;


public class CamManager : MonoBehaviour
{
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
        zoomCam();

        if (Input.GetKey(KeyCode.Alpha1))
        {
            GameManager.Instance.currentMode = currentModeType.SalvageMode;
            setCamMode();
        }else if(Input.GetKey(KeyCode.Alpha2))
        {
            GameManager.Instance.currentMode = currentModeType.PlayMode;
            setCamMode();
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
    private void zoomCam()
    {
        nextTargetPos = zoomTargetPos + cameraDirection * (zoomInput * zoomSpeed);
        if (zoomIsInBounds(nextTargetPos))
        {
            zoomTargetPos = nextTargetPos;
        }
        //camHolder.localPosition = Vector3.Lerp(camHolder.localPosition, nextTargetPos, Time.deltaTime * zoomSmoothness);
        camHolder.localPosition = nextTargetPos;
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

}
