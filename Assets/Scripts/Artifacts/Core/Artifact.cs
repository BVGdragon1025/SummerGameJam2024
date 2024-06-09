using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Artifact : MonoBehaviour
{
    [SerializeField] protected string artifactName;
    [SerializeField, TextArea] protected string artifactDescription;
    [SerializeField] protected float artifactValue;
    [SerializeField, Tooltip("Should this artefact activate on Start (for Artifacts not changing value in Game Manager)")]
    protected bool activeOnStart;

    public string ArtifactName {  get { return artifactName; } }
    public string ArtifactDescription { get {  return artifactDescription; } }
    public float ArtifactValue { get {  return artifactValue; } }

    public void UseArtifact()
    {
        ArtifactPower();
    }

    protected abstract void ArtifactPower();


}
