using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float error = 0.05f;

    private new Rigidbody2D rigidbody2D;
    protected Animator animator;
    protected Vector2 currentDirection = Vector2.right;
    protected Vector2 nextDirection;

    public Vector2 CurrentDirection { get => currentDirection; }

    // Start is called before the first frame update
    protected void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected void FixedUpdate()
    {
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
                RaycastHit2D hit = Physics2D.Raycast(transform.position, nextDirection, 1f);

                if (!hit)
                {
                    RoundPosition();
                    currentDirection = nextDirection;
                    nextDirection = Vector2.zero;
                }
            }
        }

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

    private void RoundPosition()
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
