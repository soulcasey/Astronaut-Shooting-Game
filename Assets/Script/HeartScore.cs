using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HeartScore : MonoBehaviour
{
    public Text heartText;
    public PlayerMovement player;
    

    // Update is called once per frame
    void Update()
    {
        heartText.text = (player.heartScore).ToString();
    }
}
