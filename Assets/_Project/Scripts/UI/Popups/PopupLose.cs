using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupLose : PopupUI
{
    [SerializeField] private Button replayBtn, homeBtn;
    [SerializeField] private TextMeshProUGUI currentRadish, totalRadish;
    public override void Initialize(UIManager manager)
    {
        base.Initialize(manager);
        replayBtn.onClick.AddListener(OnReplay);
        homeBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);
            GameManager.NEW_LEVEL = false;
            SceneManager.LoadScene(0);
        });
    }
    public override void Show(Action onClose)
    {
        base.Show(onClose);
        currentRadish.text = $"+{GameController.CurretnRadish}";
        DataManager.Instance.UsingRadish(GameController.CurretnRadish);
        totalRadish.text = $"Total: {DataManager.Radish}";
    }
    private void OnReplay()
    {
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);
        replayBtn.interactable = false;
        GameManager.Instance.ReloadScene();
    }
}
