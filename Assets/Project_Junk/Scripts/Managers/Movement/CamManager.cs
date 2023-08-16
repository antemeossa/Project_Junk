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

public enum CameraCinematicType
{
    MotherShipLanding
}
public class CamManager : MonoBehaviour
{






    [Header("Main Variables")]
    public Transform camHolder;
    public Camera MainCamera;
    public CameraModeEnum currentMode = CameraModeEnum.Factory;
    public float speedMultiplier = 2;
    public float rotationSpeed;

    [Header("Factory Mode Variables")]
    public float movementSpeed_F;
    public float zoomSpeed_F;
    public float movementSmoothness_F;
    public float zoomSmoothness_F;
    public Vector2 movementBounds_F = new Vector2(0, 0);
    public Vector2 zoomBounds_F = new Vector2(0, 0);
    public Vector3 defaultFactoryPosition = new Vector3(0, 0, 0);


    [Header("Explore Mode Variables")]
    public float movementSpeed_E;
    public float zoomSpeed_E;
    public float movementSmoothness_E;
    public float zoomSmoothness_E;
    public Vector2 movementBounds_E = new Vector2(0, 0);
    public Vector2 zoomBounds_E = new Vector2(0, 0);
    public Vector3 defaultExplorePosition = new Vector3(0, 0, 0);

    [Header("LandingCinematic")]
    public Vector3 landingWatchPosition = new Vector3(0, 0, 0);

    [SerializeField] private float currentMovementSpeed;
    [SerializeField] private MotherShipMovement mothership;
    private float defaultMovementSpeed, currentZoomSpeed, currentMovementSmoothness, currentZoomSmoothness;
    private float currentRotationSpeed, currentRotationSmoothness;
    private Vector2 currentMovementBounds;
    private Vector2 currentZoomBounds;
    private Vector3 nextTargetZoomPos, zoomTargetPos, nextTargetPos;
    private Quaternion targetRotation;
    private float targetAngle, currentAngle;
    private Vector3 cameraDirection => transform.InverseTransformDirection(camHolder.forward);
    private bool isTransitioning = false;


    private void Awake()
    {
        
        
       
    }
    private void Start()
    {
        if (!mothership.hasLanded )
        {
            camTransition(CameraModeEnum.Cinematic);
            changeVariables(CameraModeEnum.Cinematic);
            camHolder.transform.localPosition = landingWatchPosition;
            StartCoroutine(watchLandingCinematic());

        }
        else
        {
            zoomTargetPos = camHolder.position;
            targetRotation = camHolder.rotation;
            targetAngle = camHolder.eulerAngles.y;
            currentAngle = targetAngle;
            currentRotationSpeed = 0;
        }
       
    }

    private void Update()
    {
        if (mothership.hasLanded || !isTransitioning)
        {
            camInputActions();
            camMovement();
            zoomAction();
            rotateCam();
        }
        
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

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            if (!isTransitioning)
            {
                currentMovementSpeed += Time.deltaTime + speedMultiplier;
                currentMovementSpeed = Mathf.Clamp(currentMovementSpeed, defaultMovementSpeed, defaultMovementSpeed * 4);
            }

        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            currentMovementSpeed = defaultMovementSpeed;
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
            defaultMovementSpeed = movementSpeed_F;
            currentMovementSpeed = movementSpeed_F;
            currentZoomSpeed = zoomSpeed_F;
            currentMovementBounds = movementBounds_F;
            currentZoomBounds = zoomBounds_F;
            currentZoomSmoothness = zoomSmoothness_F;
            currentMovementSmoothness = movementSmoothness_F;
        }
        else if (mode == CameraModeEnum.Explore)
        {
            defaultMovementSpeed = movementSpeed_E;
            currentMovementSpeed = movementSpeed_E;
            currentZoomSpeed = zoomSpeed_E;
            currentMovementBounds = movementBounds_E;
            currentZoomBounds = zoomBounds_E;
            currentZoomSmoothness = zoomSmoothness_E;
            currentMovementSmoothness = movementSmoothness_E;
        }
    }

    private void camTransition(CameraModeEnum nextMode)
    {
        CameraModeEnum previousMode = currentMode;
        
        
        
        if(previousMode != CameraModeEnum.Cinematic)
        {
            if (previousMode == CameraModeEnum.Factory && nextMode == CameraModeEnum.Explore)
            {
                camHolder.DOKill();
                isTransitioning = true;
                camHolder.DOLocalMove(defaultExplorePosition, 1).OnComplete(() =>
                {
                    zoomTargetPos = defaultExplorePosition;
                    //camHolder.localPosition = defaultExplorePosition;
                    isTransitioning = false;
                });
            }
            else if (previousMode == CameraModeEnum.Explore && nextMode == CameraModeEnum.Factory)
            {
                camHolder.DOKill();
                isTransitioning = true;
                
                camHolder.DOLocalMove(defaultFactoryPosition, 1).OnComplete(() =>
                {
                    camHolder.DOLocalRotate(new Vector3(40, 0, 0), 1).OnComplete(() =>
                    {
                        targetAngle = camHolder.eulerAngles.y;
                        currentAngle = targetAngle;
                    });
                    zoomTargetPos = defaultFactoryPosition;
                    //camHolder.localPosition = defaultFactoryPosition;
                    isTransitioning = false;
                }); ;
            }
        }
        else if(previousMode == CameraModeEnum.Cinematic && nextMode == CameraModeEnum.Factory)
        {
            camHolder.DOKill();
            isTransitioning = true;
            Debug.Log("In");
            camHolder.DOLocalMove(defaultFactoryPosition, 2).OnComplete(() =>
            {
                zoomTargetPos = defaultFactoryPosition;
                camHolder.DOLocalRotate(new Vector3(40, 0, 0), 1).OnComplete(() =>
                {
                    targetAngle = camHolder.eulerAngles.y;
                    currentAngle = targetAngle;
                });
                //camHolder.localPosition = defaultExplorePosition;
                isTransitioning = false;
                Debug.Log("transition");
            });
        }
        

    }

    IEnumerator watchLandingCinematic()
    {
        float t = mothership.landingTime;
        float currentT = 0;

        
         
        while(currentT < t + 1)
        {
            currentT += Time.deltaTime;
            camHolder.LookAt(mothership.gameObject.transform);
            yield return null;
        }

        currentT = 0;

        while (currentT < mothership.timeAfterLanding)
        {
            currentT += Time.deltaTime;

            yield return null;
        }
        
        camTransition(CameraModeEnum.Factory);
        changeVariables(CameraModeEnum.Factory);
        zoomTargetPos = camHolder.position;
        targetRotation = camHolder.rotation;
        targetAngle = camHolder.eulerAngles.y;
        currentAngle = targetAngle;
        currentRotationSpeed = 0;
        yield return null;

    }

    private void camMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 right = transform.right * x;
        Vector3 forward = transform.forward * z;

        Vector3 movementInput = (forward + right).normalized;
        
        nextTargetPos = transform.position + movementInput * currentMovementSpeed;
        Debug.Log(nextTargetPos);
        setBounds(nextTargetPos);
        transform.position = setBounds(nextTargetPos);

    }

    private void zoomAction()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");

        
        nextTargetZoomPos = zoomTargetPos + cameraDirection * (zoomInput * currentZoomSpeed);
        zoomTargetPos = nextTargetZoomPos;
        camHolder.localPosition = Vector3.Lerp(camHolder.localPosition, nextTargetZoomPos, currentZoomSmoothness);

    }

    private void rotateCam()
    {
        handleRotationInput();
        currentAngle = Mathf.Lerp(currentAngle, -targetAngle, Time.deltaTime * currentRotationSmoothness);
        //camHolder.rotation = Quaternion.AngleAxis(currentAngle, new Vector3(0,1,0));
        transform.Rotate(Vector3.up, currentRotationSpeed, Space.World);
    }

    private void handleRotationInput()
    {
        if (Input.GetMouseButton(2))
        {
            targetAngle -= Input.GetAxisRaw("Mouse X") * currentRotationSpeed;
            currentRotationSpeed = rotationSpeed * Input.GetAxisRaw("Mouse X");
        }
        else if (Input.GetMouseButtonUp(2))
        {
            targetAngle = 0;
            currentRotationSpeed = 0;
        }
    }

    private Vector3 setBounds(Vector3 pos)
    {

        float clampedX = Mathf.Clamp(pos.x, currentMovementBounds.x, currentMovementBounds.y);
        float clampedZ = Mathf.Clamp(pos.z, currentMovementBounds.x, currentMovementBounds.y);
        return new Vector3(clampedX, pos.y, clampedZ);
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
