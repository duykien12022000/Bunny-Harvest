using DG.Tweening;
using FastFood;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : ScreenUI
{
    [SerializeField] Joystick joystick;
    public Joystick Joystick => joystick;
    public override void Initialize(UIManager uiManager)
    {
    }
}
