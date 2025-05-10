using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Text killText;
    public Text heartText;
    public Text spikeText;
    public Text timeText;

    public TimeScore timeScore;
    public Button startButton;

    public void Start()
    {
        startButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }

    private void OnEnable()
    {
        killText.text = GameManager.Instance.KillScore.ToString();
        heartText.text = GameManager.Instance.HeartScore.ToString();
        spikeText.text = GameManager.Instance.SpikeScore.ToString();
        timeText.text = timeScore.GetFormattedTime();
    }
}