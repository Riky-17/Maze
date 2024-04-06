using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;

    void OnEnable() => GameManager.onTimeChange += CalculateTime;
    void OnDisable() => GameManager.onTimeChange -= CalculateTime;

    void CalculateTime(float elapsedTime)
    {
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
