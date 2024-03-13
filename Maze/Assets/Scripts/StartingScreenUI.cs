using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingScreenUI : MonoBehaviour
{
    [SerializeField] Button startGameButton;
    [SerializeField] Button exitGameButton;

    void Awake()
    {
        startGameButton.onClick.AddListener(() => 
        {
            SceneLoader.LoadScene(Scenes.Maze);
        });

        exitGameButton.onClick.AddListener(() => 
        {
            Application.Quit();
        });
    }
}
