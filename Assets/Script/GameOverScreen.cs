using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Text heartText;
    public Text timeText;

    public TimeScore timeScore;
    public Button startButton;

    public void Start()
    {
        startButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }

    private void OnEnable()
    {
        heartText.text = GameManager.Instance.heartScore.ToString();
        timeText.text = timeScore.GetFormattedTime();
    }
}