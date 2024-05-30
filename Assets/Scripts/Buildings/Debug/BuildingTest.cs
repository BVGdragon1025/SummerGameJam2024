using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTest : Building
{
    public override void GiveResourceToPlayer()
    {
        Debug.Log("Gave resource to Player!");
        GameManager.Instance.ChangeCurrencyValue(resourceAmount);
    }
}
