using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPlayerMovement : PlayerMovement
{
    private Camera mainCamera;
    public Rigidbody rb;

    private bool isGround = true;

    // Roll
    private Coroutine rollCoroutine;
    private bool canRoll = true;
    private const float ROLL_COOLDOWN = 1f;
    private const float ROLL_DURATION = 0.5f;
    private const float ROLL_FORCE = 18f;

    private const int MOVEMENT_SPEED = 7;
    private const int JUMP_FORCE = 20;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    protected override void Update()
    {
        if (IsAlive == false || GameManager.Instance.IsPaused == true) return;

        PlayerRotation();

        HandleMovement();

        HandleShooting();

        CheckStatus();
    }

    private Vector3 GetInputDirection()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        return (camForward * v + camRight * h).normalized;
    }

    private void HandleMovement()
    {
        if (isRolling || !IsAlive) return;
        
        Vector3 moveDirection = GetInputDirection();

        // Play animations
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
            else if (moveDirection != Vector3.zero)
            {
                anim.SetInteger(ANIMATION_KEY, (int)PlayerAnimation.Walk);
            }
            else
            {
                anim.SetInteger(ANIMATION_KEY, (int)PlayerAnimation.Idle);
            }
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            jumpSound.Play();
            rb.AddForce(new Vector3(0, JUMP_FORCE, 0), ForceMode.Impulse);
            isGround = false;
        }

        transform.Translate(moveDirection * Time.deltaTime * MOVEMENT_SPEED, Space.World);

        if (moveDirection != Vector3.zero && !isShooting)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    private void PlayerRotation()
    {
        if (isShooting == false) return;
        transform.rotation = Quaternion.Euler(0, mainCamera.transform.rotation.eulerAngles.y, 0);
    }

    private IEnumerator PerformRoll()
    {
        StartCoroutine(RollCooldown());

        isRolling = true;

        jumpSound.Play();

        Vector3 moveDirection = GetInputDirection();

        // Default forward if no input
        if (moveDirection == Vector3.zero)
        {
            moveDirection = transform.forward;
        }

        // Face the roll direction
        transform.rotation = Quaternion.LookRotation(moveDirection);

        anim.SetInteger(ANIMATION_KEY, (int)PlayerAnimation.Roll);

        float elapsed = 0f;
        while (elapsed < ROLL_DURATION)
        {
            transform.Translate(moveDirection * Time.deltaTime * ROLL_FORCE, Space.World);
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
        if(Input.GetKey(KeyCode.Mouse0) && !isRolling)
        {
            isShooting = true;
            gunShot.Shoot();
        }
        else
        {
            isShooting = false;
        }
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
