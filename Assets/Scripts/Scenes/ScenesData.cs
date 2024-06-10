using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scenes Container", menuName = "Data Container/Scenes Container")]
public class ScenesData : ScriptableObject
{
    public List<string> scenesList;
    public List<string> defaultSceneList;
    public int scenesPlayed;

}
