using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class LevelCompleteAnalytics : MonoBehaviour
{
    public void SendLevelCompleteEvent(string levelName, bool success, float timeElapsed)
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>
        {
            { "level_name", levelName },
            { "success", success },
            { "time_elapsed", timeElapsed }
        };

        // 发送自定义事件
        AnalyticsResult result = Analytics.CustomEvent("level_complete", eventData);

    }
}
