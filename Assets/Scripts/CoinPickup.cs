using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsPerGoldCoin = 100;

    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            FindObjectOfType<GameSession>().AddToScore(pointsPerGoldCoin);
            FindObjectOfType<AudioSource>().PlayOneShot(coinPickupSFX);
            //AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            wasCollected = true;
            Destroy(gameObject);
        }
    }
}
