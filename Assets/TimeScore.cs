using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeScore : MonoBehaviour
{
    public Text timeText;
    private float startTime;
    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f0");
        if(t%60 < 10)
        {
            timeText.text = minutes + ":" + "0" + seconds;
        }
        else
        {
            timeText.text = minutes + ":" + seconds;
        }
        
    }
}
