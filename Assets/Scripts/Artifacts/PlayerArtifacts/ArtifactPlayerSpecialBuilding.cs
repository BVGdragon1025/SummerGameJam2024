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
            case ArtifactBuildingType.Obelisk: gameManager.LevelData.isObeliskUnlocked = true; break;
            case ArtifactBuildingType.Stonehenge: gameManager.LevelData.isStonehengeUnlocked = true; break;
            case ArtifactBuildingType.Rostrum: gameManager.LevelData.isRostrumUnlocked = true; break;
        }
    }
}
