using System;
using UnityEngine;

public class TweenAnchoredPosition : TweenBase
{
    private void Awake()
    {
        rectTrans = GetComponent<RectTransform>();
    }

    protected override void UpdateTween(float t)
    {
        if (rectTrans == null)
        {
            return;
        }
        rectTrans.anchoredPosition = Vector2.Lerp(from, to, t);
    }

    RectTransform rectTrans = null;
    public Vector2 from = Vector2.zero;
    public Vector2 to = Vector2.zero;
}