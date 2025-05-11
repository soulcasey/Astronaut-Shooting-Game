using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public Slider audioSlider;
    public Button resumeButton;
    public Button exitButton;

    private void Start()
    {
        resumeButton.onClick.AddListener(() => 
        {
            gameObject.SetActive(false);
        });
        exitButton.onClick.AddListener(() => Application.Quit());
        audioSlider.onValueChanged.AddListener((value) => 
        {
            AudioManager.Instance.SetVolume(value);
        });
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseGame(false);
    }

    private void OnEnable()
    {
        audioSlider.value = AudioListener.volume;
        GameManager.Instance.PauseGame(true);
    }
}