using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPlayerSpecialBuilding : Artifact
{
    [SerializeField] private ArtifactBuildingType _buildingType;

    protected override void ArtifactPower()
    {
        switch (_buildingType)
        {
            case ArtifactBuildingType.Obelisk: GameManager.Instance.LevelData.isObeliskUnlocked = true; break;
            case ArtifactBuildingType.Stonehenge: GameManager.Instance.LevelData.isStonehengeUnlocked = true; break;
            case ArtifactBuildingType.Rostrum: GameManager.Instance.LevelData.isRostrumUnlocked = true; break;
        }
    }
}
