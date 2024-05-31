using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBasic : Building
{
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
    }
}
