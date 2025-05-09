using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;

    public GunShot gunShot;

    public bool ground = true;
    public bool right = false;
    private bool moveable;

    public HealthBar healthBar;
    public float CurrentHealth { get; private set; } = MAX_HEALTH;
    private const float MAX_HEALTH = 100;

    public GameObject heart;
    public ParticleSystem heartGain;
    public ParticleSystem heartCircle;
    public int heartScore;

    [Header("Audio")]
    public AudioSource heartSound;
    public AudioSource jumpSound;
    public AudioSource robotHitSound;
    public AudioSource spikeHitSound;

    public bool IsAlive { get; private set; } = true;

    private const string ANIMATION_KEY = "AnimationPar";
    private const int MOVEMENT_SPEED = 7;
    private const int JUMP_FORCE = 20;

    private void Update()
    {
        if (IsAlive == false) return;

        healthBar.SetHealth(CurrentHealth);

        HandleMovement();

        HandleShooting();

        //PassiveDamage();
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

        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * MOVEMENT_SPEED, 0, Input.GetAxis("Vertical") * Time.deltaTime * MOVEMENT_SPEED);
    }
    private void HandleShooting()
    {
       if (Input.GetKey(KeyCode.Mouse0)) gunShot.Shoot();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && ground)
        {
            
            ground = false;
            // jumpSound.Play();
            rb.AddForce(new Vector3(0, JUMP_FORCE, 0), ForceMode.Impulse);
        }

        if (gameObject.transform.position.y < -30 || CurrentHealth <= 0)
        {
            Death();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Robot"))
        {
            ground = true;
            anim.SetBool("Air", false);
            anim.SetFloat("AnimSpeed", 1f);
            anim.SetBool("Right", right);
            right = !right;
        }

        if (collision.gameObject.CompareTag("Spike"))
        {
            spikeHitSound.Play();
            CurrentHealth -= 10f;
            ApplyKnockback(collision, 500);
        }

        if (collision.gameObject.CompareTag("Robot"))
        {
            robotHitSound.Play();
            CurrentHealth -= 30f;
            ApplyKnockback(collision, 2000);
        }
    }

    private void ApplyKnockback(Collision collision, int force)
    {
        float dirx = collision.contacts[0].point.x - transform.position.x;
        float dirz = collision.contacts[0].point.z - transform.position.z;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(-dirx * force, 0, -dirz * force);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ground = false;
            anim.SetBool("Air", true);
            anim.SetFloat("AnimSpeed", 0.4f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Heart"))
        {
            heartSound.Play();
            heartScore += 1;
            CurrentHealth = Math.Min(MAX_HEALTH, CurrentHealth + 50);

            heart.transform.position = new Vector3(UnityEngine.Random.Range(-32.5f, 32.5f), 7f, UnityEngine.Random.Range(10f, 75f));
            heartGain.Emit(10);
            heartCircle.Emit(1);
        }
    }

    // void PassiveDamage()
    // {
    //     if (Time.time > nextDamage)
    //     {
    //         nextDamage = Time.time + 0.005f;
    //         CurrentHealth -= 0.01f;
    //     }
    // }

    private void Death()
    {
        IsAlive = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        Time.timeScale = 0;
        GameManager.Instance.GameOver();
    }

    void Freeze()
    {
        moveable = false;
    }

}
