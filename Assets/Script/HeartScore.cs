using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HeartScore : MonoBehaviour
{
    public Text heartText;
    
    public void SetScore(int score)
    {
        heartText.text = score.ToString();
    }
}
