using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartGame : MonoBehaviour
{
    public void RestartCurrentScene()
    {
        // Reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }


    void Update(){
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("XXXXX");
            RestartCurrentScene();
        }
    }
}
