using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTest : Building
{
    public override void GiveResource()
    {
        Debug.Log("Gave resource to Player!");
        GameManager.Instance.ChangeCurrencyValue(resourceAmount);
    }

    public override void ResetProduction()
    {
        throw new System.NotImplementedException();
    }
}
