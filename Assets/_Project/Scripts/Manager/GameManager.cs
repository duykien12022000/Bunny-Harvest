using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] UIManager uiManager;
    //[SerializeField] LevelManager levelManager;
    [SerializeField] GameController gameController;
    public static GameState currentState { get; private set; }
    public static GameMode currentMode { get; private set; }
    public static bool NEW_LEVEL = false;
    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        SwitchGameState(NEW_LEVEL ? GameState.PLAY : GameState.NONE);
    }
    private void Start()
    {
        AudioManager.Instance.PlayMusic("BGM", 1, true);
        uiManager.Initialize(this);
        if (NEW_LEVEL)
        {
            uiManager.ActiveScreen<InGameUI>();
        }
        else
        {
            uiManager.ActiveScreen<HomeUI>();
        }

        gameController.Initialize();
    }
    public void ReloadScene()
    {
        NEW_LEVEL = true;
        SceneManager.LoadScene(0);
    }
    private void FixedUpdate()
    {
        gameController.UpdatePhysic();
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.V))
        {
            DataManager.Radish += 100000;
        }
#endif
    }
    public void SwitchGameState(GameState newState)
    {
#if UNITY_EDITOR
        Debug.Log($"<color=#9CDCE1>=> STATUS _ GAME-STATE: {newState}</color>");
#endif
        currentState = newState;
    }
    public void SwitchGameMode(GameMode newMode)
    {
#if UNITY_EDITOR
        Debug.Log($"<color=#9CDCE1>=> STATUS _ GAME-MODE: {newMode}</color>");
#endif
        currentMode = newMode;
    }

    public void Delay(float duration, Action onComplete)
    {
        StartCoroutine(IECallBack());
        IEnumerator IECallBack()
        {
            float t = duration;
            while (t > 0)
            {
                t -= Time.deltaTime;
                yield return null;
            }
            onComplete?.Invoke();
        }
    }
}
public enum GameState
{
    NONE, TUTORIAL, PLAY, LOSE, WIN, PAUSE
}
public enum GameMode
{
    IN_GAME
}

