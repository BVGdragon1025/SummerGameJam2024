using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPlayerHealth : Artifact
{
    protected override void ArtifactPower()
    {
        gameManager.MaxPlayerPlagueValue = artifactValue;
    }

}
