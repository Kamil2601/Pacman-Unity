using System;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    protected float speed;
    private float error = 0.1f;
    protected LayerMask raycastMask;

    protected Vector3Int currentCell;

    private new Rigidbody2D rigidbody2D;
    protected Animator animator;

    protected Vector2 currentDirection = Vector2.right;
    protected Vector2 nextDirection;

    public Vector2 CurrentDirection { get => currentDirection; }

    protected Vector3 startingPosition;


    public abstract void ResetState();

    public void Stop()
    {
        speed = 0;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        raycastMask = LayerMask.GetMask("Walls");
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();


        startingPosition = transform.position;
    }

    protected virtual void FixedUpdate()
    {
        currentCell = NavigationHelper.Instance.GetCellOnBoard(transform.position);
        SetNextDirection();
        SetCurrentDirection();    
    }

    protected abstract void SetNextDirection();

    protected void SetCurrentDirection()
    {
        if (currentDirection == -nextDirection)
        {
            currentDirection = nextDirection;
            nextDirection = Vector2.zero;
        }
        else if (IsDirectionAxisChange())
        {
            if (IsCloseToCellCenter())
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, nextDirection, 1f, raycastMask);

                if (!hit)
                {
                    RoundPosition();
                    currentDirection = nextDirection;
                    nextDirection = Vector2.zero;
                }
            }
        }

        // currentDirection = nextDirection;

        SetAnimation();
        SetVelocity();
    }


    protected bool IsCloseToCellCenter()
    {
        var horizontalFraction = Math.Abs(transform.position.x - Mathf.Round(transform.position.x));
        var verticalFraction = Math.Abs(transform.position.y - Mathf.Round(transform.position.y));

        return horizontalFraction < error && verticalFraction < error;
    }

    private bool IsDirectionAxisChange()
    {
        if (IsHorizontal(currentDirection))
            return IsVertical(nextDirection);
        else if (IsVertical(currentDirection))
            return IsHorizontal(nextDirection);

        return false;
    }

    private bool IsHorizontal(Vector2 direction)
    {
        return Math.Abs(direction.x) == 1f && Math.Abs(direction.y) == 0f;;
    }
    
    private bool IsVertical(Vector2 direction)
    {
        return Math.Abs(direction.y) == 1f && Math.Abs(direction.x) == 0f;;
    }

    protected void RoundPosition()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
    }

    private void SetVelocity()
    {
        rigidbody2D.velocity = currentDirection * speed;
    }

    // public void SetPosition()

    protected abstract void SetAnimation();
    
}
