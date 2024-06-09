using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactForestFrequentInfection : Artifact
{
    protected override void ArtifactPower()
    {
        GameManager.Instance.InfectionFrequency = artifactValue;
    }
}
