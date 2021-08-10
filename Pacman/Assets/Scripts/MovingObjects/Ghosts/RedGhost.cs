using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGhost : Ghost
{
    protected override Vector3Int TargetCell()
    {
        return NavigationHelper.instance.GetPlayerCell();
    }
}
