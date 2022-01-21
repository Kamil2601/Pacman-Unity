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
    private Vector3Int scatterTargetCell;

    public Mode Mode { get => mode; }

    private Coroutine frightenedCoroutine;
    private BoxCollider2D boxCollider;

    public void SetFrightened(float seconds = 10)
    {
        if (mode == Mode.Scatter || mode == Mode.Chase)
        {
            mode = Mode.Frightened;
            speed = Speed.Frightened;
            animator.SetInteger("State", (int)AnimationState.Frightened);

            currentDirection = -currentDirection;
            ForceSetNextDirection();
            lastCell = scatterTargetCell;

            frightenedCoroutine = StartCoroutine(FrightenedCoroutine(seconds));
        }
        else if (mode == Mode.Frightened)
        {
            StopCoroutine(frightenedCoroutine);
            frightenedCoroutine = StartCoroutine(FrightenedCoroutine(seconds));
        }
    }

    public void BoxCenterTriggered()
    {
        if (mode == Mode.InBox || mode == Mode.EnteringBox)
        {
            Debug.Log("Box center reached");
            mode = Mode.LeavingBox;
            // speed = Speed.BoxLeave;
            boxCollider.enabled = false;
            RoundPosition();
            currentDirection = Vector2.up;
            animator.SetInteger("State", (int)AnimationState.Normal);
        }
    }

    public void BoxGateTriggered()
    {
        if (mode == Mode.LeavingBox)
        {
            boxCollider.enabled = true;
            mode = Mode.Scatter;
            speed = Speed.Chase;      
        }
        else if (mode == Mode.Eaten)
        {
            mode = Mode.EnteringBox;
            boxCollider.enabled = false;
            RoundPosition();
            currentDirection = Vector2.down;
        }

        
    }

    private IEnumerator FrightenedCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds - 2);

        animator.SetInteger("State", (int)AnimationState.Blinking);

        yield return new WaitForSeconds(2);

        mode = Mode.Chase;
        speed = Speed.Chase;
        animator.SetInteger("State", (int)AnimationState.Normal);
    }

    private IEnumerator WaitInBox(float seconds)
    {
        mode = Mode.InBox;
        speed = Speed.BoxWait;

        yield return new WaitForSeconds(seconds);

        speed = Speed.BoxLeave;
    }

    protected override void Start()
    {
        base.Start();
        boxCollider = GetComponent<BoxCollider2D>();
        GameManager.Instance.AddGhost(this);
        scatterTargetCell = NavigationHelper.Instance.GetCellOnBoard(scatterTarget.transform);
        // currentDirection = Vector2.right;
        speed = Speed.Chase;

        if (waitingTime > 0)
            StartCoroutine(WaitInBox(waitingTime));
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
        if (IsCloseToCellCenter() && lastCell != currentCell)
        {
            ForceSetNextDirection();
        }
    }

    private void ForceSetNextDirection()
    {
        var legalDirections = directions.Where(dir => IsLegalDirection(dir))
                .OrderBy(dir => DistanceToTarget(dir))
                .ThenByDescending(dir => priority[dir]);

        if (mode == Mode.Frightened)
            nextDirection = legalDirections.LastOrDefault();
        else
            nextDirection = legalDirections.FirstOrDefault();

        lastCell = currentCell;
    }

    private bool IsLegalDirection(Vector2 direction)
    {
        if (mode == Mode.InBox || mode == Mode.LeavingBox || mode == Mode.EnteringBox)
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
        if (mode == Mode.Scatter)
            return scatterTargetCell;
        else if (mode == Mode.Chase || mode == Mode.Frightened)
            return ChaseTargetCell();
        else if (mode == Mode.InBox || mode == Mode.EnteringBox)
            return NavigationHelper.Instance.BoxCenter;
        else if (mode == Mode.Eaten || mode == Mode.LeavingBox)
            return NavigationHelper.Instance.BoxGate;
        else
            return ChaseTargetCell();
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        ForceSetNextDirection();
        RoundPosition();
        SetCurrentDirection();
    }

    private void Die()
    {
        StopAllCoroutines();
        mode = Mode.Eaten;
        speed = Speed.Eaten;
        animator.SetInteger("State", (int)AnimationState.Eaten);
    }

    protected abstract Vector3Int ChaseTargetCell();
}
