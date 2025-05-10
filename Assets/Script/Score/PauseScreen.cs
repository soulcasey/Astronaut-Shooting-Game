using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public Button resumeButton;
    public Button exitButton;

    private void Start()
    {
        resumeButton.onClick.AddListener(() => 
        {
            gameObject.SetActive(false);
        });
        exitButton.onClick.AddListener(() => Application.Quit());
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseGame(false);
    }

    private void OnEnable()
    {
        GameManager.Instance.PauseGame(true);
    }
}