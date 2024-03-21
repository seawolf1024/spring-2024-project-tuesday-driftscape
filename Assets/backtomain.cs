using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class backtomain : MonoBehaviour
{
    public void LoadLevel()
    {
        Debug.Log("Back to main");
        SceneManager.LoadScene("Home");
    }

}