using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BoosterItemUI : MonoBehaviour
{
    public BoosterType type;
    public Image timeLeftBar;
    public Animator animator;
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        if (active)
        {
            transform.SetAsLastSibling();
        }
    }
    Tween tween;
    public void OnTime(float time, Action onTimeOut = null)
    {
        if(tween != null) tween.Kill();
        tween = timeLeftBar.DOFillAmount(0, time).From(1).SetEase(Ease.Linear).OnComplete(() =>
        {
            animator.Play("EndBooster");
            GameManager.Instance.Delay(0.34f, () =>
            {
                SetActive(false);
            });
            onTimeOut?.Invoke();
        });
    }
}
