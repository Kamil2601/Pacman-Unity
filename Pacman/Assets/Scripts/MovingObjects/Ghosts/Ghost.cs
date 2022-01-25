using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Ghost : MovingObject
{
    enum AnimationState
    {
        Normal,
        Frightened,
        Blinking,
        Eaten,
    }

    private static readonly List<Vector2> directions = new List<Vector2> { Vector2.up, Vector2.left, Vector2.down, Vector2.right };
    private static readonly Dictionary<Vector2, int> priority = new Dictionary<Vector2, int> {
        {Vector2.up, 4},
        {Vector2.left, 3},
        {Vector2.down, 2},
        {Vector2.right, 1}
    };


    [SerializeField] private GameObject scatterTarget;
    [SerializeField] private float waitingTime;
    
    private Vector3Int lastCell;
    
    private Mode mode;
    protected Vector3Int scatterTargetCell;

    public Mode Mode { get => mode; }

    private Coroutine frightenedCoroutine;
    private BoxCollider2D boxCollider;

    private bool fixedMove = true;
    private Vector3Int fixedMoveTarget;

    protected override void Start()
    {
        base.Start();
        boxCollider = GetComponent<BoxCollider2D>();
        GameManager.Instance.AddGhost(this);
        scatterTargetCell = NavigationHelper.Instance.GetCellOnBoard(scatterTarget.transform);

        ResetState();
    }


    public override void ResetState()
    {
        StopAllCoroutines();
        transform.position = startingPosition;

        mode = Mode.Scatter;
        speed = Speed.Chase;
        lastCell = scatterTargetCell;

        animator.SetInteger("State", (int)AnimationState.Normal);

        if (waitingTime > 0)
            StartCoroutine(WaitInBox(waitingTime));
    }

    public void BoxCenterTriggered()
    {
        if (fixedMove)
        {
            fixedMoveTarget = NavigationHelper.Instance.BoxGate;
            boxCollider.enabled = false;
            RoundPosition();
            currentDirection = Vector2.up;

            if (mode == Mode.Eaten)
            {
                animator.SetInteger("State", (int)AnimationState.Normal);
                mode = Mode.Scatter;
            }
        }
    }

    public void BoxGateTriggered()
    {
        if (fixedMove)
        {
            fixedMove = false;
            boxCollider.enabled = true;
            if (mode == Mode.Frightened)
                speed = Speed.Frightened;
            else
                speed = Speed.Chase;
        }
        else if (mode == Mode.Eaten)
        {
            fixedMove = true;
            fixedMoveTarget = NavigationHelper.Instance.BoxCenter;
            boxCollider.enabled = false;
            RoundPosition();
            currentDirection = Vector2.down;
        }
    }

    public void SetFrightened(float seconds = 20)
    {
        animator.SetInteger("State", (int)AnimationState.Frightened);
        if (fixedMove)
        {
            mode = Mode.Frightened;
        }
        else if (mode == Mode.Scatter || mode == Mode.Chase)
        {
            mode = Mode.Frightened;
            speed = Speed.Frightened;
            animator.SetInteger("State", (int)AnimationState.Frightened);

            currentDirection = -currentDirection;
            ForceSetNextDirection();
            lastCell = scatterTargetCell;
        }
        else if (mode == Mode.Frightened)
        {
            StopCoroutine(frightenedCoroutine);
        }

        frightenedCoroutine = StartCoroutine(FrightenedCoroutine(seconds));
    }

    private IEnumerator FrightenedCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds - 2);

        animator.SetInteger("State", (int)AnimationState.Blinking);

        yield return new WaitForSeconds(2);
        
        mode = Mode.Chase;

        if (!fixedMove)
        {
            speed = Speed.Chase;
        }
        animator.SetInteger("State", (int)AnimationState.Normal);
    }

    private IEnumerator WaitInBox(float seconds)
    {
        fixedMove = true;
        fixedMoveTarget = NavigationHelper.Instance.BoxCenter;
        speed = Speed.BoxWait;

        yield return new WaitForSeconds(seconds);

        speed = Speed.BoxLeave;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();

        if (!player)
            return;

        if (this.Mode == Mode.Frightened)
        {
            Die();
        }
    }

    private void Die()
    {
        StopCoroutine(frightenedCoroutine);
        mode = Mode.Eaten;
        speed = Speed.Eaten;
        animator.SetInteger("State", (int)AnimationState.Eaten);
    }

    #region no mode/fixedMove change

    protected override void SetNextDirection()
    { 
        if (IsCloseToCellCenter() && lastCell != currentCell)
        {
            ForceSetNextDirection();
        }
    }

    private void ForceSetNextDirection()
    {
        var legalDirections = directions.Where(dir => IsLegalDirection(dir))
                .OrderBy(dir => DistanceToTarget(dir))
                .ThenByDescending(dir => priority[dir])
                .ToList();

        if (mode == Mode.Frightened && !fixedMove)
        {
            int index = Random.Range(0, legalDirections.Count);
            nextDirection = legalDirections[index];
        }
        else
            nextDirection = legalDirections.FirstOrDefault();

        lastCell = currentCell;
    }

    private bool IsLegalDirection(Vector2 direction)
    {
        if (fixedMove)
            return true;

        if (direction == -currentDirection)
        {
            return false;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, raycastMask);

        return !hit;
    }

    private float DistanceToTarget(Vector3 direction)
    {
        var newPosition = transform.position + direction;
        var newCell = NavigationHelper.Instance.GetCellOnBoard(newPosition);
        var dist = Vector3.Distance(newCell, TargetCell());

        return dist;
    }

    private Vector3Int TargetCell()
    {
        if (fixedMove)
            return fixedMoveTarget;
        if (mode == Mode.Scatter)
            return scatterTargetCell;
        else if (mode == Mode.Eaten)
            return NavigationHelper.Instance.BoxGate;
        else
            return ChaseTargetCell();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ForceSetNextDirection();
        RoundPosition();
        SetCurrentDirection();
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

    protected abstract Vector3Int ChaseTargetCell();

    # endregion
}
