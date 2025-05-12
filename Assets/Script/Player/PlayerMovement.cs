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

public abstract class PlayerMovement : MonoBehaviour
{
    public Animator anim;

    public GunShot gunShot;
    protected bool isShooting = false;

    public float CurrentHealth { get; protected set; } = MAX_HEALTH;
    public const float MAX_HEALTH = 100;

    public ParticleSystem heartGainParticle;

    [Header("Audio")]
    public AudioSource heartSound;
    public AudioSource jumpSound;

    // Roll
    protected bool isRolling = false;

    public bool IsAlive { get; protected set; } = true;

    protected const string ANIMATION_KEY = "Animation";

    protected virtual void Update() { }

    // private Vector3 GetInputDirection()
    // {
    //     float h = Input.GetAxis("Horizontal");
    //     float v = Input.GetAxis("Vertical");

    //     Vector3 camForward = mainCamera.transform.forward;
    //     Vector3 camRight = mainCamera.transform.right;
    //     camForward.y = 0f;
    //     camRight.y = 0f;
    //     camForward.Normalize();
    //     camRight.Normalize();

    //     return (camForward * v + camRight * h).normalized;
    // }

    // private void HandleMovement()
    // {
    // }

    // private void PlayerRotation()
    // {
    //     if (isShooting == false) return;
    //     transform.rotation = Quaternion.Euler(0, mainCamera.transform.rotation.eulerAngles.y, 0);
    // }

    // private IEnumerator PerformRoll()
    // {
    //     StartCoroutine(RollCooldown());

    //     isRolling = true;

    //     jumpSound.Play();

    //     Vector3 moveDirection = GetInputDirection();

    //     // Default forward if no input
    //     if (moveDirection == Vector3.zero)
    //     {
    //         moveDirection = transform.forward;
    //     }

    //     // Face the roll direction
    //     transform.rotation = Quaternion.LookRotation(moveDirection);

    //     anim.SetInteger(ANIMATION_KEY, (int)PlayerAnimation.Roll);

    //     float elapsed = 0f;
    //     while (elapsed < ROLL_DURATION)
    //     {
    //         transform.Translate(moveDirection * Time.deltaTime * ROLL_FORCE, Space.World);
    //         elapsed += Time.deltaTime;
    //         yield return null;
    //     }

    //     isRolling = false;
    // }

    // private IEnumerator RollCooldown()
    // {
    //     canRoll = false;
    //     yield return new WaitForSeconds(ROLL_COOLDOWN);
    //     canRoll = true;
    // }

    // private void HandleShooting()
    // {
    //     if(Input.GetKey(KeyCode.Mouse0) && !isRolling)
    //     {
    //         isShooting = true;
    //         gunShot.Shoot();
    //     }
    //     else
    //     {
    //         isShooting = false;
    //     }
    // }

    // private void CheckStatus()
    // {
    //     if (gameObject.transform.position.y < -30 || CurrentHealth <= 0)
    //     {
    //         Death();
    //     }
    // }

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


    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         isGround = true;
    //         if (!isRolling) anim.SetInteger(ANIMATION_KEY, (int)PlayerAnimation.Idle);
    //         anim.SetFloat("AnimSpeed", 1f);
    //     }
    // }
    
    // private void OnCollisionExit(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         isGround = false;
    //         anim.SetBool("Right", !anim.GetBool("Right"));
    //         anim.SetInteger(ANIMATION_KEY, (int)PlayerAnimation.Jump);
    //         anim.SetFloat("AnimSpeed", 0.4f);
    //     }
    // }

    public void ApplyKnockback(Collision collision, int force)
    {
    }


    private void Death()
    {
        IsAlive = false;
        GameManager.Instance.GameOver();
    }
}
