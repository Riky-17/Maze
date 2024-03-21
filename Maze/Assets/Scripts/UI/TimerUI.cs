using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public float ElapsedTime {get; private set;}
    int prevSecond;

    void Update()
    {
        if(Time.timeScale == 0)
            return;

        ElapsedTime += Time.deltaTime;
        int seconds = Mathf.FloorToInt(ElapsedTime % 60);
        int minutes = Mathf.FloorToInt(ElapsedTime / 60);
        if(prevSecond != seconds)
        {
            timerText.text = $"{minutes:00}:{seconds:00}";
            prevSecond = seconds;
        }
    }
}
