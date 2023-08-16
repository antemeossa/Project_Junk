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
   

    private Color guardColor = new Color(0,0,0,1);


    private void Start()
    {
        landMothership(Vector3.zero, landingTime);

    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            landMothership(Vector3.zero, landingTime);
        }
    }

    public void landMothership(Vector3 landingTarget, float time)
    {
        forceFieldMat.SetVector("_RimPowerRange", new Vector2(0, 2));
        landingOperations(landingGear, landingTime / 2);        
        forceFieldMat.color = guardColor;
        
        
        
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

    public void landingOperations(GameObject obj, float time)
    {
        obj.transform.DOScale(1, time);

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
