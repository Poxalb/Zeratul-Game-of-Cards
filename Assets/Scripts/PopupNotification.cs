using System.Collections;
using UnityEngine;
using TMPro;

public class PopupNotification : MonoBehaviour
{
    public static PopupNotification Instance;

    public UIFade fade;
    public GameObject notificationPanel;
    public TMP_Text notificationText;

    private Coroutine currentNotification;
    private Coroutine fadeCoroutine;

    public Vector3 hiddenPosition = new Vector3(0, 200, 0);
    public Vector3 shownPosition = new Vector3(0, 0, 0);
    public float slideSpeed = 500f;

    private RectTransform panelRect;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        panelRect = notificationPanel.GetComponent<RectTransform>();
        canvasGroup = notificationPanel.GetComponent<CanvasGroup>();

        panelRect.anchoredPosition = hiddenPosition;
        canvasGroup.alpha = 1f;
        notificationPanel.SetActive(false);
    }

    public void ShowNotification(string message)
    {
        if (currentNotification != null)
        {
            StopCoroutine(currentNotification);
            currentNotification = null;
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        fade.StopAllCoroutines();


        notificationPanel.SetActive(false);
        panelRect.anchoredPosition = hiddenPosition;
        canvasGroup.alpha = 1f;
        notificationPanel.SetActive(true);

        currentNotification = StartCoroutine(ShowMessage(message));
    }

    IEnumerator ShowMessage(string message)
    {
        notificationText.text = message;

        yield return StartCoroutine(SlidePanel(hiddenPosition, shownPosition));

        yield return new WaitForSeconds(7f);

        fadeCoroutine = StartCoroutine(FadeAndHide());
    }

    IEnumerator FadeAndHide()
    {
        fade.FadeOut();

        yield return new WaitForSeconds(fade.fadeDuration);

        notificationPanel.SetActive(false);
        panelRect.anchoredPosition = hiddenPosition;
        canvasGroup.alpha = 1f;
    }

    IEnumerator SlidePanel(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = Mathf.Abs(Vector3.Distance(from, to)) / slideSpeed;

        while (elapsed < duration)
        {
            panelRect.anchoredPosition = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        panelRect.anchoredPosition = to;
    }
}
