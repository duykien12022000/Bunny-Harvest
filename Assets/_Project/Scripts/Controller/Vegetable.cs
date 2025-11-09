using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Vegetable : ObjectInteract
{
    [SerializeField] Image processClaimImg;
    [SerializeField] SizeAndPos youngScale, mediumScale, fullyScale;
    [SerializeField] GameObject display, impact;

    [SerializeField] Sprite[] spriteStatus;
    [SerializeField] AudioClip flySFX, breakSFX;
    public bool IsAvailable2Claim;

    private float timeYoung, timeGrownUp, timeGold;
    private Rigidbody rb;
    private Collider m_collider;
    public override void Initialize()
    {
        base.Initialize();
        timeYoung = Random.Range(3f, 6f);
        timeGrownUp = Random.Range(5f, 8f);
        timeGold = Random.Range(8f, 10f);

        youngScale.SetValue(model.transform);
        rb = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        EnableCollider(true);
        display.SetActive(true);
        impact.SetActive(false);
        SetStatus(false);
    }
    public void EnableCollider(bool enable)
    {
        m_collider.enabled = enable;
    }
    public void SetStatus(bool isActive)
    {
        processClaimImg.gameObject.SetActive(isActive);
        if (!isActive) return;
        switch (currentState)
        {
            case State.SMALL:
                processClaimImg.sprite = spriteStatus[0];
                break;
            case State.MEDIUM:
                processClaimImg.sprite = spriteStatus[1];
                break;
            case State.FULLY:
                processClaimImg.sprite = spriteStatus[2];
                break;
        }
    }
    protected override void UpdateLogic()
    {
        base.UpdateLogic();
        if (currentState == State.SMALL)
        {
            IsAvailable2Claim = false;
            timeYoung -= Time.deltaTime;
            if (timeYoung < 0)
            {
                timeYoung = 0;
                mediumScale.SetValue(model.transform);
                SwitchSate(State.MEDIUM);
                IsAvailable2Claim = true;
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
    public Vector3 InitPos { private set; get; }
    public IEnumerator Curve(Vector3 start, Vector3 finish)
    {
        EnableCollider(false);
        SetStatus(false);
        InitPos = transform.position;
        float timePast = 0f;
        AudioManager.Instance.PlayOneShot(flySFX, 2);
        while (timePast < duration)
        {
            timePast += Time.deltaTime;

            float linearTime = timePast / duration;
            float heightTime = curve.Evaluate(linearTime);

            float height = Mathf.Lerp(0f, maxHeightY, heightTime);
            Vector3 nextPos = Vector3.Lerp(start, finish + new Vector3(0, 0.3f, 0), linearTime) + new Vector3(0f, height, 0f);
            Vector3 dir = nextPos - transform.localPosition;
            dir.Normalize();
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
            transform.localPosition = nextPos;

            yield return null;
        }
        AudioManager.Instance.PlayOneShot(breakSFX, 2);

        display.SetActive(false);
        CreateBooster();
        impact.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        GameController.Instance.DespawnVegetable(this);
    }
    public override void CancelClaim()
    {
    }
    private void CreateBooster()
    {
        int rand = Random.Range(0, 100); // 0..99
        string boosterName = "";

        if (rand < 5)                    // 0–4 → 5%
        {
            boosterName = "MagnetItem";
        }
        else if (rand < 15)              // 5–14 → 10%
        {
            boosterName = "HeartItem";
        }
        else if (rand < 20)              // 15–19 → 5%
        {
            boosterName = "ShieldItem";
        }
        else if (rand < 35)              // 20–34 → 15%
        {
            boosterName = "SpeedUpItem";
        }
        else                             // 35–99 → 65% không rơi
        {
            boosterName = "";
        }

        if (!string.IsNullOrEmpty(boosterName))
        {
            var b = FactoryObject.Spawn<Booster>("Booster", boosterName);
            b.Initialize();
            b.transform.position = new Vector3(transform.position.x, 0.75f, transform.position.z);
        }
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
