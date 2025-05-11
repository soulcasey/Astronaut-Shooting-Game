using System.Collections;
using UnityEngine;

public class SpikeManager : SingletonBase<SpikeManager>
{
    public SpikeObjectPool objectPool;

    private const float SPAWN_INTERVAL_SECONDS = 0.7f;    
    private const float SPAWN_ZONE = 38f;
    private const float SPAWN_HEIGHT = 20f;


    private void Start()
    {
        StartCoroutine(SpikeSpawnCoroutine());
    }

    private IEnumerator SpikeSpawnCoroutine()
    {
        while(true)
        {
            Spike spike = objectPool.Get();
            spike.transform.position = GetRandomPosition();

            yield return new WaitForSeconds(SPAWN_INTERVAL_SECONDS);
        }
    }

    
    private Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(-SPAWN_ZONE, SPAWN_ZONE),
            SPAWN_HEIGHT,
            Random.Range(-SPAWN_ZONE, SPAWN_ZONE)
        );
    }
}