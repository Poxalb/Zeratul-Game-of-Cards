using System.Collections.Generic;
using UnityEngine;
using System;

public static class Keybinds
{
    private static Dictionary<string, KeyCode> keyMap = new Dictionary<string, KeyCode>();

    public static void SetKey(string action, KeyCode key)
    {
        keyMap[action] = key;
    }

    public static KeyCode GetKey(string action)
    {
        return keyMap.ContainsKey(action) ? keyMap[action] : KeyCode.None;
    }

    public static bool GetKeyDown(string action)
    {
        return Input.GetKeyDown(GetKey(action));
    }

    public static bool GetKeyUp(string action)
    {
        return Input.GetKeyUp(GetKey(action));
    }

    public static void LoadDefaultsIfEmpty()
    {
        TryLoadKey("MoveLeft", KeyCode.A);
        TryLoadKey("MoveRight", KeyCode.D);
        TryLoadKey("Jump", KeyCode.Space);
        TryLoadKey("Dash", KeyCode.LeftShift);
        TryLoadKey("Interact", KeyCode.E);
        TryLoadKey("OpenGatesGuide", KeyCode.C);
    }

    private static void TryLoadKey(string action, KeyCode defaultKey)
    {
        string saved = PlayerPrefs.GetString(action, defaultKey.ToString());
        if (Enum.TryParse<KeyCode>(saved, out var parsedKey))
        {
            SetKey(action, parsedKey);
        }
    }

}
