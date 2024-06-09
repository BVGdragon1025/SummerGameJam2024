using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPlayerCurrency : Artifact
{
    protected override void ArtifactPower()
    {
        gameManager.MaxPlayerCurrency = artifactValue;
    }
}
