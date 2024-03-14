using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    int prevSecond;

    void Update()
    {
        if(Time.timeScale == 0)
            return;

        elapsedTime += Time.deltaTime;
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        if(prevSecond != seconds)
        {
            timerText.text = $"{minutes:00}:{seconds:00}";
            prevSecond = seconds;
        }
    }
}
