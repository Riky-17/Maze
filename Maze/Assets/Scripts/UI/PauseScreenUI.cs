using UnityEngine;
using UnityEngine.UI;

public class PauseScreenUI : MonoBehaviour
{
    [SerializeField] GameObject canvas;
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
            GameManager.Instance.TogglePause();
            canvas.SetActive(false);
            SceneLoader.Instance.LoadScene(Scenes.MainMenu);
        });
    }
}
