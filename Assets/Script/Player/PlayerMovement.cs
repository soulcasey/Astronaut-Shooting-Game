using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerStatus
{
    Active,
    Hit,
    Dead
}

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;

    public GunShot gunShot;

    private bool isGround = true;

    public float CurrentHealth { get; private set; } = MAX_HEALTH;
    public const float MAX_HEALTH = 100;

    public ParticleSystem heartGainParticle;

    [Header("Audio")]
    public AudioSource heartSound;
    public AudioSource jumpSound;

    public bool IsAlive { get; private set; } = true;

    private const string ANIMATION_KEY = "AnimationPar";
    private const int MOVEMENT_SPEED = 7;
    private const int JUMP_FORCE = 20;

    private void Update()
    {
        if (IsAlive == false) return;

        HandleMovement();

        HandleShooting();

        CheckStatus();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.S))
        {
            anim.SetInteger(ANIMATION_KEY, 2);
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            anim.SetInteger(ANIMATION_KEY, 1);
        }
        else
        {
            anim.SetInteger(ANIMATION_KEY, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            jumpSound.Play();
            rb.AddForce(new Vector3(0, JUMP_FORCE, 0), ForceMode.Impulse);
            isGround = false;
        }

        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * MOVEMENT_SPEED, 0, Input.GetAxis("Vertical") * Time.deltaTime * MOVEMENT_SPEED);
    }
    private void HandleShooting()
    {
       if (Input.GetKey(KeyCode.Mouse0)) gunShot.Shoot();
    }

    private void CheckStatus()
    {
        if (gameObject.transform.position.y < -30 || CurrentHealth <= 0)
        {
            Death();
        }
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth = Math.Max(0, CurrentHealth - amount);
        if (CurrentHealth < 0)
        {
            Death();
        }
    }

    public void Heal(int amount)
    {
        CurrentHealth = Math.Min(MAX_HEALTH, CurrentHealth + amount);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            anim.SetBool("Air", false);
            anim.SetFloat("AnimSpeed", 1f);
            anim.SetBool("Right", !anim.GetBool("Right"));
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
            anim.SetBool("Air", true);
            anim.SetFloat("AnimSpeed", 0.4f);
        }
    }

    public void ApplyKnockback(Collision collision, int force)
    {
        float dirx = collision.contacts[0].point.x - transform.position.x;
        float dirz = collision.contacts[0].point.z - transform.position.z;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(-dirx * force, 0, -dirz * force);
    }
    private void Death()
    {
        IsAlive = false;
        GameManager.Instance.GameOver();
    }
}
