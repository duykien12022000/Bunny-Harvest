using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : ScreenUI
{
    [SerializeField] RectTransform tabMain, tabSkin, tabMap;
    [SerializeField] TextMeshProUGUI radishTxt, skinCostTxt, mapCostTxt;

    [SerializeField] Button playBtn, skinBtn, mapBtn, settingBtn, nextBtn, nextMapBtn, preBtn, preMapBtn, buyBtn, buyMapBtn, closeBtn, closeMapBtn;
    [SerializeField] Image mapDemo;
    private Vector2 tabMainDefaultPos;
    private CharacterSO characterSO => DataManager.Instance.CharacterContainer;
    private MapSO mapSO => DataManager.Instance.MapContainer;
    private Character curCharData;
    private GameObject curChar;

    private MapData curMapData;
    private MapController curMap;

    int index, indexMap;

    public override void Initialize(UIManager uiManager)
    {
        this.uiManager = uiManager;
        playBtn.onClick.AddListener(OnPlay);
        skinBtn.onClick.AddListener(OnOpenSkinTab);
        mapBtn.onClick.AddListener(OnOpenMapTab);
        settingBtn.onClick.AddListener(OnSetting);
        nextBtn.onClick.AddListener(OnNextSkin);
        preBtn.onClick.AddListener(OnPreSkin);

        nextMapBtn.onClick.AddListener(OnNextMap);
        preMapBtn.onClick.AddListener(OnPreMap);

        buyBtn.onClick.AddListener(OnBuySkin);
        buyMapBtn.onClick.AddListener(OnBuyMap);
        closeBtn.onClick.AddListener(OnCloseSkinTab);
        closeMapBtn.onClick.AddListener(OnCloseMapTab);
        tabSkin.gameObject.SetActive(false);
        tabMap.gameObject.SetActive(false);
        tabMainDefaultPos = tabMain.anchoredPosition;
        CreatCharacter(DataManager.CurrCharID);
        if (GameManager.NEW_LEVEL)
        {
            SetUpMapReplay();
        }
        else
        {
            CreatMap(DataManager.CurrMapID, () =>
            {
                indexMap = curMapData.id;
            });
        }
        index = curCharData.id;
        UpdateRadish();
    }

    private void OnPlay()
    {
        uiManager.ActiveScreen<InGameUI>();
        curMap.SetUpMap();

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
    private void OnOpenMapTab()
    {
        tabMain.DOAnchorPosX(435f, 0.35f).SetEase(Ease.InBack).OnComplete(() =>
        {
            tabMap.gameObject.SetActive(true);
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
    private void OnNextMap()
    {
        if (indexMap < mapSO.GetLenght() - 1) indexMap++;
        else indexMap = 0;
        CreatMap(indexMap);
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

    }

    private void OnPreMap()
    {
        if (indexMap > 0) indexMap--;
        else indexMap = mapSO.GetLenght() - 1;
        CreatMap(indexMap);
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

    }
    private void OnBuySkin()
    {
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
    private void OnBuyMap()
    {
        if (DataManager.Instance.UsingRadish(-curCharData.radish))
        {
            curMapData.isBought++;
            DataManager.CurrMapID = curMapData.id;
            UpdateRadish();
            buyMapBtn.gameObject.SetActive(false);
        }
        else
        {
            mapCostTxt.DOKill();
            Sequence seq = DOTween.Sequence();

            seq.Append(mapCostTxt.DOColor(Color.red, 0.1f))
               .Append(mapCostTxt.DOColor(Color.white, 0.1f))
               .SetLoops(3)
               .OnComplete(() => mapCostTxt.color = Color.white);
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
    private void OnCloseMapTab()
    {
        tabMain.DOAnchorPosX(tabMainDefaultPos.x, 0.35f).SetEase(Ease.OutBack);
        CameraController.Instance.MoveTo(new Vector3(0.7f, -4.75f, 4.5f), 0.35f, 0f, Ease.Linear);
        tabMap.gameObject.SetActive(false);
        CreatMap(DataManager.CurrMapID);
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
        if (curCharData.isBought > 0)
        {
            DataManager.CurrCharID = curCharData.id;
        }
        curChar = Instantiate(characterSO.GetCharacter(id).prefabs);
        curChar.transform.SetParent(PlayerController.Instance.root);
        curChar.transform.localPosition = Vector3.zero;
        PlayerController.Instance.RebindAnimator();
    }
    private void CreatMap(int id, Action OnComplete = null)
    {
        if (curMap != null)
        {
            Destroy(curMap);
            Destroy(curMap.gameObject);
        }
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            yield return null;
            curMapData = mapSO.GetMap(id);
            OnComplete?.Invoke();
            mapCostTxt.text = curMapData.radish.ToString();
            buyMapBtn.gameObject.SetActive(curMapData.isBought == 0);
            if (curMapData.isBought > 0)
            {
                DataManager.CurrMapID = curMapData.id;
            }
            mapDemo.sprite = curMapData.demoUI;
            curMap = Instantiate(mapSO.GetMap(id).prefabs);
            curMap.transform.SetParent(GameController.Instance.mapRoot);
            GameController.Instance.areaCtrl = curMap.AreaController;
        }

    }
    private void SetUpMapReplay()
    {
        curMap = Instantiate(mapSO.GetMap(DataManager.CurrMapID).prefabs);
        curMap.transform.SetParent(GameController.Instance.mapRoot);
        GameController.Instance.areaCtrl = curMap.AreaController;
        GameController.Instance.SetUpMap();
    }
}
