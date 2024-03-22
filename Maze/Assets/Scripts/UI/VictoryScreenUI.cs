using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryScreenUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    
    void Awake()
    {
        float elapsedTime = GameManager.Instance .ElapsedTime;
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int minutes = Mathf.FloorToInt(elapsedTime / 60);

        timerText.text = $"You've beaten the maze in: {minutes:00}:{seconds:00}";
    }
}
