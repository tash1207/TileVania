using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] AudioClip deathSFX;

    bool isPlayingDeathSFX = false;

    Rigidbody2D rb2d;
    PlayerMovement player;
    float bulletVelocity;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        bulletVelocity = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        rb2d.velocity = new Vector2(bulletVelocity, 0f);
    }

    void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && !isPlayingDeathSFX)
        {
            // Don't include the start scene enemy in the enemy run.
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                EnemyRunManager.instance.KillEnemy();
            }
            isPlayingDeathSFX = true;
            SoundFXManager.instance.PlaySoundFXClip(deathSFX, 1f);
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}
