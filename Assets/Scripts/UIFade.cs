using UnityEngine;
using System.Collections;

public class UIFade : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;

    public void FadeIn()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeCanvas(0f, 1f));
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeCanvas(1f, 0f, deactivateAfter: true));
    }

    private IEnumerator FadeCanvas(float startAlpha, float endAlpha, bool deactivateAfter = false)
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        if (deactivateAfter && endAlpha == 0f)
            gameObject.SetActive(false);
    }
}
