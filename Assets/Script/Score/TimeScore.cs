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

    public string GetFormattedTime()
    {
        int minutes = ElapsedSeconds / 60;
        int seconds = ElapsedSeconds % 60;
        return minutes.ToString() + ":" + seconds.ToString("D2");
    }

    private IEnumerator UpdateTime()
    {
        while (true)
        {
            timeText.text = GetFormattedTime();

            yield return new WaitForSeconds(1f);
            ElapsedSeconds++;
        }
    }
}