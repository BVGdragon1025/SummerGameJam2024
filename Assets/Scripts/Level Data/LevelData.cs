using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Data Container/Level Data")]
public class LevelData : ScriptableObject
{
    public float lvlPlagueValue;
    public float currency;
    public float playerPlagueValue;
}
