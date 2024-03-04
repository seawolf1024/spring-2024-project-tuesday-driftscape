using UnityEngine;

public class ChangeGlobalGravity : MonoBehaviour
{
    // 设置新的重力值，你可以在Unity的Inspector窗口中修改这些值
    public Vector3 newGravity = new Vector3(0, 9.81f, 0);

    // 在脚本启动时调用
    void Start()
    {
        // 设置全局重力
        Physics2D.gravity = newGravity;
        Debug.Log("New gravity set to: " + Physics.gravity);

    }
}
