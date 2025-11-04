using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : ScreenUI
{
    [SerializeField] Joystick joystick;
    [SerializeField] Button pickUpBtn;
    public Joystick Joystick => joystick;
    public override void Initialize(UIManager uiManager)
    {
        pickUpBtn.onClick.AddListener(OnPickUp);
    }
    private void OnPickUp()
    {
        PlayerController.Instance.PickUp();
    }
}
