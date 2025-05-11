using System;
using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour, IDamageable, IPoolable
{
    public AudioSource hitSound;

    private enum SpikeState { Grow, Fall, Grounded, Shrink }
    private SpikeState currentState = SpikeState.Grow;
    private Coroutine stateCoroutine;
    private float hp;

    public GameObject needle;
    public Collider innerCollider;
    public Collider outerCollider;

    public Outline outline;
    private Coroutine outlineCoroutine;

    private const int FALL_SPEED = 4;
    private const int KNOCKBACK_FORCE = 500;
    private const int HIT_DAMAGE = 10;
    private const float MAX_HEALTH = 3f;

    public void InitPoolObject()
    {
        hp = MAX_HEALTH;
        transform.localScale = Vector3.one * 0.1f;
        needle.transform.localScale = Vector3.one * 0.1f;
        needle.SetActive(false);
        innerCollider.enabled = true;
        outerCollider.enabled = false;
        outline.enabled = false;

        ChangeState(SpikeState.Grow);
    }

    public void OnHit(float damage)
    {
        if (currentState != SpikeState.Fall) return;

        HitOutline();
        hp = Mathf.Max(0f, hp - damage);
        
        if (hp <= 0)
        {
            GameManager.Instance.SpikeScore ++;
            ChangeState(SpikeState.Shrink);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState != SpikeState.Fall && currentState != SpikeState.Grounded) return;

        if (collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            hitSound.Play();
            playerMovement.ApplyKnockback(collision, KNOCKBACK_FORCE);
            playerMovement.TakeDamage(HIT_DAMAGE);
            ChangeState(SpikeState.Shrink);
        }
    }

    private void ChangeState(SpikeState newState)
    {
        if (stateCoroutine != null) StopCoroutine(stateCoroutine);
        currentState = newState;
        stateCoroutine = StartCoroutine(HandleState());
    }

    private IEnumerator HandleState()
    {
        switch (currentState)
        {
            case SpikeState.Grow:
                yield return Grow();
                ChangeState(SpikeState.Fall);
                break;

            case SpikeState.Fall:
                yield return Fall();
                if (hp > 0f) ChangeState(SpikeState.Grounded);
                break;

            case SpikeState.Grounded:
                needle.SetActive(true);
                innerCollider.enabled = false;
                outerCollider.enabled = true;
                yield return NeedleOut();
                break;

            case SpikeState.Shrink:
                innerCollider.enabled = false;
                outerCollider.enabled = false;
                yield return Shrink();
                break;
        }
    }

    private IEnumerator Grow()
    {
        while (transform.localScale.x < 2f)
        {
            transform.localScale += 2 * Time.deltaTime * Vector3.one;
            yield return null;
        }

        transform.localScale = 2f * Vector3.one;
    }

    private IEnumerator Fall()
    {
        while (transform.position.y > 0.5f && hp > 0f)
        {
            transform.Translate(FALL_SPEED * Time.deltaTime * Vector3.down);
            transform.Rotate(0f, 0.1f, 0f);

            yield return null;
        }

        if (hp > 0)
        {
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }
    }

    private IEnumerator NeedleOut()
    {
        while (needle.transform.localScale.x < 1f)
        {
            needle.transform.localScale += 4.5f * Time.deltaTime * Vector3.one;

            yield return null;
        }

        needle.transform.localScale = Vector3.one;
    }

    private IEnumerator Shrink()
    {
        while (transform.localScale.x > 0.1f)
        {
            transform.localScale -= 8f * Time.deltaTime * Vector3.one;

            if (transform.localScale.x < 0.1f)
            {
                transform.localScale = Vector3.one * 0.1f;
                break;
            }

            yield return null;
        }

        SpikeManager.Instance.objectPool.Return(this);
    }

    private void HitOutline()
    {
        if (outlineCoroutine != null)
        {
            StopCoroutine(outlineCoroutine);
        }
        outlineCoroutine = StartCoroutine(ShowOutline());
    }
    
    private IEnumerator ShowOutline()
    {
        outline.enabled = true;
        yield return new WaitForSeconds(0.2f);
        outline.enabled = false;
    }

    public void ResetPoolObject()
    {
        StopAllCoroutines();
    }
}
