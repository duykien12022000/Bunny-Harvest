using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class InGameUI : ScreenUI
{
    [SerializeField] Joystick joystick;
    [SerializeField] Button pickUpBtn;
    [SerializeField] TextMeshProUGUI scoreTxt, healthTxt;
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
