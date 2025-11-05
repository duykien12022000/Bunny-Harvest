using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    Tween currentTween;

    public static int BestScore
    {
        set { PlayerPrefs.SetInt("BEST_SCORE", value); }
        get { return PlayerPrefs.GetInt("BEST_SCORE", 0); }
    }
    public void AnimateTo(int startValue, int targetValue, TextMeshProUGUI numberText)
    {
        if (currentTween != null) currentTween.Kill();
        currentTween = DOTween.To(() => startValue, x => {
            numberText.text = Mathf.RoundToInt(x).ToString();
        }, targetValue, 1f).SetEase(Ease.OutCubic);
    }
}