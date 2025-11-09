using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : ScreenUI
{
    [SerializeField] RectTransform tabMain, tabSkin;
    [SerializeField] TextMeshProUGUI radishTxt, skinCostTxt;

    [SerializeField] Button playBtn, skinBtn, settingBtn, nextBtn, preBtn, buyBtn, closeBtn;

    private Vector2 tabMainDefaultPos;
    private CharacterSO characterSO => DataManager.Instance.CharacterContainer;
    private Character curCharData;
    private GameObject curChar;
    int index;

    public override void Initialize(UIManager uiManager)
    {
        this.uiManager = uiManager;
        playBtn.onClick.AddListener(OnPlay);
        skinBtn.onClick.AddListener(OnOpenSkinTab);
        settingBtn.onClick.AddListener(OnSetting);
        nextBtn.onClick.AddListener(OnNextSkin);
        preBtn.onClick.AddListener(OnPreSkin);
        buyBtn.onClick.AddListener(OnBuySkin);
        closeBtn.onClick.AddListener(OnCloseSkinTab);
        tabSkin.gameObject.SetActive(false);
        tabMainDefaultPos = tabMain.anchoredPosition;
        CreatCharacter(DataManager.CurrCharID);
        index = curCharData.id;
        UpdateRadish();
    }

    private void OnPlay()
    {
        uiManager.ActiveScreen<InGameUI>();
        Deactive();
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

    }

    private void OnOpenSkinTab()
    {
        tabMain.DOAnchorPosX(435f, 0.35f).SetEase(Ease.InBack).OnComplete(() =>
        {
            tabSkin.gameObject.SetActive(true);
        });
        CameraController.Instance.MoveTo(new Vector3(0, -4.75f, 4.5f), 0.35f, 0f, Ease.Linear);
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

    }

    private void OnSetting()
    {
        uiManager.ShowPopup<PopupSettingMain>(null);
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

    }

    private void OnNextSkin()
    {
        if (index < characterSO.GetLenght() - 1) index++;
        else index = 0;
        CreatCharacter(index);
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

    }

    private void OnPreSkin()
    {
        if (index > 0) index--;
        else index = characterSO.GetLenght() - 1;
        CreatCharacter(index);
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

    }

    private void OnBuySkin()
    {
        var c = characterSO.GetCharacter(index);
        if (DataManager.Instance.UsingRadish(-curCharData.radish))
        {
            curCharData.isBought++;
            DataManager.CurrCharID = curCharData.id;
            UpdateRadish();
            buyBtn.gameObject.SetActive(false);
        }
        else
        {
            skinCostTxt.DOKill();
            Sequence seq = DOTween.Sequence();

            seq.Append(skinCostTxt.DOColor(Color.red, 0.1f))
               .Append(skinCostTxt.DOColor(Color.white, 0.1f))
               .SetLoops(3)
               .OnComplete(() => skinCostTxt.color = Color.white);
        }
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

    }

    private void OnCloseSkinTab()
    {
        tabMain.DOAnchorPosX(tabMainDefaultPos.x, 0.35f).SetEase(Ease.OutBack);
        CameraController.Instance.MoveTo(new Vector3(0.7f, -4.75f, 4.5f), 0.35f, 0f, Ease.Linear);
        tabSkin.gameObject.SetActive(false);
        CreatCharacter(DataManager.CurrCharID);
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);


    }
    public void UpdateRadish()
    {
        radishTxt.text = DataManager.Radish.ToString();
    }
    private void CreatCharacter(int id)
    {
        if (curChar != null) Destroy(curChar);
        curCharData = characterSO.GetCharacter(id);
        skinCostTxt.text = curCharData.radish.ToString();
        buyBtn.gameObject.SetActive(curCharData.isBought == 0);
        curChar = Instantiate(characterSO.GetCharacter(id).prefabs);
        curChar.transform.SetParent(PlayerController.Instance.root);
        curChar.transform.localPosition = Vector3.zero;
        PlayerController.Instance.RebindAnimator();
    }
}
