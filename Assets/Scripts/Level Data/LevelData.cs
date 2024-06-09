using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Data Container/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Level Data")]
    public float lvlPlagueValue;
    public float currency;
    public float playerPlagueValue;
    public float plagueIncreaseFrequency;
    public float infectionFrequency;

    [Header("Player Specific Data")]
    public float playerSpeed;

    [Header("Building Specific Data")]
    public int maxBuildingInRange;
    public bool isObeliskUnlocked;
    public bool isStonehengeUnlocked;
    public bool isRostrumUnlocked;

    [field: Header("Default values")]
    [field: SerializeField] public float DefaultLvlPlagueValue { get; private set; }
    [field: SerializeField] public float DefaultCurrency { get; private set; }
    [field: SerializeField] public float DefaultPlayerPlagueValue { get; private set; }
    [field: SerializeField] public float DefaultPlagueIncreaseFrequency { get; private set; }
    [field: SerializeField] public float DefaultInfectionFrequency { get; private set; }
    [field: SerializeField] public float DefaultPlayerSpeed { get; private set; }
    [field: SerializeField] public float DefaultMaxBuildingInRange { get; private set; }

}
