using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArtifactContainer", menuName = "Data Container/Artifact Container")]
public class ArtifactSO : ScriptableObject
{
    public List<Artifact> forestArtifacts;
    public List<Artifact> playerArtifacts;
}
