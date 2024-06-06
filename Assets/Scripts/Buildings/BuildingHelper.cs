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
                StartCoroutine(_building.StartProduction(_timer));
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
            _building.GiveResourceToPlayer();
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

    public void ResetProduction()
    {
        ResetTimer();
        StartCoroutine(_building.StartProduction(_timer));
    }

    private string ProductionTimer()
    {
        _timer -= Time.deltaTime;
        int timeInSeconds = (int)(_timer % 60);
        return string.Format("{0:00}", timeInSeconds);
    }

    private void ResetTimer()
    {
        if (_timer <= 0.0f)
        {
            _timer = _building.SpawnRate;
        }
        else
        {
            Debug.Log($"SpawnRate: {_building.SpawnRate}, timer: {_timer}");
            float helper = _building.SpawnRate - _timer;
            _timer = _building.SpawnRate - helper;
            Debug.Log($"SpawnRate: {_building.SpawnRate}, timer: {_timer}, helper: {helper}");
        }
        
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ResetTimer();
    }

}
