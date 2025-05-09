using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovement : MonoBehaviour, IDamageable
{
    private enum RobotState { Off, On, Idle, Chase }
    private RobotState currentState = RobotState.Off;

    public Transform target;
    public Animator anim;
    
    public AudioSource startSound;
    public AudioSource stopSound;
    public AudioSource attackSound;

    public float CurrentHealth { get; private set; } = MAX_HEALTH;
    public const float MAX_HEALTH = 30f;
    
    private const int MOVEMENT_SPEED = 9;
    private const float OFF_DURATION = 10f;
    private const float ON_DURATION = 4f;

    private const int KNOCKBACK_FORCE = 2000;
    private const int HIT_DAMAGE = 30;

    private void Start()
    {
        ChangeState(RobotState.On);
    }

    private void ChangeState(RobotState newState)
    {
        StopAllCoroutines();
        currentState = newState;
        StartCoroutine(HandleState());
    }

    private IEnumerator HandleState()
    {
        switch (currentState)
        {
            case RobotState.Off:
                stopSound.Play();
                anim.SetBool("Walk_Anim", false);
                anim.SetBool("Open_Anim", false);

                yield return new WaitForSeconds(OFF_DURATION);
                
                ChangeState(RobotState.On);
                break;

            case RobotState.On:
                startSound.Play();
                anim.SetBool("Open_Anim", true);

                yield return new WaitForSeconds(ON_DURATION);

                CurrentHealth = MAX_HEALTH;

                ChangeState(RobotState.Idle);
                break;

            // Later on, Idle will be set if player is not within Robot's boundary
            // For now, always chase.
            case RobotState.Idle:
                ChangeState(RobotState.Chase);
                break;

            case RobotState.Chase:
                anim.SetBool("Walk_Anim", true);
                yield return ChaseTarger();
                break;
        }
    }

    private IEnumerator ChaseTarger()
    {
        while (currentState == RobotState.Chase && CurrentHealth > 0f)
        {
            Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(targetPosition);
            transform.position += MOVEMENT_SPEED * Time.deltaTime * transform.forward;

            yield return null;
        }
    }

    public void OnHit(float damage)
    {
        if (currentState != RobotState.Idle && currentState != RobotState.Chase) return;
                
        CurrentHealth = Mathf.Max(0f, CurrentHealth - damage);
        
        if (CurrentHealth <= 0)
        {
            ChangeState(RobotState.Off);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            attackSound.Play();
            playerMovement.ApplyKnockback(collision, KNOCKBACK_FORCE);
            playerMovement.TakeDamage(HIT_DAMAGE);
        }
    }
}
