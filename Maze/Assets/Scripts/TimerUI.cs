using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;

    void Update()
    {
        if(Time.timeScale == 0)
            return;

        elapsedTime += Time.deltaTime;
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int minutes = Mathf.FloorToInt(elapsedTime / 60); 
        timerText.text = minutes > 9 && seconds > 9 ? $"{minutes}:{seconds}" 
                       : minutes < 10 && seconds > 9 ? $"0{minutes}:{seconds}" 
                       : minutes > 9 && seconds < 10 ? $"{minutes}:0{seconds}" 
                       : $"0{minutes}:0{seconds}";
    }
}
