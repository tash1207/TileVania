using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    SpriteRenderer spriteRenderer;
    float gravityScaleAtStart;
    bool isAlive = true;

    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float groundJumpSpeed = 10f;
    [SerializeField] float waterJumpSpeed = 13f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(1f, 1f);

    [SerializeField] AudioClip bounceSFX;
    [SerializeField] AudioClip bulletSFX;
    [SerializeField] AudioClip deathSFX;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gravityScaleAtStart = rb2d.gravityScale;
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Bounce();
        Die();
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        SoundFXManager.instance.PlaySoundFXClip(bulletSFX, 1f);
        Instantiate(bullet, gun.position, bullet.transform.rotation);
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            jumpSpeed = waterJumpSpeed;
        }
        else {
            jumpSpeed = groundJumpSpeed;
        }

        if (value.isPressed)
        {
            rb2d.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rb2d.velocity.y);
        rb2d.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rb2d.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(rb2d.velocity.x, moveInput.y * climbSpeed);
        rb2d.velocity = climbVelocity;
        rb2d.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(rb2d.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Bounce()
    {
        // Might need to add a boool to ensure this only plays once
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Bouncing")))
        {
            SoundFXManager.instance.PlaySoundFXClip(bounceSFX, 1f);
        }
    }

    void Die()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            Death();
        }
    }

    public void Death()
    {
        Debug.Log("Death");
        SoundFXManager.instance.PlaySoundFXClip(deathSFX, 1f);
        isAlive = false;
        animator.SetTrigger("Dying");
        spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        rb2d.velocity = deathKick;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
