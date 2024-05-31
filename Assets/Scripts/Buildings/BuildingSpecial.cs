using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpecial : Building
{
    public override void GiveResourceToPlayer()
    {
        Debug.Log("This building does nothing to player. If you see this message - something bad happened!");
    }
}
