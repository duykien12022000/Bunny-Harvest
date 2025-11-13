using DG.Tweening;
using TMPro;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    Tween currentTween;
    public CharacterSO CharacterContainer;
    public MapSO MapContainer;
    public static int CurrCharID
    {
        set { PlayerPrefs.SetInt("curChar", value); }
        get { return PlayerPrefs.GetInt("curChar", 0); }
    }
    public static int CurrMapID
    {
        set { PlayerPrefs.SetInt("curMap", value); }
        get { return PlayerPrefs.GetInt("curMap", 0); }
    }
    public static int Radish
    {
        set { PlayerPrefs.SetInt("RADISH", value); }
        get { return PlayerPrefs.GetInt("RADISH", 0); }
    }
    public static int MaxtHeart
    {
        set { PlayerPrefs.SetInt("MAX_HEART", value); }
        get { return PlayerPrefs.GetInt("MAX_HEART", 4); }
    }
    public static int Tutorial
    {
        set { PlayerPrefs.SetInt("Tutorial", value); }
        get { return PlayerPrefs.GetInt("Tutorial", 0); }
    }
    protected override void Awake()
    {
        base.Awake();
        var skinDefault = CharacterContainer.GetCharacter(0);
        if (skinDefault.isBought == 0)
        {
            skinDefault.isBought = 999;
        }
        var mapDefault = MapContainer.GetMap(0);
        if (mapDefault.isBought == 0)
        {
            mapDefault.isBought = 999;
        }
    }
    public void AnimateTo(int startValue, int targetValue, TextMeshProUGUI numberText)
    {
        if (currentTween != null) currentTween.Kill();
        currentTween = DOTween.To(() => startValue, x =>
        {
            numberText.text = Mathf.RoundToInt(x).ToString();
        }, targetValue, 1f).SetEase(Ease.OutCubic);
    }
    public bool UsingRadish(int value)
    {
        if (value < 0 && Radish + value < 0) return false;
        Radish += value;
        return true;
    }
}