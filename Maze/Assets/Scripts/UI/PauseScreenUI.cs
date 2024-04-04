using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreenUI : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button MainMenuButton;

    void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePause();
        });
    
        MainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.Instance.LoadScene(Scenes.MainMenu);
        });
    }
}
