using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    public GameObject notificationPanel;
    public TMP_Text notificationText;
    private Coroutine currentNotification;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        notificationPanel.SetActive(false);
    }

    public void ShowNotification(string message)
    {
        if (currentNotification != null)
            StopCoroutine(currentNotification);

        currentNotification = StartCoroutine(ShowMessage(message));
    }

    IEnumerator ShowMessage(string message)
    {
        notificationPanel.SetActive(true);
        notificationText.text = message;

        yield return new WaitForSeconds(2f);   
    }
}
