using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] AudioClip deathSFX;

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
        if (other.tag == "Enemy")
        {
            SoundFXManager.instance.PlaySoundFXClip(deathSFX, 1f);
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}
