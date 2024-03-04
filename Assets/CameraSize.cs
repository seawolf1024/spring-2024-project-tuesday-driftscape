using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]

public class CameraSize : MonoBehaviour
{
    // 目标宽高比
    private float targetAspectRatio = 1.6f; // 例如，960x600的宽高比是1.6

    void Start()
    {
        AdjustCamera();
    }

    void AdjustCamera()
    {
        // 获取当前摄像机组件
        Camera camera = GetComponent<Camera>();

        // 计算当前窗口的宽高比
        float windowAspectRatio = (float)Screen.width / (float)Screen.height;

        // 计算缩放比例
        float scaleHeight = windowAspectRatio / targetAspectRatio;

        // 根据缩放比例调整camera的viewportRect
        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}