using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPlayerSpeed : Artifact
{
    protected override void ArtifactPower()
    {
        gameManager.PlayerSpeed = artifactValue;
    }
}
