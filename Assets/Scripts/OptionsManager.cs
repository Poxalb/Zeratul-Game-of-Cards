using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
public class OptionsManager : MonoBehaviour
{

    private AudioManager audioManager;


    public bool isMusicEnabled = true;

    public List<TMP_FontAsset> fonts;

    public static event Action OnFontChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GameManager.Instance.audioManager;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public TMP_FontAsset GetFontClass(string classId)
    {
        switch (classId)
        {
            case "MenuText":
                return fonts[0];
            case "CardDescriptionText":
                return fonts[1];
            case "CardTitleText":
                return fonts[2];
            case "CardBoldText":
                return fonts[3];
            case "MenuTextBold":
                return fonts[4];
            default:
                return fonts[0];
        }
    }

    public void UpdateFont()
    {
        OnFontChanged?.Invoke();
    }
}
