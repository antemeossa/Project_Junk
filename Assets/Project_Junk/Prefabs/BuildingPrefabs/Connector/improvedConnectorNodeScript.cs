using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class improvedConnectorNodeScript : MonoBehaviour
{
    [SerializeField]
    private GameObject straightMesh, mergerMesh, inputMesh, turn1, turn2, turn3, turn4;

    [SerializeField]
    private Mesh turnMesh1, turnMesh2, turnMesh3, turnMesh4;
    private bool canPlace = true, DestroyOnEnd, hasPlaced = false;
    private bool destroy = false;
    private int meshIndex;
    private Ray ray;
    private RaycastHit hit;

    private void Start()
    {

    }

    public void swapMeshToTurn(Mesh mesh)
    {

        transform.GetChild(0).GetComponent<MeshFilter>().mesh = mesh;
        transform.GetChild(1).GetComponent<MeshFilter>().mesh = mesh;
        transform.GetChild(2).GetComponent<MeshFilter>().mesh = mesh;
        //transform.rotation = Quaternion.identity;
        //transform.localScale = new Vector3(2, 2, 3f);

    }

    public void switchToMerger()
    {


        swapMeshToTurn(turnMesh1);
        transform.rotation = Quaternion.identity;







    }
    public void switchToInput()
    {
        GameObject obj = Instantiate(inputMesh, transform);
        obj.transform.SetParent(transform.parent, true);
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Selectable"))
        {
            canPlace = false;
        }
        else
        {
            canPlace = true;
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Selectable"))
        {
            canPlace = false;
            DestroyOnEnd = true;
        }
        else
        {
            canPlace = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Merger"))
        {
            DestroyOnEnd = false;
        }

        if (other.gameObject.CompareTag("Selectable"))
        {
            canPlace = true;
        }

    }



    public bool getCanPlace { get { return canPlace; } }

    public bool getDestroyOnEnd { get { return DestroyOnEnd; } }

    public bool getHasPlaced { get { return hasPlaced; } }

    public void setHasPlaced(bool value) { hasPlaced = value; }
}
