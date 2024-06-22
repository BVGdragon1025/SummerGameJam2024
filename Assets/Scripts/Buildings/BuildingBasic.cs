using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingManager))]

public class BuildingBasic : Building
{
    [Header("Basic Structure Specific")]
    [SerializeField, Tooltip("How many Nature Points player looses, when they collect resources from this building. 0 - none.")]
    private float _natureCost;
    public float NatureCost { get { return _natureCost; } }
    [SerializeField]
    private float _productionTimer;

    private void OnEnable()
    {
        timerText.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(_productionTimer > 0)
        {
            if(plagueState == PlagueState.Healthy)
            {
                timerText.text = ProductionTimer();
            }
        }
        else
        {
            timerText.text = "Ready!";
        }
    
    }

    protected string ProductionTimer()
    {
        _productionTimer -= Time.deltaTime;
        int timeInSeconds = (int)(_productionTimer % 60);
        return string.Format("{0:00}", timeInSeconds);
    }

    protected void ResetTimer()
    {
        if (_productionTimer <= 0.0f || _productionTimer >= spawnRate)
        {
            _productionTimer = spawnRate;

        }
        else
        {
            float helper = spawnRate - _productionTimer;
            _productionTimer = spawnRate - helper;

        }

    }

    public override void ResetProduction()
    {
        ResetTimer();
        StartCoroutine(StartProduction(_productionTimer));
    }

    public override void GiveResource()
    {
        switch(buildingType)
        {
            case BuildingType.Pond:
                gameManager.ChangeForestPlagueLevel(-resourceAmount);
                break;
            case BuildingType.Meadow:
                gameManager.ChangePlayerPlagueLevel(-resourceAmount);
                break;
            case BuildingType.Tree:
                gameManager.ChangeCurrencyValue(resourceAmount);
                break;
        }

        ResetProduction();
        gameManager.ChangeCurrencyValue(-_natureCost);

    }
}
