using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupLoading : MonoBehaviour
{
    [SerializeField] private Image loadingBar;
    [SerializeField] private TextMeshProUGUI loadingTxt;
    [SerializeField] private Animation m_animation;
    private void Start()
    {
        loadingBar.DOFillAmount(1f, 3f).From(0).SetEase(Ease.InOutSine).OnUpdate(() =>
        {
            loadingTxt.text = $"LOADING {(int)Math.Round(loadingBar.fillAmount * 100f)}%";
        }).OnComplete(() =>
        {
            m_animation.Play();
            DOVirtual.DelayedCall(m_animation.clip.length, () =>
            {
                gameObject.SetActive(false);
            });
        });
    }
}
