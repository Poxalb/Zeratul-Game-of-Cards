using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(DrawPileManager))]
public class DManagerEditor : Editor
{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();

        DrawPileManager drawPile = (DrawPileManager)target;
        if (GUILayout.Button("Draw Next Card"))
        {
            HandManager handManager = FindFirstObjectByType<HandManager>();
            if (handManager != null)
            {
                drawPile.DrawCard(handManager);
            }
            else
            {
                Debug.LogWarning("HandManager not found in the scene.");
            }
        }
    }
}
#endif