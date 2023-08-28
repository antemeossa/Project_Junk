using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckageManager : MonoBehaviour
{
    public List<GameObject> allWreckages = new List<GameObject>();

    [SerializeField]
    GameObject wreckageParent;

    private void Start()
    {
        for (int i = 0; i < wreckageParent.transform.childCount; i++)
        {
            allWreckages.Add(wreckageParent.transform.GetChild(i).gameObject);
        }
    }
}
