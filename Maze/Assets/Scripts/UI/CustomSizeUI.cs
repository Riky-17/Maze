using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomSizeUI : MonoBehaviour
{
    [SerializeField] GameObject difficultySelectionScreen;
    [SerializeField] TMP_InputField mazeWidthInput;
    [SerializeField] TMP_InputField mazeHeightInput;
    [SerializeField] Button startGameButton;
    [SerializeField] Button backButton;
    const string VALID_CHARACTERS = "0123456789";

    void Awake()
    {
        mazeWidthInput.onValidateInput = (string text, int charIndex, char addedChar) => {return ValidateChar(addedChar);};
        mazeWidthInput.onValidateInput = (string text, int charIndex, char addedChar) => {return ValidateChar(addedChar);};

        startGameButton.onClick.AddListener(() => 
        {
            if(mazeWidthInput.text == "" || mazeHeightInput.text == "")
                return;

            int mazeWidth = Mathf.Clamp(int.Parse(mazeWidthInput.text), 5, 150);
            int mazeHeight = Mathf.Clamp(int.Parse(mazeHeightInput.text), 5, 150);
            GameManager.Instance.SetDifficulty(Difficulties.Custom);
            GameManager.Instance.SetCustomSize(mazeWidth, mazeHeight);
            SceneLoader.LoadScene(Scenes.Maze);
        });

        backButton.onClick.AddListener(() => 
        {
            difficultySelectionScreen.SetActive(true);
            gameObject.SetActive(false);
        });
    }

    private char ValidateChar(char addedChar)
    {
        if(VALID_CHARACTERS.IndexOf(addedChar) != -1)
            return addedChar;
        return '\0';
    }
}
