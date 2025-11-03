//using System;
//using TMPro;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class PopupWin : PopupUI
//{
//    [SerializeField] private Button nextLvBtn, replayBtn, homeBtn;
//    [SerializeField] private TextMeshProUGUI coinTxt;
//    [SerializeField] private GameObject perfectObj;
//    int coin;
//    public override void Initialize(UIManager manager)
//    {
//        base.Initialize(manager);
//        nextLvBtn.onClick.AddListener(OnNextLevel);
//        replayBtn.onClick.AddListener(OnReplay);
//        homeBtn.onClick.AddListener(() =>
//        {
//            AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);
//            LevelManager.Coin += coin;
//            LevelManager.Level++;
//            GameManager.NEW_LEVEL = false;
//            SceneManager.LoadScene(0);
//        });
//        SetAllBtnInteract(true);
//    }
//    private void OnReplay()
//    {
//        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);
//        replayBtn.interactable = false;
//        GameManager.Instance.ReloadScene();
//    }
//    public override void Show(Action onClose)
//    {
//        base.Show(onClose);
//        coin = LevelController.Instance.IsAllPerfect() ? 200 : 100;
//        coinTxt.text = "+ " + coin.ToString();
//        perfectObj.SetActive(LevelController.Instance.IsAllPerfect());
//    }

//    private void SetAllBtnInteract(bool isInteractable)
//    {
//        nextLvBtn.interactable = isInteractable;
//        replayBtn.interactable = isInteractable;
//    }
//    private void OnNextLevel()
//    {
//        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);
//        LevelManager.Coin += coin;
//        SetAllBtnInteract(false);
//        LevelManager.Level++;
//        GameManager.Instance.ReloadScene();
//    }
//}
