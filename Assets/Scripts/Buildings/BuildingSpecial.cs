using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpecial : Building
{
    public override void GiveResource()
    {
        Debug.Log("This building does nothing to player. If you see this message - something bad happened!");
    }

    public override void ResetProduction()
    {
        throw new System.NotImplementedException();
    }
}
