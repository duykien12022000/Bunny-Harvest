using DG.Tweening;
using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class Vegetable : ObjectInteract
{
    [SerializeField] Image processClaimImg;
    [SerializeField] SizeAndPos youngScale, mediumScale, fullyScale;
    private float timeYoung, timeGrownUp, timeGold;
    private Rigidbody rb;
    private bool attracted = false;
    public override void Initialize()
    {
        base.Initialize();
        timeYoung = Random.Range(3f, 6f);
        timeGrownUp = Random.Range(5f, 8f);
        timeGold = Random.Range(8f, 10f);

        youngScale.SetValue(model.transform);
        rb = GetComponent<Rigidbody>(); 
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
        StartCoroutine(Curve(transform.localPosition, player.transform.position));
    }
    [Header("Curve")]
    [SerializeField] AnimationCurve curve;
    [SerializeField] float duration = 1.0f;
    [SerializeField] float maxHeightY = 3.0f;
    public IEnumerator Curve(Vector3 start, Vector3 finish)
    {
        float timePast = 0f;
        while (timePast < duration)
        {
            timePast += Time.deltaTime;

            float linearTime = timePast / duration;
            float heightTime = curve.Evaluate(linearTime);

            float height = Mathf.Lerp(0f, maxHeightY, heightTime);
            Vector3 nextPos = Vector3.Lerp(start, finish, linearTime) + new Vector3(0f, height, 0f);
            Vector3 dir = nextPos - transform.localPosition;
            dir.Normalize();
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
            transform.localPosition = nextPos;

            yield return null;
        }
        GameController.Instance.DespawnVegetable(this);
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
