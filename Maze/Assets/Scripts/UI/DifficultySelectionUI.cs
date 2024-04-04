using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelectionUI : MonoBehaviour
{
    [SerializeField] GameObject startingScreen;
    [SerializeField] GameObject customSizeScreen;
    [SerializeField] GameObject canvas;

    [SerializeField] Button easyButton;
    [SerializeField] Button mediumButton;
    [SerializeField] Button hardButton;
    [SerializeField] Button customButton;
    [SerializeField] Button backButton;

    void Awake()
    {
        easyButton.onClick.AddListener(() =>
        {
            GameManager.Instance.SetDifficulty(Difficulties.Easy);
            canvas.SetActive(false);
            SceneLoader.Instance.LoadScene(Scenes.Maze);
        });
        
        mediumButton.onClick.AddListener(() =>
        {
            GameManager.Instance.SetDifficulty(Difficulties.Medium);
            canvas.SetActive(false);
            SceneLoader.Instance.LoadScene(Scenes.Maze);
        });
        
        hardButton.onClick.AddListener(() =>
        {
            GameManager.Instance.SetDifficulty(Difficulties.Hard);
            canvas.SetActive(false);
            SceneLoader.Instance.LoadScene(Scenes.Maze);
        });
        
        customButton.onClick.AddListener(() =>
        {
            customSizeScreen.SetActive(true);
            gameObject.SetActive(false);
        });

        backButton.onClick.AddListener(() =>
        {
            startingScreen.SetActive(true);
            gameObject.SetActive(false);
        });
    }
}
