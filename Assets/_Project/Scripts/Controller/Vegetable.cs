using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Vegetable : ObjectInteract
{
    [SerializeField] Image processClaimImg;
    [SerializeField] SizeAndPos youngScale, mediumScale, fullyScale;
    private float timeYoung, timeGrownUp, timeGold;

    public override void Initialize()
    {
        base.Initialize();
        timeYoung = Random.Range(3f, 6f);
        timeGrownUp = Random.Range(5f, 8f);
        timeGold = Random.Range(8f, 10f);

        youngScale.SetValue(model.transform);
    }
    protected override void UpdateLogic()
    {
        base.UpdateLogic();
        if (currentState == State.SMALL)
        {
            timeYoung -= Time.deltaTime;
            if (timeYoung < 0)
            {
                timeYoung = 0;
                mediumScale.SetValue(model.transform);
                SwitchSate(State.MEDIUM);
            }
        }
        if (currentState == State.MEDIUM)
        {
            timeGrownUp -= Time.deltaTime;
            if (timeGrownUp < 0)
            {
                timeGrownUp = 0;
                fullyScale.SetValue(model.transform);
                SwitchSate(State.FULLY);
            }
        }
    }
    public override void OnClaiming()
    {
        if (isClaimed) return;
        isClaimed = true;
        model.SetActive(false);
    }
    public override void CancelClaim()
    {
    }
}
[Serializable]
public struct SizeAndPos
{
    public Vector3 scaleValue;
    public Vector3 posValue;
    public void SetValue(Transform target)
    {
        target.DOScale(scaleValue, 0.2f).SetEase(Ease.Linear);
        target.DOLocalMove(posValue, 0.2f).SetEase(Ease.Linear);
    }
}
