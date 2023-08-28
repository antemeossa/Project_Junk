using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WreckPartDissolveScript : MonoBehaviour
{
    private float dissolveAmount = 1;
    private Material dissolveMaterial;


    private void Start()
    {
        dissolveMaterial = transform.parent.GetComponent<WreckAreaScript>().wreckDissolveMat;
        
    }
    
    public void updateDissolveAmount()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().material = dissolveMaterial;
        }
        
        float maxDissolve = transform.parent.GetComponent<InventoryScript>().getMaxStorage() / transform.parent.childCount;
        float currentDissolve = transform.parent.GetComponent<InventoryScript>().getCurrentStorage() % maxDissolve;

        dissolveAmount = Mathf.Lerp(dissolveAmount, currentDissolve/maxDissolve, Time.deltaTime);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().material.SetFloat("_DissolveAmount", 1 - dissolveAmount);
        }

        

    }
}
