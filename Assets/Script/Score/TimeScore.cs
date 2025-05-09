using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeScore : MonoBehaviour
{
    public Text timeText;
    public int ElapsedSeconds { get; private set; } = 0;

    private void Start()
    {
        StartCoroutine(UpdateTime());
    }

    private IEnumerator UpdateTime()
    {
        while (true)
        {
            int minutes = ElapsedSeconds / 60;
            int seconds = ElapsedSeconds % 60;
            timeText.text = minutes.ToString() + ":" + seconds.ToString("D2");

            yield return new WaitForSeconds(1f);
            ElapsedSeconds++;
        }
    }
}