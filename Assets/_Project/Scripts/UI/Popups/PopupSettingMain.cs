using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupSettingMain : PopupUI
{
    [Serializable]
    public struct ButtonProperty
    {
        public Button button;
        public Image turnOffImg;
        public void AddListener(Action action)
        {
            button.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

                action?.Invoke();
            });
        }
    }

    [SerializeField] private ButtonProperty musicBtn, soundBtn, vibrationBtn;
    [SerializeField] private Button closeBtn, homeBtn;
    private bool isMusicOff, isSoundOff, isVibrationOff;
    public override void Initialize(UIManager manager)
    {
        base.Initialize(manager);
        musicBtn.AddListener(OnClickMusic);
        soundBtn.AddListener(OnClickSound);
        vibrationBtn.AddListener(OnClickVibration);
        closeBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);
            if (GameManager.currentState != GameState.NONE)
            {
                GameManager.Instance.SwitchGameState(GameState.PLAY);
            }
            Hide();
        });
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
        if (GameManager.currentState != GameState.NONE)
        {
            GameManager.Instance.SwitchGameState(GameState.PAUSE);
            homeBtn.gameObject.SetActive(true);
        }
        else
        {
            homeBtn.gameObject.SetActive(false);
        }
        OnInitSetting();
    }
    private void OnClickVibration()
    {
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

        if (isVibrationOff)
        {
            isVibrationOff = false;
            vibrationBtn.turnOffImg.DOFade(0, 0.2f);
        }
        else
        {
            isVibrationOff = true;
            vibrationBtn.turnOffImg.DOFade(1, 0.2f);
        }
        AudioManager.Instance.EnableVibra(!isVibrationOff);
    }

    private void OnClickSound()
    {
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

        if (isSoundOff)
        {
            isSoundOff = false;
            soundBtn.turnOffImg.DOFade(0, 0.2f);
        }
        else
        {
            isSoundOff = true;
            soundBtn.turnOffImg.DOFade(1, 0.2f);
        }
        AudioManager.Instance.EnableSound(!isSoundOff);
    }

    private void OnClickMusic()
    {
        AudioManager.Instance.PlayOneShot(SFXStr.CLICK, 2);

        if (isMusicOff)
        {
            isMusicOff = false;
            musicBtn.turnOffImg.DOFade(0, 0.2f);
        }
        else
        {
            isMusicOff = true;
            musicBtn.turnOffImg.DOFade(1, 0.2f);
        }
        AudioManager.Instance.EnableMusic(!isMusicOff);
    }
    private void OnInitSetting()
    {
        isMusicOff = AudioManager.MusicSetting == 0;
        isSoundOff = AudioManager.SoundSetting == 0;
        isVibrationOff = AudioManager.VibraSetting == 0;
        musicBtn.turnOffImg.DOFade(isMusicOff ? 1 : 0, 0.2f);
        soundBtn.turnOffImg.DOFade(isSoundOff ? 1 : 0, 0.2f);
        vibrationBtn.turnOffImg.DOFade(isVibrationOff ? 1 : 0, 0.2f);
    }
}
