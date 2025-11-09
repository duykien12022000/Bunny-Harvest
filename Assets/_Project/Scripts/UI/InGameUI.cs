using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class InGameUI : ScreenUI
{
    [SerializeField] Joystick joystick;
    [SerializeField] Button pickUpBtn, jumpBtn, settingBtn, tutorialBtn;
    [SerializeField] TextMeshProUGUI scoreTxt, healthTxt, bonusHealthTxt;
    [SerializeField] BoosterItemUI[] booster;
    public BoosterItemUI GetBooster(BoosterType type)
    {
        for (int i = 0; i < booster.Length; i++)
        {
            if(booster[i].type == type)
            {
                return booster[i];
            }
        }
        return null;
    }
    public Joystick Joystick => joystick;
    public override void Initialize(UIManager uiManager)
    {
        this.uiManager = uiManager;
        pickUpBtn.onClick.AddListener(OnPickUp);
        jumpBtn.onClick.AddListener(OnJump);
        settingBtn.onClick.AddListener(() =>
        {
            uiManager.ShowPopup<PopupSettingMain>(null);
        });
        tutorialBtn.onClick.AddListener(() =>
        {
            uiManager.ShowPopup<PopupTutorial>(null);
        });
        scoreTxt.text = "0x";
        foreach(var item in booster)
        {
            item.SetActive(false);
        }
    }
    public override void Active()
    {
        base.Active();
        if (DataManager.Tutorial == 0)
        {
            uiManager.ShowPopup<PopupTutorial>(action);
            DataManager.Tutorial++;
            return;
        }
        action();
        void action()
        {
            GameManager.Instance.SwitchGameState(GameState.PLAY);
            CameraController.Instance.MoveTo(Vector3.zero, 0.35f, 0f, Ease.InQuad);
            CameraController.Instance.RotateTo(Vector3.right * 35f, 0.35f, 0, Ease.InQuad);
        }
    }
    private void OnPickUp()
    {
        PlayerController.Instance.PickUp();
    }
    private void OnJump()
    {
        PlayerController.Instance.HandleJumping();
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
    public void UpdateCurrentHealth(int amount)
    {
        Color color = amount > 0 ? Color.green : Color.red;
        healthTxt.transform.localScale = Vector3.one * 1.1f;
        Sequence seq = DOTween.Sequence().SetLoops(2, LoopType.Yoyo);
        seq.Join(healthTxt.DOColor(color, 0.2f).SetEase(Ease.OutQuad));
        seq.Join(healthTxt.transform.DOScale(1f, 0.2f).SetEase(Ease.InOutQuad));
        healthTxt.text = $"x{amount.ToString()}";
    }

}
