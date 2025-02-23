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
        Vector3 localPosition = canvasRectTransform.InverseTransformPoint(instantiatedPosition);
    
        noCustomerText.rectTransform.localPosition = localPosition;

        StartCoroutine(AnimateAndDestroy());
    }

    /* AnimateAndDestroy()
    Handles all of the positional and alpha animations before destroying the game object.
    */
    private IEnumerator AnimateAndDestroy()
    {
        yield return StartCoroutine(LerpTextPosition(noCustomerText.rectTransform, noCustomerText.rectTransform.localPosition + new Vector3(0, 75, 0), noCustomerText.rectTransform.localPosition + new Vector3(0, 120, 0), lerpDuration));
        yield return StartCoroutine(LerpTextAlpha(noCustomerText, noCustomerText.color, new Color(noCustomerText.color.r, noCustomerText.color.g, noCustomerText.color.b, 0f), lerpDuration));

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
            float t = elapsedTime / duration;

            rectTransform.localPosition = Vector3.Lerp(startPos, endPos, t);
            
            yield return null;
        }

        rectTransform.localPosition = endPos;
    }

    /* LerpTextAlpha()
    Slowly fades out the text over a specific duration.
    */
    private IEnumerator LerpTextAlpha(TMP_Text text, Color startAlpha, Color endAlpha, float duration)
    {
        float elapsedTime = 0f;

        yield return new WaitForSeconds(fadeDelay);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            text.color = Color.Lerp(startAlpha, endAlpha, t);

            yield return null;
        }

        text.color = endAlpha;
    }
}
