using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Result : MonoBehaviour
{
    [Header("Result UI")]
    [SerializeField] GameObject goUI = null;
    [SerializeField] TextMeshProUGUI[] txtCount = null;
    [SerializeField] TextMeshProUGUI txtScore = null;
    [SerializeField] TextMeshProUGUI txtMaxCombo = null;
    [SerializeField] TextMeshProUGUI txtGrade = null;

    ScoreManager theScoreManager;
    TimingManager theTimingManager;
    EffectManager theEffectManager;
    UIManager theUIManager;

    void Start()
    {
        theScoreManager = FindObjectOfType<ScoreManager>();
        theTimingManager = FindObjectOfType<TimingManager>();
        theEffectManager = FindObjectOfType<EffectManager>();
        theUIManager = FindObjectOfType<UIManager>();

        goUI.SetActive(false);
    }

    public void ShowResult()
    {
        FindObjectOfType<CenterFrame>().ResetMusic(); // FindObjectOfType : Update처럼 반복해서 호출되는 건 과부화 문제가 발생할 수 있기에 피하자
        
        goUI.SetActive(true);
        theUIManager.TempEnable(true);
        AudioManager.instance.StopBGM();

        for (int i = 0; i < txtCount.Length; i++)
        {
            txtCount[i].text = "0";
            txtScore.text = "0";
            txtMaxCombo.text = "0";
        }

        int[] t_judgement = theTimingManager.GetJudgementRecore();
        int t_currentScore = theScoreManager.GetCurrentScore();
        int t_maxCombo = theEffectManager.GetMaxCombo();
        int t_AllCount = 0;

        for (int i = 0; i < txtCount.Length; i++)
        {
            txtCount[i].text = string.Format("{0:#,##0}", t_judgement[i]);
            t_AllCount += t_judgement[i];
        }

        float t_AllScore = theScoreManager.increaseScore * t_AllCount;
        float t_grade = t_currentScore / t_AllScore * 100;

        txtScore.text = string.Format("{0:#,##0}", t_currentScore);
        txtMaxCombo.text = string.Format("{0:#,##0}", t_maxCombo);
        txtGrade.text = GetGrade(t_grade);
    }

    public void HideResult()
    {
        goUI.SetActive(false);
    }

    string GetGrade(float per)
    {
        string grade = "F";

        if (per > 95) grade = "S";
        else if (per > 85) grade = "A";
        else if (per > 70) grade = "B";
        else if (per > 50) grade = "C";
        else if (per > 30) grade = "D";
        else if (per > 10) grade = "E";

        return grade;
    }

    public void BtnMainMenu()
    {
        goUI.SetActive(false);
        GameManager.instance.MainMenu();
    }
}
