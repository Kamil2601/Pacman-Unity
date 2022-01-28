using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField] private AudioClip playerDeath;
    [SerializeField] private AudioClip eatGhost;

    private float lastDotEatenTime = -2f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayDelayed(4f);
    }

    private void OnEnable()
    {
        Player.playerDeath += PlayPlayerDeath;
        Ghost.ghostEaten += PlayEatGhost;
    }

    private void OnDisable()
    {
        Player.playerDeath -= PlayPlayerDeath;
        Ghost.ghostEaten -= PlayEatGhost;
    }

    private void PlayEatGhost()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(eatGhost);
        audioSource.PlayDelayed(1f);
    }

    private void PlayPlayerDeath(int livesLeft)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(playerDeath);

        if (livesLeft > 0)
            audioSource.PlayDelayed(3f);
    }
}
