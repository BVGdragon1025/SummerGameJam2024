using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPlayerMoreBuildings : Artifact
{
    protected override void ArtifactPower()
    {
        GameManager.Instance.MaxBuildingLimit = (int)artifactValue;
    }
}
