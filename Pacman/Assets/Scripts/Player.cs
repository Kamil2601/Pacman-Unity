using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject leftPortal;
    [SerializeField] private GameObject rightPortal;
    [SerializeField] private float speed;
    [SerializeField] private float error = 0.05f;

    private new Rigidbody2D rigidbody2D;
    private Animator animator;
    private Vector2 currentDirection = Vector2.right;
    private Vector2 nextDirection;
    private Quaternion rightRotation = Quaternion.Euler(0,0,0);
    private Quaternion leftRotation = Quaternion.Euler(0,0,180);
    private Quaternion upRotation = Quaternion.Euler(0,0,90);
    private Quaternion downRotation = Quaternion.Euler(0,0,270);

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("Move", true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        DetectInput();
        ChangeDirection();    
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

        SetRotation();
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

    private void SetRotation()
    {
        if (currentDirection == Vector2.down)
            transform.rotation = downRotation;
        else if (currentDirection == Vector2.up)
            transform.rotation = upRotation;
        else if (currentDirection == Vector2.left)
            transform.rotation = leftRotation;
        else if (currentDirection == Vector2.right)
            transform.rotation = rightRotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == leftPortal && currentDirection == Vector2.left)
        {
            transform.position = rightPortal.transform.position;
        }
        else if (other.gameObject == rightPortal && currentDirection == Vector2.right)
        {
            transform.position = leftPortal.transform.position;
        }
    }
}
