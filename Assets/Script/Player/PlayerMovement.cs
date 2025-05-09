using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerAnimation
{
    Idle,
    Walk,
    Backward,
    Jump,
    Roll
}


public enum PlayerStatus
{
    Active,
    Hit,
    Dead
}

public class PlayerMovement : MonoBehaviour
{
    private Camera mainCamera;

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

    // Roll
    private bool isRolling = false;
    private Coroutine rollCoroutine;
    private bool canRoll = true;
    private const float ROLL_COOLDOWN = 1.2f;
    private const float ROLL_DURATION = 0.6f;
    private const float ROLL_FORCE = 18f;

    public bool IsAlive { get; private set; } = true;

    private const string ANIMATION_KEY = "Animation";
    private const int MOVEMENT_SPEED = 7;
    private const int JUMP_FORCE = 20;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (IsAlive == false) return;

        PlayerRotation();

        HandleMovement();

        HandleShooting();

        CheckStatus();
    }

    private void HandleMovement()
    {
        if (isRolling || !IsAlive) return;

        if (isGround)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && canRoll)
            {
                rollCoroutine = StartCoroutine(PerformRoll());
            }
            else if (Input.GetKey(KeyCode.S))
            {
                anim.SetInteger(ANIMATION_KEY, (int)PlayerAnimation.Backward);
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                anim.SetInteger(ANIMATION_KEY, (int)PlayerAnimation.Walk);
            }
            else
            {
                anim.SetInteger(ANIMATION_KEY, (int)PlayerAnimation.Idle);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            jumpSound.Play();
            rb.AddForce(new Vector3(0, JUMP_FORCE, 0), ForceMode.Impulse);
            isGround = false;
        }

        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * MOVEMENT_SPEED, 0, Input.GetAxis("Vertical") * Time.deltaTime * MOVEMENT_SPEED);
    }

    private void PlayerRotation()
    {
        if (isRolling == true) return;
        transform.rotation = Quaternion.Euler(0, mainCamera.transform.rotation.eulerAngles.y, 0);
    }

    private IEnumerator PerformRoll()
    {
        StartCoroutine(RollCooldown());

        isRolling = true;

        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (inputDirection == Vector3.zero)
        {
            inputDirection = Vector3.forward; // Default forward if no input
        }

        Vector3 rollDirection = transform.TransformDirection(inputDirection);

        // Face the roll direction
        transform.rotation = Quaternion.LookRotation(rollDirection);

        anim.SetInteger(ANIMATION_KEY, (int)PlayerAnimation.Roll);

        float elapsed = 0f;
        while (elapsed < ROLL_DURATION)
        {
            transform.Translate(rollDirection * Time.deltaTime * ROLL_FORCE, Space.World);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isRolling = false;
    }

    private IEnumerator RollCooldown()
    {
        canRoll = false;
        yield return new WaitForSeconds(ROLL_COOLDOWN);
        canRoll = true;
    }

    private void HandleShooting()
    {
       if (Input.GetKey(KeyCode.Mouse0) && !isRolling) gunShot.Shoot();
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
            if (!isRolling) anim.SetInteger(ANIMATION_KEY, (int)PlayerAnimation.Idle);
            anim.SetFloat("AnimSpeed", 1f);
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
            anim.SetBool("Right", !anim.GetBool("Right"));
            anim.SetInteger(ANIMATION_KEY, (int)PlayerAnimation.Jump);
            anim.SetFloat("AnimSpeed", 0.4f);
        }
    }

    public void ApplyKnockback(Collision collision, int force)
    {
        if (isRolling == true && rollCoroutine != null)
        {
            StopCoroutine(rollCoroutine);
            isRolling = false;
            anim.SetInteger(ANIMATION_KEY, (int)PlayerAnimation.Idle);
        }

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
