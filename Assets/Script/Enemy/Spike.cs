using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour, IDamageable
{
    private enum SpikeState { Grow, Fall, Grounded, Shrink }

    private SpikeState currentState = SpikeState.Grow;
    private float hp = 3f;

    private const int FALL_SPEED = 4;
    public const float SPAWN_INTERVAL_SECONDS = 0.7f;
    private const float SPAWN_ZONE = 38f;
    private const float SPAWN_HEIGHT = 20f;

    private const int KNOCKBACK_FORCE = 500;
    private const int HIT_DAMAGE = 10;

    private void Start()
    {
        transform.localScale = Vector3.one * 0.1f;
        StartCoroutine(HandleState());
    }

    public static Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(-SPAWN_ZONE, SPAWN_ZONE),
            SPAWN_HEIGHT,
            Random.Range(-SPAWN_ZONE, SPAWN_ZONE)
        );
    }

    public void OnHit(float damage)
    {
        if (currentState != SpikeState.Fall) return;
                
        hp = Mathf.Max(0f, hp - damage);
        
        if (hp <= 0)
        {
            ChangeState(SpikeState.Shrink);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.ApplyKnockback(collision, KNOCKBACK_FORCE);
            playerMovement.TakeDamage(HIT_DAMAGE);
            ChangeState(SpikeState.Shrink);
        }
    }

    private void ChangeState(SpikeState newState)
    {
        StopAllCoroutines();
        currentState = newState;
        StartCoroutine(HandleState());
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
                break;

            case SpikeState.Shrink:
                yield return ShrinkAndDestroy();
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

    private IEnumerator ShrinkAndDestroy()
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

        Destroy(gameObject);
    }
}
