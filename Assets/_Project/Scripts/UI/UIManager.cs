//using BaseService;
using DG.Tweening;
using MyPlugins;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
namespace RPGFantasy.EditorClass
{
    using UnityEditor;
    [CustomEditor(typeof(UIManager))]
    public class UIManagerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Get References"))
            {
                UIManager u = target as UIManager;
                u.screens = FindObjectsOfType<ScreenUI>(true);
                //u.topUI = u.GetComponentInChildren<TopUI>(true);
                //u.vfxUI = u.GetComponentInChildren<VfxUI>(true);
                u.canvasScaler = u.GetComponentsInChildren<CanvasScaler>(true);
                //u.popups = FindObjectsOfType<PopupUI>(true);
                PopupUI[] popups = Resources.LoadAll<PopupUI>("Popup/");
                for (int i = 0; i < popups.Length; i++)
                {
                    string nname = popups[i].GetType().Name;
                    Debug.Log(nname);
                    popups[i].gameObject.name = nname;
                    string pa = AssetDatabase.GetAssetPath(popups[i].gameObject);
                    AssetDatabase.RenameAsset(pa, nname);
                    AssetDatabase.SaveAssetIfDirty(popups[i].gameObject);
                }
                EditorUtility.SetDirty(u);
            }
        }
    }
}

#endif
public class UIManager : Singleton<UIManager>//, IAdsUI
{
    //#region IAdsUI
    //public void RequestAdsNativeFullscreen()
    //{
    //}

    //public void ShowLoadingInterScreen(bool show)
    //{
    //}

    //public void ShowPopupNativeFullscreen(bool show)
    //{
    //}

    //public void ShowWelcomeBack(bool show)
    //{
    //    if (show)
    //    {
    //        ShowPopup<WelcomeBackPopup>(null);
    //    }
    //    else
    //    {
    //        GetPopup<WelcomeBackPopup>().Hide();
    //    }
    //}
    //#endregion

    public ScreenUI[] screens;
    //public NotifyTeamPower notifyWorld;
    //public TopUI topUI;
    //public VfxUI vfxUI;
    // public PopupContainerSO popupContainer;
    //public SaveBatteryPanel saveBatteryPanel;
    //public FlashLightPanel flashLightPanel;
    //public TransitionUI transition;
    //public GameObject blockerUI;
    public RectTransform screenHolder;
    public RectTransform popupHolder;
    public CanvasScaler[] canvasScaler;
    public List<PopupUI> listPopupCached;
    public List<PopupUI> listPopupExist;
    private GameManager manager;
    public Camera UICamera;
    public void Initialize(GameManager manager)
    {

        //AdsUtils.SetAdsUI(this);
        this.manager = manager;
        listPopupCached = new List<PopupUI>();
        listPopupCached = popupHolder.GetComponentsInChildren<PopupUI>(true).ToList();
        for (int i = 0; i < canvasScaler.Length; i++)
        {
            canvasScaler[i].matchWidthOrHeight = 1;
            if (Utils.isiPad())
            {
                canvasScaler[i].matchWidthOrHeight = 0f;
            }
        }
        for (int i = 0; i < listPopupCached.Count; i++)
        {
            listPopupCached[i].Initialize(this);
        }
        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].Initialize(this);
        }
        listPopupExist = new List<PopupUI>(listPopupCached);
        //blockerUI.SetActive(false);
        //topUI.Initialize(this);
        //saveBatteryPanel.Initialize();
        PopupUI.OnDestroyPopup += OnPopupDestroyed;
        //GameManager.OnPause += Pause;
        //GameManager.OnResume += Resume;
    }

    private void Pause()
    {
        // blockerUI.SetActive(true);
    }
    private void Resume()
    {
        // blockerUI.SetActive(false);
    }
    private void OnPopupDestroyed(PopupUI obj)
    {
        if (listPopupCached.Contains(obj))
        {
            listPopupCached.Remove(obj);
        }
        listPopupExist.Remove(obj);
        Destroy(obj.gameObject);
    }

    public void DeactiveAllScreen(bool showTopUI = false)
    {
        //topUI.SetActive(showTopUI);
        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].Deactive();
        }
    }
    public T ActiveScreen<T>(bool hideAll = true, bool showTopUI = false, bool lockTopUI = false) where T : ScreenUI
    {
        //topUI.isLock = lockTopUI;
        //if (!showTopUI)
        //{
        //    topUI.SetActive(false, true);
        //}
        //else
        //{
        //    topUI.SetActive(true);
        //}
        T screen = default;
        for (int i = 0; i < screens.Length; i++)
        {
            if (screens[i] is T)
            {
                screens[i].Active();
                screen = screens[i].GetComponent<T>();
            }
            else
            {
                if (hideAll)
                {
                    screens[i].Deactive();
                }
            }
        }
        return screen;
    }
    public T GetScreen<T>() where T : ScreenUI
    {
        for (int i = 0; i < screens.Length; i++)
        {
            if (screens[i] is T)
            {
                return screens[i].GetComponent<T>();
            }
        }
        return default;
    }
    public T ShowPopup<T>(System.Action onClose) where T : PopupUI
    {
        for (int i = 0; i < listPopupCached.Count; i++)
        {
            if (listPopupCached[i] is T)
            {
                listPopupCached[i].Show(onClose);
                listPopupCached[i].transform.SetAsLastSibling();
                return listPopupCached[i].GetComponent<T>();
            }
        }
        T popup = CreatePopup<T>();
        popup.Show(onClose);
        popup.transform.SetAsLastSibling();
        return popup;
    }
    private T CreatePopup<T>() where T : PopupUI
    {
        string popupName = typeof(T).Name;
        T popup = Instantiate(Resources.Load<T>("Popup/" + popupName), popupHolder);
        popup.Initialize(this);
        listPopupExist.Add(popup);
        if (popup.isCache)
        {
            listPopupCached.Add(popup);
        }
        return popup;
    }
    public T GetPopup<T>() where T : PopupUI
    {
        T popup = default;
        for (int i = 0; i < listPopupCached.Count; i++)
        {
            if (listPopupCached[i] is T)
            {
                popup = listPopupCached[i].GetComponent<T>();
                return popup;
            }
        }
        popup = CreatePopup<T>();
        return popup;
    }
    public bool HasPopupShowing()
    {
        foreach (var item in listPopupExist)
        {
            if (item.isShowing) return true;
        }
        return false;
    }
    public void EffectText(Text t)
    {
        t.DOKill();
        t.rectTransform.DOScale(1.2f, 0.3f).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            t.rectTransform.localScale = Vector3.one;
            t.rectTransform.DOKill();
        });
        t.DOColor(Color.yellow, 0.3f).OnComplete(() =>
        {
            t.color = Color.white;
            t.DOKill();
        });
    }
}
public static class CanvasPositioningExtensions
{
    public static Vector3 WorldToCanvasPosition(this Canvas canvas, Vector3 worldPosition, Camera camera = null)
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
        var viewportPosition = camera.WorldToViewportPoint(worldPosition);
        return canvas.ViewportToCanvasPosition(viewportPosition);
    }

    public static Vector3 ScreenToCanvasPosition(this Canvas canvas, Vector3 screenPosition)
    {
        var viewportPosition = new Vector3(screenPosition.x / Screen.width, screenPosition.y / Screen.height, 0);
        return canvas.ViewportToCanvasPosition(viewportPosition);
    }

    public static Vector3 ViewportToCanvasPosition(this Canvas canvas, Vector3 viewportPosition)
    {
        var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
        var canvasRect = canvas.GetComponent<RectTransform>();
        var scale = canvasRect.sizeDelta;
        return Vector3.Scale(centerBasedViewPortPosition, scale);
    }
}
