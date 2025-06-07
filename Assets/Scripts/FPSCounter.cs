using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float deltaTime;
    private int frameCount;
    private float fpsBuffer;

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