using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Ghost : MovingObject
{
    private static readonly List<Vector2> directions = new List<Vector2> { Vector2.up, Vector2.left, Vector2.down, Vector2.right };
    private static readonly Dictionary<Vector2, int> priority = new Dictionary<Vector2, int> {
        {Vector2.up, 4},
        {Vector2.left, 3},
        {Vector2.down, 2},
        {Vector2.right, 1}
    };
    
    private Vector3Int lastCell;
    private Mode mode;

    [SerializeField] private GameObject scatterTarget;
    private Vector3Int scatterTargetCell;

    protected override void Start()
    {
        base.Start();
        GameManager.instance.AddGhost(this);
        mode = Mode.Scatter;
        scatterTargetCell = NavigationHelper.instance.GetCellOnBoard(scatterTarget.transform);
    }

    protected override void SetAnimation()
    {
        if (currentDirection == Vector2.left)
            animator.SetInteger("Direction", (int)Direction.Left);
        else if (currentDirection == Vector2.right)
            animator.SetInteger("Direction", (int)Direction.Right);
        else if (currentDirection == Vector2.up)
            animator.SetInteger("Direction", (int)Direction.Up);
        else if (currentDirection == Vector2.down)
            animator.SetInteger("Direction", (int)Direction.Down);
    }

    protected override void SetNextDirection()
    { 
        var currentCell = NavigationHelper.instance.GetCellOnBoard(transform);

        if (IsCloseToCellCenter() && lastCell != currentCell)
        {
            var legalDirections = directions.Where(dir => IsLegalDirection(dir))
                .OrderBy(dir => DistanceToTarget(dir))
                .ThenByDescending(dir => priority[dir]);

            nextDirection = legalDirections.FirstOrDefault();

            lastCell = currentCell;
        }
    }

    private bool IsLegalDirection(Vector2 direction)
    {
        if (direction == -currentDirection)
        {
            return false;
        }
            
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f);

        return !hit;
    }

    private float DistanceToTarget(Vector3 direction)
    {
        var newPosition = transform.position + direction;
        var newCell = NavigationHelper.instance.GetCellOnBoard(newPosition);
        return Vector3.Distance(newCell, TargetCell());
    }

    private Vector3Int TargetCell()
    {
        switch (mode)
        {
            case Mode.Scatter:
                return scatterTargetCell;
            case Mode.Chase:
                return ChaseTargetCell();
        }

        return ChaseTargetCell();
    }

    protected abstract Vector3Int ChaseTargetCell();
}
