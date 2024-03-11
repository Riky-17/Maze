using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    bool isPaused = false;
    [SerializeField] GameObject pauseScreen;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseScreen.SetActive(isPaused);
        if(isPaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
