using UnityEngine;
using System.Collections;
using Proyecto26;
using UnityEngine.Networking;
using System;

public class LevelCompleteAnalytics : MonoBehaviour
{
    public void SendLevelCompleteEvent(string levelName, bool success, float timeElapsed)
    {
        Debug.Log(levelName);
        Debug.Log(success);
        Debug.Log(timeElapsed);
        StartCoroutine(SendLevelCompleteEventCoroutine(levelName, success, timeElapsed));
    }

    private IEnumerator SendLevelCompleteEventCoroutine(string levelName, bool success, float timeElapsed)
    {
        Debug.Log(levelName);
        Debug.Log(success);
        Debug.Log(timeElapsed);
        PlayerData player = new PlayerData();
        player.levelName = levelName;
        player.success = success;
        player.timeElapsed = timeElapsed;
        string json = JsonUtility.ToJson(player);

        yield return RestClient.Post("https://driftspace-default-rtdb.firebaseio.com/.json", json);
    }
}

[System.Serializable]
public class PlayerData
{
    public string levelName;
    public bool success;
    public float timeElapsed;
    public float locationX = 0;
    public float locationY = 0;

}
