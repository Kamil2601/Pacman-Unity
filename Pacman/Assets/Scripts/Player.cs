using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public Vector2 currentDirection = Vector2.left;
    public Vector2 nextDirection;
    public float speed = 1.0f;
    public float error = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        // Vector2.down;
    }

    // Update is called once per frame
    void Update()
    {
        DetectInput();
        ChangeDirection();
        
        transform.Translate(currentDirection * speed * Time.deltaTime);
    }

    private void DetectInput()
    {
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal < 0)
            nextDirection = Vector2.left;
        else if (horizontal > 0)
            nextDirection = Vector2.right;
        else if (vertical < 0)
            nextDirection = Vector2.down;
        else if (vertical > 0)
            nextDirection = Vector2.up;
    }

    private void ChangeDirection()
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
                transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, 0);
                currentDirection = nextDirection;
                nextDirection = Vector2.zero;
            }
        }
        else if (IsVertical(currentDirection) && IsHorizontal(nextDirection))
        {
            var fraction = Math.Abs(transform.position.y - Mathf.Round(transform.position.y));

            if (fraction < error)
            {
                transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), 0);
                currentDirection = nextDirection;
                nextDirection = Vector2.zero;
            }
        }
    }

    private bool IsHorizontal(Vector2 direction)
    {
        return Math.Abs(direction.x) == 1f && Math.Abs(direction.y) == 0f;;
    }
    
    private bool IsVertical(Vector2 direction)
    {
        return Math.Abs(direction.y) == 1f && Math.Abs(direction.x) == 0f;;
    }

    private void SetRotation()
    {
        if (currentDirection == Vector2.down)
        {
            // transform.rotation == Vector3
        }
    }
}
