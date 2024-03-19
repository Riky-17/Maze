using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;
    [SerializeField] TextMeshProUGUI signalsLeftText;

    void OnEnable()
    {
        GameManager.onPauseToggle += TogglePause;
        Player.onSignalUpdate += UpdateSignalsLeft;
    }

    void OnDisable()
    {
        GameManager.onPauseToggle -= TogglePause;
        Player.onSignalUpdate -= UpdateSignalsLeft;
    }

    private void UpdateSignalsLeft(int signalsLeft) => signalsLeftText.text = $"Signals left: {signalsLeft}";
    private void TogglePause(bool isPaused) => pauseScreen.SetActive(isPaused);
}
