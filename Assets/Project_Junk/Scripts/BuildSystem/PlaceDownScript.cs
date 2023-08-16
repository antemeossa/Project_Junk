using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceDownScript : MonoBehaviour
{
    [SerializeField]
    private float dropHeight, dropTime, shakeStrength, shakeDuration;
    

    private void Start()
    {
        dropDown(transform.gameObject);
    }
    public void dropDown(GameObject obj)
    {
        float y = transform.position.y;
        //transform.position = new Vector3(transform.position.x, y + dropHeight, transform.position.z);
        obj.transform.DOMoveY(y + dropHeight *2, .1f);
        obj.transform.DOMoveY(y, dropTime, false).OnComplete(() =>
        {
            GetComponent<BuildingScript>().playPlacementVFX();
            GameManager.Instance.Utils.camShake(shakeDuration, shakeStrength);
        });
        
        //GetComponent<VFXPlayer>().PlayPlacementVFX();
    }
}
