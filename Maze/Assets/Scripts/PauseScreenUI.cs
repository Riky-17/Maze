using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreenUI : MonoBehaviour
{
    [SerializeField] Button resumeButton;

    void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePause();
        });
    }
}
