using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Artifact : MonoBehaviour
{
    [SerializeField] protected string artifactName;
    [SerializeField, TextArea] protected string artifactDescription;
    [SerializeField] protected float artifactValue;
    protected GameManager gameManager;

    public string ArtifactName {  get { return artifactName; } }
    public string ArtifactDescription { get {  return artifactDescription; } }
    public float ArtifactValue { get {  return artifactValue; } }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void UseArtifact()
    {
        ArtifactPower();
    }

    protected abstract void ArtifactPower();


}
