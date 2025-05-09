using UnityEngine;
using System;

public class Heart: MonoBehaviour
{
    private const int ROTATION_SPEED = 20;

    private const float SPAWN_ZONE = 37f;
    private const float SPAWN_HEIGHT = 7f;

    private const int HEAL_AMOUNT = 50;

    private void Update()
    {
        transform.Rotate(ROTATION_SPEED * Time.deltaTime * new Vector3(0, 0, 1));
    }

    private void Reposition()
    {
        Vector3 spawnPosition = new Vector3(
            UnityEngine.Random.Range(-SPAWN_ZONE, SPAWN_ZONE),
            SPAWN_HEIGHT,
            UnityEngine.Random.Range(-SPAWN_ZONE, SPAWN_ZONE)
        );

        transform.position = spawnPosition;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && collider.TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.heartGainParticle.Play();
            playerMovement.Heal(HEAL_AMOUNT);

            GameManager.Instance.heartScore ++;
            Reposition();
        }
    }
}
