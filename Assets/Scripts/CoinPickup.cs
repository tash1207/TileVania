using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsPerGoldCoin = 100;

    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            // Don't include the start scene coin in the coin run.
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                CoinRunManager.instance.CollectCoin();
            }
            FindObjectOfType<GameSession>().AddToScore(pointsPerGoldCoin);
            SoundFXManager.instance.PlaySoundFXClip(coinPickupSFX, 1f);
            wasCollected = true;
            Destroy(gameObject);
        }
    }
}
