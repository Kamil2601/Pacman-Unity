using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{
    private Quaternion rightRotation = Quaternion.Euler(0,0,0);
    private Quaternion leftRotation = Quaternion.Euler(0,0,180);
    private Quaternion upRotation = Quaternion.Euler(0,0,90);
    private Quaternion downRotation = Quaternion.Euler(0,0,270);

    private bool death = false;

    public override void ResetState()
    {
        death = false;
        transform.position = startingPosition;
        currentDirection = Vector2.right;
        nextDirection = Vector2.zero;
        animator.SetTrigger("Switch");
        speed = Speed.Chase;
    }

    protected override void Start()
    {
        base.Start();
        speed = Speed.Chase;
        GameManager.Instance.AddPlayer(this);
    }

    protected override void SetNextDirection()
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

    protected override void SetAnimation()
    {
        if (death)
            return;

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
        var ghost = other.GetComponentInParent<Ghost>();

        if (!ghost)
            return;

        if (ghost.Mode != Mode.Frightened && ghost.Mode != Mode.Eaten)
        {
            Die();
            GameManager.Instance.StopAll();
        }
    }

    private void Die()
    {
        transform.rotation = rightRotation;
        death = true;
        animator.SetTrigger("Switch");
    }
}
