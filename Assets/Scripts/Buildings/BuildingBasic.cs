using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBasic : Building
{
    [Header("Basic Structure Specific")]
    [SerializeField, Tooltip("How many Nature Points player looses, when they collect resources from this building. 0 - none.")]
    private float _natureCost;
    public float NatureCost { get { return _natureCost; } }

    public override void GiveResourceToPlayer()
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

        gameManager.ChangeCurrencyValue(-_natureCost);

    }
}
