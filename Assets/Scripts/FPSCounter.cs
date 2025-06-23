using UnityEngine;
using TMPro;

/// <summary>
/// FPSCounter - Frame Rate Display System
/// 
/// Main Logic:
/// This script calculates and displays the current frame rate (FPS) in real-time.
/// It uses a buffer system to average FPS over multiple frames for more stable
/// readings. The FPS is displayed using TextMeshPro UI component and updates
/// every 4 frames to provide smooth, readable performance metrics.
/// 
/// Key Features:
/// - Real-time FPS calculation and display
/// - Frame averaging for stable readings
/// - TextMeshPro UI integration
/// - Configurable update frequency
/// - Performance monitoring tool
/// </summary>
public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float deltaTime;
    private int frameCount;
    private float fpsBuffer;

    /// <summary>
    /// Calculates and updates the FPS display
    /// Accumulates frame time and updates the display every 4 frames
    /// </summary>
    void Update()
    {
        deltaTime += Time.unscaledDeltaTime;
        frameCount++;

        if (frameCount >= 4)
        {
            fpsBuffer = frameCount / deltaTime;
            deltaTime = 0f;
            frameCount = 0;
            fpsText.text = "FPS: " + fpsBuffer.ToString("F1");
        }
    }
}