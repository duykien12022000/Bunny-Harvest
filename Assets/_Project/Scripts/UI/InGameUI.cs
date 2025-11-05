using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class InGameUI : ScreenUI
{
    [SerializeField] Joystick joystick;
    [SerializeField] Button pickUpBtn;
    [SerializeField] TextMeshProUGUI scoreTxt;
    public Joystick Joystick => joystick;
    public override void Initialize(UIManager uiManager)
    {
        pickUpBtn.onClick.AddListener(OnPickUp);
        scoreTxt.text = "0x";
    }
    private void OnPickUp()
    {
        PlayerController.Instance.PickUp();
    }
    public void UpdateCurrentScore(int score)
    {
        scoreTxt.transform.localScale = Vector3.one;
        scoreTxt.transform.DOScale(1.3f, 0.15f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                scoreTxt.transform.DOScale(1f, 0.15f)
                    .SetEase(Ease.InQuad);
            });
        scoreTxt.text = $"{score.ToString()}x";
    }
}
