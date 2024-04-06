using UnityEngine;
using UnityEngine.UI;

public class StartingScreenUI : MonoBehaviour
{
    [SerializeField] GameObject difficultySelectionScreen;

    [SerializeField] Button startGameButton;
    [SerializeField] Button exitGameButton;

    void Awake()
    {
        startGameButton.onClick.AddListener(() => 
        {
            difficultySelectionScreen.SetActive(true);
            gameObject.SetActive(false);
        });

        exitGameButton.onClick.AddListener(() => 
        {
            Application.Quit();
        });
    }
}
