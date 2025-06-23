using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using System.Collections;

/// <summary>
/// Money - UI Money Display System
/// 
/// Main Logic:
/// This script manages the visual display of money earned in the game. It creates
/// animated text that appears at world positions, moves upward, and fades out.
/// The system converts world coordinates to screen coordinates and provides smooth
/// animations for money notifications when players complete tasks like serving coffee.
/// 
/// Key Features:
/// - World-to-screen coordinate conversion
/// - Animated text movement and fading
/// - Coroutine-based smooth animations
/// - Automatic text cleanup
/// - Configurable animation parameters
/// </summary>
public class Money : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera playerCamera;

    private Vector3 startPosition;
    private Color startColor;
    private Coroutine currentAnimation;

    /// <summary>
    /// Initializes the money display system
    /// Sets up initial colors and finds required components
    /// </summary>
    void Start()
    {
        startColor = moneyText.color;
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }
        DisableText();
    }

    /// <summary>
    /// Shows animated money text at a specific world position
    /// Converts world coordinates to screen coordinates and starts the animation
    /// </summary>
    /// <param name="text">The text to display (usually money amount)</param>
    /// <param name="worldPosition">The world position where the text should appear</param>
    public void ShowMoneyText(string text, Vector3 worldPosition)
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        moneyText.text = text;
        moneyText.gameObject.SetActive(true);

        // Convert world position to screen position
        Vector3 screenPos = playerCamera.WorldToScreenPoint(worldPosition);
        startPosition = screenPos;

        currentAnimation = StartCoroutine(AnimateText());
    }

    /// <summary>
    /// Coroutine that animates the money text movement and fading
    /// Moves the text upward while gradually fading it out
    /// </summary>
    /// <returns>IEnumerator for coroutine execution</returns>
    private IEnumerator AnimateText()
    {
        float elapsedTime = 0f;
        Vector3 targetPosition = startPosition + Vector3.up * moveDistance;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        // Reset position and color
        moneyText.transform.position = startPosition;
        moneyText.color = startColor;

        // Move up
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            moneyText.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            moneyText.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        DisableText();
    }

    /// <summary>
    /// Disables the money text display
    /// Hides the text and cleans up the display
    /// </summary>
    private void DisableText()
    {
        moneyText.gameObject.SetActive(false);
    }
}
