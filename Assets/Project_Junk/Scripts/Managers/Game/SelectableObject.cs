using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [SerializeField]
    private bool shouldChangeColors;

    [SerializeField]
    private int matIndex;

    [SerializeField]
    private Material defaultMaterial, currentMaterial, selectionMaterial;

    [SerializeField]
    private GameObject objectToChangeMaterial;

    private List<Material> materials = new List<Material>();

    private void Start()
    {
        
    }
    public void selectIt()
    {
        if(shouldChangeColors)
        {

            currentMaterial = selectionMaterial;
            materials = objectToChangeMaterial.GetComponent<Renderer>().materials.ToList();
            materials[matIndex] = currentMaterial;
            objectToChangeMaterial.GetComponent<Renderer>().SetMaterials(materials);
           
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
            materials = objectToChangeMaterial.GetComponent<Renderer>().materials.ToList();
            materials[matIndex] = currentMaterial;
            objectToChangeMaterial.GetComponent<Renderer>().SetMaterials(materials);

        }
        else
        {

        }
    }
}
