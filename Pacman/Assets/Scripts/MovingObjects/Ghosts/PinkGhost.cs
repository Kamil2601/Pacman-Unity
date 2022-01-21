using UnityEngine;

public class PinkGhost : Ghost
{
    protected override Vector3Int ChaseTargetCell()
    {
        return NavigationHelper.Instance.GetPinkTargetCell();
    }
}
