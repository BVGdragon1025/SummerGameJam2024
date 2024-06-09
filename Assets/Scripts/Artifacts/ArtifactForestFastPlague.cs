using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactForestFastPlague : Artifact
{
    protected override void ArtifactPower()
    {
        gameManager.LevelData.plagueIncreaseFrequency = artifactValue;
    }
}
