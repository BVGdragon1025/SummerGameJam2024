using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpecial : Building
{
    [Header("Special Building Specific")]
    public int elementsInTrigger;
    public GameObject triggerGameObject;

    private void OnEnable()
    {
        triggerGameObject.SetActive(true);
    }

    public override void GiveResource()
    {
        Debug.Log("This building does nothing to player. If you see this message - something bad happened!");
    }

    public override void ResetProduction()
    {
        throw new System.NotImplementedException();
    }
}
