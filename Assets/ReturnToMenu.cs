using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{

    void switchToMenu()
    {
        // Reloads the current scene
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void testswitch()
    {
        // Reloads the current scene
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}
