using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{
    static class Rotation
    {
        public static Quaternion Right { get; } = Quaternion.Euler(0,0,0);
        public static Quaternion Left { get; } = Quaternion.Euler(0,0,180);
        public static Quaternion Up { get; } = Quaternion.Euler(0,0,90);
        public static Quaternion Down { get; } = Quaternion.Euler(0,0,270);

    }

    public static event Action<int> playerDeath;

    private bool death = false;

    private int livesLeft = 3;

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
            transform.rotation = Rotation.Down;
        else if (currentDirection == Vector2.up)
            transform.rotation = Rotation.Up;
        else if (currentDirection == Vector2.left)
            transform.rotation = Rotation.Left;
        else if (currentDirection == Vector2.right)
            transform.rotation = Rotation.Right;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var ghost = other.GetComponentInParent<Ghost>();

        if (!ghost)
            return;

        if (ghost.Mode != Mode.Frightened && ghost.Mode != Mode.Eaten)
        {
            Die();
        }
    }

    private void Die()
    {
        livesLeft --;
        transform.rotation = Rotation.Right;
        death = true;
        animator.SetTrigger("Switch");

        playerDeath?.Invoke(livesLeft);
    }
}
