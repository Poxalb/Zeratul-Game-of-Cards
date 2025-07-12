using UnityEngine;
using TMPro;
using System.Collections.Generic;
[RequireComponent(typeof(TMP_Text))]
public class FontSetter : MonoBehaviour
{
    public string fontClass;

    private void OnEnable()
    {
        //subscribe to the OnFontChanged event
        OptionsManager.OnFontChanged += SetFont;
        SetFont();
    }
    private void OnDisable()
    {
        //unsubscribe from the OnFontChanged event
        OptionsManager.OnFontChanged -= SetFont;
    }
    
    private void SetFont()
    {
        TMP_Text textComponent = GetComponent<TMP_Text>();
        if (textComponent && GameManager.Instance.optionsManager != null)
        {
            textComponent.font = GameManager.Instance.optionsManager.GetFontClass(fontClass);
        }
        else
        {
            Debug.LogWarning("TMP_Text component or OptionManager is not found.");
        }
    }
}
