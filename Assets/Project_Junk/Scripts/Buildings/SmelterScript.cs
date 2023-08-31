using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmelterScript : MonoBehaviour
{
    TransferItemsScript transfer;

    private void Start()
    {
        transfer = GetComponent<TransferItemsScript>();
    }

    private void Update()
    {
        transfer.setFacilities(GameManager.Instance.mothership, gameObject);
    }
}
