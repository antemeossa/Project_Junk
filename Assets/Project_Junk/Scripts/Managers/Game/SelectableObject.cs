using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [SerializeField]
    private bool shouldChangeColors;

    [SerializeField]
    private Material defaultMaterial, currentMaterial, selectionMaterial;

    [SerializeField]
    private GameObject objectToChangeMaterial;

    private void Start()
    {
        
    }
    public void selectIt()
    {
        if(shouldChangeColors)
        {
            currentMaterial = selectionMaterial;
            objectToChangeMaterial.GetComponent<MeshRenderer>().material = currentMaterial;
           
        }
        else
        {

        }
    }

    public void deselectIt()
    {
        if(shouldChangeColors)
        {
            currentMaterial = defaultMaterial;
            objectToChangeMaterial.GetComponent<MeshRenderer>().material = currentMaterial;
        }
        else
        {

        }
    }
}
