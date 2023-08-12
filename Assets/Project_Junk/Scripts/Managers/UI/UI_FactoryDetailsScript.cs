using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FactoryDetailsScript : MonoBehaviour
{
    [SerializeField]
    private PlayerController PC;
    [SerializeField]
    private BuildSystemManager BS_M;
    [SerializeField]
    private GameObject element, listParent;

    private FactoryScript factory;
    private List<GameObject> facilities = new List<GameObject>();

    private void OnEnable()
    {
        setList();
        factory = BS_M.getCurrentFactory.transform.parent.GetComponent<FactoryScript>();
        facilities = factory.getAllBuildingsOnFactory;
    }
    private void setList()
    {
        for(int i = 0; i < facilities.Count; i++)
        {
            Instantiate(element, listParent.transform, false);
            
        }
    }
}
