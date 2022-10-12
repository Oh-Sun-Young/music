using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject goUI = null;

    [SerializeField] AudioSource bgmPlayer = null;

    float timer;
    float playTime;

    UIManager theUIManager;
    StageMenu theStageMenu;

    private void Awake()
    {
        theUIManager = FindObjectOfType<UIManager>();
        theStageMenu = FindObjectOfType<StageMenu>();
    }

    private void Start()
    {
        ShowGameOver(false);
    }

    private void Update()
    {
        if (GameManager.instance.isStartGame)
        {
            if (playTime < timer)
            {
                ShowGameOver(true);
            }
            else if (playTime - 1 < timer)
            {
                timer += Time.deltaTime;
            }
            else if (timer < playTime)
            {
                if (bgmPlayer.isPlaying)
                {
                    timer += Time.deltaTime;
                }
            }
        }
    }

    public void Initialized()
    {
        timer = 0;
        playTime = theStageMenu.GetMusicTime();
    }

    public void ShowGameOver(bool active)
    {
        goUI.SetActive(active);
        theUIManager.TempEnable(active);
    }

    public void BtnMainMenu()
    {
        goUI.SetActive(false);
        GameManager.instance.MainMenu();
    }
}
