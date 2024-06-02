using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingHelper : MonoBehaviour
{
    private Building _building;
    [SerializeField] private float _timer;
    

    private void OnEnable()
    {
        _building = GetComponentInParent<Building>();
        _building.triggerGameObject = gameObject;
        Debug.Log(_timer);
        
    }

    private void Update()
    {
        if (_timer > 0)
        {
            if( !_building.isInfected )
            {
                _building.timerText.text = ProductionTimer();
            }
            else
            {
                _building.timerText.gameObject.SetActive(false);
            }
        }
        else
        {
            _building.timerText.text = "Ready!";
        }
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_building.BuildingType.ToString()))
        {
            
            Debug.Log("Element in trigger!");
            _building.elementsInTrigger++;

            if (!other.GetComponentInParent<ElementsController>().hasPlague)
            {
                ResetTimer();
                _building.timerText.gameObject.SetActive(true);
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
        ResetTimer();
        StartCoroutine(_building.StartProduction());
    }

    private string ProductionTimer()
    {
        _timer -= Time.deltaTime;
        int timeInSeconds = (int)(_timer % 60);
        return string.Format("{0:00}", timeInSeconds);
    }

    private void ResetTimer()
    {
        _timer = _building.SpawnRate;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ResetTimer();
    }

}
