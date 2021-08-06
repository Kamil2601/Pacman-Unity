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

    // Start is called before the first frame update
    protected void Start()
    {
        Debug.Log("Start" +  gameObject.name);
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // animator.SetBool("Move", true);
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
        else if (IsHorizontal(currentDirection) && IsVertical(nextDirection))
        {
            var fraction = Math.Abs(transform.position.x - Mathf.Round(transform.position.x));

            if (fraction < error)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, nextDirection, 1f);

                if (hit.collider == null)
                {
                    RoundXPosition();
                    currentDirection = nextDirection;
                    nextDirection = Vector2.zero;
                }    
            }
        }
        else if (IsVertical(currentDirection) && IsHorizontal(nextDirection))
        {
            var fraction = Math.Abs(transform.position.y - Mathf.Round(transform.position.y));

            if (fraction < error)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, nextDirection, 1f);

                if (hit.collider == null)
                {
                    RoundYPosition();
                    currentDirection = nextDirection;
                    nextDirection = Vector2.zero;
                }
            }
        }

        SetAnimation();
        SetVelocity();
    }


    private bool IsHorizontal(Vector2 direction)
    {
        return Math.Abs(direction.x) == 1f && Math.Abs(direction.y) == 0f;;
    }
    
    private bool IsVertical(Vector2 direction)
    {
        return Math.Abs(direction.y) == 1f && Math.Abs(direction.x) == 0f;;
    }

    private void RoundXPosition()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, 0);
    }

    private void RoundYPosition()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), 0);
    }

    private void SetVelocity()
    {
        rigidbody2D.velocity = currentDirection * speed;
    }

    protected abstract void SetAnimation();
    
}
