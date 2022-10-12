using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject goUI = null;

    UIManager theUIManager;
    CenterFrame theCenterFrame;

    private void Awake()
    {
        theUIManager = FindObjectOfType<UIManager>();
        theCenterFrame = FindObjectOfType<CenterFrame>();
    }

    void Start()
    {
        goUI.SetActive(false);
    }

    public void ShowPause(bool active)
    {
        theCenterFrame.ResetMusic(); // FindObjectOfType : Update처럼 반복해서 호출되는 건 과부화 문제가 발생할 수 있기에 피하자

        goUI.SetActive(active);
        theUIManager.TempEnable(active);

        if (active)
        {
            AudioManager.instance.PauseBGM();
        }
    }

    public void BtnPlay()
    {
        ShowPause(false);
        theUIManager.TempEnable(false);
        AudioManager.instance.PlayBGM();
        GameManager.instance.isPauseCheck = false;
    }

    public void BtnMainMenu()
    {
        goUI.SetActive(false);
        GameManager.instance.MainMenu();
    }
}
