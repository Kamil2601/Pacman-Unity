using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeGhost : Ghost
{
    protected override Vector3Int ChaseTargetCell()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, NavigationHelper.Instance.GetPlayerPosition());

        if (distanceToPlayer < 4)
        {
            return scatterTargetCell;
        }
        else
        {
            return NavigationHelper.Instance.GetPlayerCell();
        }
    }
}
