using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHelper : MonoBehaviour
{
    private Building _building;

    private void OnEnable()
    {
        _building = GetComponentInParent<Building>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_building.BuildingType.ToString()))
        {
            Debug.Log("Element in trigger!");
            _building.elementsInTrigger++;

            if (!other.GetComponentInParent<ElementsController>().hasPlague)
            {
                Debug.LogFormat("Element is <color=green>healthy!</color>");
                StartCoroutine(_building.StartProduction());
            }
            else
            {
                Debug.LogWarningFormat("Element has <color=black>plague!</color>");
            }
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && _building.hasFinished)
        {
            ResetProduction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(_building.BuildingType.ToString()))
        {
            Debug.Log("Element exited trigger!");
            _building.elementsInTrigger--;
        }
    }

    void ResetProduction()
    {
        _building.GiveResourceToPlayer();
        StartCoroutine(_building.StartProduction());
    }

}
