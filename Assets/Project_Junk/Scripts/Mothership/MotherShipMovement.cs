using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines.Interpolators;

public class MotherShipMovement : MonoBehaviour
{
    private string mothersShipName;
    private float speed;
    public float landingTime;
    public GameObject landingGear, forceFieldGuard, jetEngineVFX;
    public bool hasLanded = false;
    public float landingShakeDuration, timeAfterLanding;
    public Material forceFieldMat, jetEngineMat;
    public Transform apexPoint;
    public GameObject factoryGround;
   

    private Color guardColor = new Color(0,0,0,1);


    private void Start()
    {
        
        if (hasLanded)
        {
            transform.position = Vector3.zero;
            landingGear.transform.DOScale(1f, 0.1f);
        }
        else
        {
            transform.position = new Vector3(0, 5000, 0);
        }
        landMothership(Vector3.zero, landingTime);
    }
    

    public void landMothership(Vector3 landingTarget, float time)
    {
        if (!hasLanded)
        {
            forceFieldMat.SetVector("_RimPowerRange", new Vector2(0, 2));
            forceFieldMat.color = guardColor;

            StartCoroutine(openLandingGear());

            transform.DOMove(landingTarget, time).OnComplete(() =>
            {
                GameManager.Instance.Utils.camShake(5, landingShakeDuration);
                forceFieldGuard.GetComponent<Renderer>().material.DOFade(0, 2);
                StartCoroutine(changeForceFieldOpacity());
                jetEngineVFX.SetActive(false);
                hasLanded = true;
            });
            GameManager.Instance.Utils.camShake(1, time);
        }
        else
        {
            jetEngineVFX.SetActive(false);
            forceFieldGuard.GetComponent<Renderer>().material.DOFade(0, 2);
            forceFieldMat.SetVector("_RimPowerRange", new Vector2(0, 20));
        }
        


    }

    public void landingOperations(GameObject obj, float time)
    {
        obj.transform.DOScale(1, time);
        obj.transform.DORotate(new Vector3(0, -180, 0), (time / 4) * 3, RotateMode.Fast); 

    }

    IEnumerator openLandingGear()
    {
        float currentT = 0f;

        while(currentT < (landingTime / 4))
        {
            currentT += Time.deltaTime;
            yield return null;
        }

        landingOperations(landingGear , landingTime / 2);

        yield return null;

    }
    IEnumerator changeForceFieldOpacity()
    {
        float currentT = 0;
        Vector2 opacityLerp = new Vector2(0, 2);

        while (currentT < landingShakeDuration)
        {
            currentT += Time.deltaTime ;
            yield return null;
        }

        currentT = 0;
        while (currentT < timeAfterLanding)
        {
            currentT += Time.deltaTime;
            forceFieldMat.SetVector("_RimPowerRange", opacityLerp);            
            opacityLerp.y += Time.deltaTime * 4;
            yield return null;
        }




    }


}
