using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void UnlockLevel(int levelIndex)
    {
        // 这里添加解锁关卡的逻辑
        // 更新关卡状态，例如保存到玩家偏好或数据库中
    }

    // 假设每个关卡按钮调用这个方法
    public void OnLevelButtonClicked(string levelName)
    {
        LoadLevel(levelName);
    }
}