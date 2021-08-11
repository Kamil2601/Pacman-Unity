using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject output;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.transform.position = output.transform.position;
    }
}
