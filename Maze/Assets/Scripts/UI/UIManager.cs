using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;
    [SerializeField] TextMeshProUGUI signalsLeftText;
    [SerializeField] GameObject victoryScreen;

    void OnEnable()
    {
        GameManager.onPauseToggle += TogglePause;
        Player.onSignalUpdate += UpdateSignalsLeft;
        MazeGenerator.onWin += ShowVictoryScreen;
    }

    void OnDisable()
    {
        GameManager.onPauseToggle -= TogglePause;
        Player.onSignalUpdate -= UpdateSignalsLeft;
        MazeGenerator.onWin -= ShowVictoryScreen;
    }

    void ShowVictoryScreen() => victoryScreen.SetActive(true);
    void UpdateSignalsLeft(int signalsLeft) => signalsLeftText.text = $"Signals left: {signalsLeft}";
    void TogglePause(bool isPaused) => pauseScreen.SetActive(isPaused);
}
