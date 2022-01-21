using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueGhost : Ghost
{
    protected override Vector3Int ChaseTargetCell()
    {
        var pinkTarget = NavigationHelper.Instance.GetPinkTargetPosition();

        Vector3 redGhostToPinkTarget = pinkTarget - NavigationHelper.Instance.GetRedGhostPosition();

        var targetPosition = pinkTarget + redGhostToPinkTarget;

        return NavigationHelper.Instance.GetCellOnBoard(targetPosition);
    }
}
