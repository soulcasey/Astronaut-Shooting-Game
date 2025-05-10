using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonBase<GameManager>
{
    public GameObject gameover;

    [Header("Map")]
    public GameObject map;

    [Header("Player")]
    public PlayerMovement player;

    [Header("Spike")]
    public Spike spikePrefab;
    public List<Spike> spikes = new List<Spike>();
    private const int PASSIVE_DAMAGE = 2;

    public Text heartScoreText;

    private int heartScore;
    public int HeartScore
    {
        get { return heartScore; }
        set
        {
            heartScore = value;
            heartScoreText.text = heartScore.ToString();
        }
    }

    public void Start()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        StartCoroutine(SpikeSpawnCoroutine());
        StartCoroutine(PassiveDamageCoroutine());
    }

    private IEnumerator SpikeSpawnCoroutine()
    {
        while(true)
        {
            spikes.Add(Instantiate(spikePrefab, Spike.GetRandomPosition(), Quaternion.identity));

            yield return new WaitForSeconds(Spike.SPAWN_INTERVAL_SECONDS);
        }
    }

    private IEnumerator PassiveDamageCoroutine()
    {
        while(player.CurrentHealth > 0)
        {
            player.TakeDamage(PASSIVE_DAMAGE);

            yield return new WaitForSeconds(1);
        }
    }

    public void GameOver()
    {
        StopAllCoroutines();
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        Time.timeScale = 0;
        gameover.SetActive(true);
    }
}