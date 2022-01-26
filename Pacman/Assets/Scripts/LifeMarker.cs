using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeMarker : MonoBehaviour
{
    [SerializeField] private int livesLeftToDisable;

    private void OnEnable()
    {
        Player.playerDeath += Disable;
    }

    private void OnDisable()
    {
        Player.playerDeath -= Disable;    
    }

    private void Disable(int livesLeft)
    {
        if (livesLeft == livesLeftToDisable)
        {
            gameObject.SetActive(false);
        }
    }
}
