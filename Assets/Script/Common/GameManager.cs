using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
    public GameObject gameover;

    // Map
    public GameObject map;

    // Spikes
    public Spike spikePrefab;
    public List<Spike> spikes = new List<Spike>();
    private Coroutine spikeSpawnCoroutine;
    private const float SPIKE_SPAWN_INTERVAL_SECONDS = 0.7f;
    private const float SPIKE_SPAWN_ZONE = 38f;
    private const float SPIKE_SPAWN_HEIGHT = 20f;


    public void Start()
    {
        Time.timeScale = 1;
        spikeSpawnCoroutine = StartCoroutine(SpikeSpawnCoroutine());
    }

    private IEnumerator SpikeSpawnCoroutine()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(-SPIKE_SPAWN_ZONE, SPIKE_SPAWN_ZONE),
            SPIKE_SPAWN_HEIGHT,
            Random.Range(SPIKE_SPAWN_ZONE, SPIKE_SPAWN_ZONE)
        );

        while(true)
        {
            spikes.Add(Instantiate(spikePrefab, spawnPosition, Quaternion.identity));

            yield return new WaitForSeconds(SPIKE_SPAWN_INTERVAL_SECONDS);
        }
    }

    public void GameOver()
    {
        StopCoroutine(spikeSpawnCoroutine);
        gameover.SetActive(true);
    }
}