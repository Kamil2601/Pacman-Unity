using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var ghost = GetComponentInParent<Ghost>();

        if (other.name == "BoxCenter")
        {
            ghost.BoxCenterTriggered();
        }
        else if (other.name == "BoxGate")
        {
            ghost.BoxGateTriggered();
        }
    }
}
