using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArtifactContainer", menuName = "Data Container/Artifact Container")]
public class ArtifactSO : ScriptableObject
{
    public List<Artifact> forestArtifacts;
    public List<Artifact> playerArtifacts;

    [field: Header("Default Lists")]
    [field: SerializeField] public List<Artifact> DefaultForestArtifacts { get; private set; }
    [field: SerializeField] public List<Artifact> DefaultPlayerArtifacts { get; private set; }
}
