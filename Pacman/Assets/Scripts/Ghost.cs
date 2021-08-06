using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MovingObject
{
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
}
