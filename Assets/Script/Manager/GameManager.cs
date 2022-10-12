using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject goTitleUI = null;
    [SerializeField] GameObject[] goGameUI = null;

    public static GameManager instance;
    public bool isStartGame = false;
    public bool isPauseCheck = false;

    NoteManager theNoteManager;
    ScoreManager theScoreManager;
    TimingManager theTimingManager;
    PlayerController thePlayerController;
    EffectManager theEffectManager;
    UIManager theUIManager;
    StageManager theStageManager;
    Pause thePause;
    GameOver theGameOver;
    CenterFrame theCenterFrame;

    [SerializeField] CenterFrame theMusic = null; // 비활성화된 객체는 FindObjectOfType으로 찾을 수 없음

    int SongNumber = 0;
    int bpm = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        theNoteManager = FindObjectOfType<NoteManager>();
        theScoreManager = FindObjectOfType<ScoreManager>();
        theTimingManager = FindObjectOfType<TimingManager>();
        thePlayerController = FindObjectOfType<PlayerController>();
        theEffectManager = FindObjectOfType<EffectManager>();
        theUIManager = FindObjectOfType<UIManager>();
        theStageManager = FindObjectOfType<StageManager>();
        thePause = FindObjectOfType<Pause>();
        theGameOver = FindObjectOfType<GameOver>();
        theCenterFrame = FindObjectOfType<CenterFrame>();
    }

    private void Start()
    {
        GameStart(false);

    }

    public void GameStart(bool active, int p_SongNumber = 0, int p_bpm = 0)
    {
        if (active)
        {
            SongNumber = p_SongNumber;
            bpm = p_bpm;
        }

        for (int i = 0; i < goGameUI.Length; i++)
        {
            goGameUI[i].SetActive(active);
        }

        if (active)
        {
            theNoteManager.bpm = p_bpm;
            Initialized(p_SongNumber);
        }
        
        isStartGame = active;

    }

    void Initialized(int p_SongNumber)
    {
        theStageManager.RemoveStage();
        theStageManager.SettingStage(p_SongNumber);
        theScoreManager.Initialized();
        theTimingManager.Initialized();
        thePlayerController.Initialized();
        theEffectManager.Initialized();
        theUIManager.TempEnable(false);
        AudioManager.instance.StopBGM();
        theMusic.bgmName = "BGM" + p_SongNumber;
        theGameOver.Initialized();
        theCenterFrame.Initialized();
    }

    public void GamePause()
    {
        isPauseCheck = true;
        thePause.ShowPause(true);
    }

    public void MainMenu()
    {
        isPauseCheck = false;
        AudioManager.instance.StopBGM();

        GameStart(false);
        theStageManager.RemoveStage();
        goTitleUI.SetActive(true);
    }

    public void GameRetry()
    {
        isPauseCheck = false;

        FindObjectOfType<Result>().HideResult();
        thePause.ShowPause(false);
        theGameOver.ShowGameOver(false);
        GameStart(true, SongNumber, bpm);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
