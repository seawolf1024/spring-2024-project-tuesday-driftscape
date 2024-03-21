using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;

    public bool GetIsPaused()
    {
        return isPaused;
    }
    void Awake()
    {
        // Deactivate pauseScreen initially
        pauseMenu.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
            Pause();
        else
            Resume();
    }

    private void Pause()
    {
        Time.timeScale = 0f; // Pause the game
        pauseMenu.SetActive(true);
    }

    private void Resume()
    {
        Time.timeScale = 1f; // Resume the game
        pauseMenu.SetActive(false);
    }

}
