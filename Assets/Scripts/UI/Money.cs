using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using System.Collections;

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

    void Start()
    {
        startColor = moneyText.color;
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }
        DisableText();
    }

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

    private void DisableText()
    {
        moneyText.gameObject.SetActive(false);
    }
}
