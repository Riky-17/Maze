using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreenUI : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Button mainMenuButton;
    
    void Awake()
    {
        float elapsedTime = GameManager.Instance .ElapsedTime;
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int minutes = Mathf.FloorToInt(elapsedTime / 60);

        timerText.text = $"You've beaten the maze in: {minutes:00}:{seconds:00}";

        mainMenuButton.onClick.AddListener(() =>
        {
            canvas.SetActive(false);
            SceneLoader.Instance.LoadScene(Scenes.MainMenu);
        });
    }
}
