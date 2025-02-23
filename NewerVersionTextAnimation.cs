using System.Collections;
using UnityEngine;
using TMPro;

public class TextAnimation : MonoBehaviour
{
    public float lerpDuration = 1f;

    private TMP_Text noCustomerText;
    private float fadeDelay = 1f;

    /* StartsTextAnimation()
    Starts the text animation by positioning it at the given location and triggering both movement and fading effects.
    */
    public void StartTextAnimation(Vector3 instantiatedPosition, Canvas noCustomerCanvas)
    {
        noCustomerText = GetComponent<TMP_Text>();
        RectTransform canvasRectTransform = noCustomerCanvas.GetComponent<RectTransform>();
        noCustomerText.rectTransform.localPosition = canvasRectTransform.InverseTransformPoint(instantiatedPosition);

        StartCoroutine(AnimateAndDestroy());
    }

    /* AnimateAndDestroy()
    Handles all of the positional and alpha animations before destroying the game object.
    */
    private IEnumerator AnimateAndDestroy()
    {
        Vector3 startPos = noCustomerText.rectTransform.localPosition;
        Vector3 midPos = startPos + new Vector3(0, 75, 0);
        Vector3 endPos = startPos + new Vector3(0, 120, 0);
        Color startColor = noCustomerText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        StartCoroutine(LerpTextPosition(noCustomerText.rectTransform, midPos, endPos, lerpDuration));
        yield return StartCoroutine(LerpTextAlpha(noCustomerText, startColor, endColor, lerpDuration));

        Destroy(gameObject);
    }

    /* LerpTextPosition()
    Moves the text from the start position to the end position over a specific duration.
    */
    private IEnumerator LerpTextPosition(RectTransform rectTransform, Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            rectTransform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            yield return null;
        }
        rectTransform.localPosition = endPos;
    }

    /* LerpTextAlpha()
    Slowly fades out the text over a specific duration.
    */
    private IEnumerator LerpTextAlpha(TMP_Text text, Color startColor, Color endColor, float duration)
    {
        yield return new WaitForSeconds(fadeDelay);
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            text.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }
        text.color = endColor;
    }
}
