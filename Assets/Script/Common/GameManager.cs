using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
    public GameObject gameover;

    [Header("Map")]
    public GameObject map;

    [Header("Spike")]
    public Spike spikePrefab;
    public List<Spike> spikes = new List<Spike>();
    private Coroutine spikeSpawnCoroutine;

    public int heartScore = 0;

    public void Start()
    {
        Time.timeScale = 1;
        spikeSpawnCoroutine = StartCoroutine(SpikeSpawnCoroutine());
    }

    private IEnumerator SpikeSpawnCoroutine()
    {
        while(true)
        {
            spikes.Add(Instantiate(spikePrefab, Spike.GetRandomPosition(), Quaternion.identity));

            yield return new WaitForSeconds(Spike.SPAWN_INTERVAL_SECONDS);
        }
    }

    public void GameOver()
    {
        StopCoroutine(spikeSpawnCoroutine);
        gameover.SetActive(true);
    }
}