using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;

    void OnEnable() => GameManager.onPauseToggle += TogglePause;
    void OnDisable() => GameManager.onPauseToggle -= TogglePause;

    private void TogglePause(bool isPaused) => pauseScreen.SetActive(isPaused);
}
