using System;
using UnityEngine;
using TMPro;

public class KeybindButton : MonoBehaviour
{
    [SerializeField] private string actionName;    
    [SerializeField] private TMP_Text buttonText;
    private bool waitingForKey = false;


    public KeyCode GetDefaultKey(string action)
    {
        switch (action)
        {
            case "MoveLeft": return KeyCode.A;
            case "MoveRight": return KeyCode.D;
            case "Jump": return KeyCode.Space;
            case "Dash": return KeyCode.LeftShift;
            case "Interact": return KeyCode.E;
            case "OpenGatesGuide": return KeyCode.C;
            default: return KeyCode.None;
        }
    }

    private void Start()
    {
        Debug.Log("KeybindButton Start");
        Keybinds.LoadDefaultsIfEmpty();
        string savedKey = PlayerPrefs.GetString(actionName, GetDefaultKey(actionName).ToString());
        KeyCode key = (KeyCode)Enum.Parse(typeof(KeyCode), savedKey);
        Keybinds.SetKey(actionName, key);
        UpdateButtonText();
    }

    public void OnClickChangeKey()
    {
        if (!waitingForKey)
            StartCoroutine(WaitForKeyPress());
    }

    private System.Collections.IEnumerator WaitForKeyPress()
    {
        waitingForKey = true;
        buttonText.text = "";

        while (!Input.anyKeyDown)
            yield return null;

        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                Keybinds.SetKey(actionName, key);
                PlayerPrefs.SetString(actionName, key.ToString());
                PlayerPrefs.Save();
                break;
            }
        }

        UpdateButtonText();
        waitingForKey = false;
    }

    private void UpdateButtonText()
    {
        buttonText.text = Keybinds.GetKey(actionName).ToString();
    }
}
