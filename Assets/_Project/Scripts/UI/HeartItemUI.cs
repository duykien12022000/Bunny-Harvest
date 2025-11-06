using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HeartItemUI : MonoBehaviour
{
    [SerializeField] private Image heart;

    private static readonly Color VisibleColor = Color.white;
    private static readonly Color HiddenColor = new Color(1f, 1f, 1f, 0f);

    public void OnSpawn()
    {
        Kill();
        heart.color = HiddenColor;
        heart.transform.localScale = Vector3.one * 1.1f;
        Sequence seq = DOTween.Sequence();
        seq.Join(heart.DOColor(VisibleColor, 0.2f).SetEase(Ease.OutQuad));
        seq.Join(heart.transform.DOScale(1f, 0.2f).SetEase(Ease.InOutQuad));
    }

    public void OnLost()
    {
        Kill();
        Sequence seq = DOTween.Sequence();
        seq.Join(heart.DOColor(HiddenColor, 0.2f).SetEase(Ease.OutQuad));
        seq.Join(heart.transform.DOScale(1.1f, 0.2f).SetEase(Ease.InOutQuad));
    }
    private void Kill()
    {
        DOTween.Kill(heart);
        DOTween.Kill(heart.transform);
    }
}
