using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenAnchoredPosition))]
public class TweenAnchoredPositionEditor : TweenBaseEditor
{
    public override void OnInspectorGUI()
    {
        TweenAnchoredPosition tween = target as TweenAnchoredPosition;
        tween.from = EditorGUILayout.Vector2Field("From", tween.from);
        tween.to = EditorGUILayout.Vector2Field("To", tween.to);

        base.OnInspectorGUI();
    }
}