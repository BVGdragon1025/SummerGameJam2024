using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactForestFastPlague : Artifact
{
    protected override void ArtifactPower()
    {
        GameManager.Instance.PlagueIncrease = artifactValue;
    }
}
