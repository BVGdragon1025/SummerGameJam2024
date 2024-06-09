using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPlayerMoreBuildings : Artifact
{
    protected override void ArtifactPower()
    {
        gameManager.MaxBuuildingLimit = (int)artifactValue;
    }
}
