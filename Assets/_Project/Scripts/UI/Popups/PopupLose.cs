using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupLose : PopupUI
{
    [SerializeField] private Button replayBtn, homeBtn;
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
    private void OnReplay()
    {
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);
        replayBtn.interactable = false;
        GameManager.Instance.ReloadScene();
    }
}
