using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtScore = null;
    public int increaseScore = 10; // 증가할 기본 점수
    int currentScore = 0; // 현재 점수

    [SerializeField] float[] weight = null;
    [SerializeField] int comboBonusScore = 10;

    void Start()
    {
        Initialized();
    }

    public void Initialized()
    {
        currentScore = 0;
        txtScore.text = "0";
    }

    public void IncreaseScore(int p_JudgementState)
    {
        int t_increaseScore = increaseScore;

        // 콤보 보너스 점수 계산
        int t_currentCombo = FindObjectOfType<EffectManager>().GetCurrentCombo();
        int t_bonusComboScore = (int)(t_currentCombo / 10) * comboBonusScore; // 콤보 보너스 점수 = (현재 콤보 / 10) * 10
                                                                              // 콤보 구간 10~19 : 10점
                                                                              // 콤보 구간 20~29 : 20점

        // 가중치 계산
        t_increaseScore = (int)(t_increaseScore * weight[p_JudgementState]) + t_bonusComboScore;

        // 점수 반영
        currentScore += t_increaseScore;
        txtScore.text = string.Format("{0:#,##0}", currentScore); // 문자열 형식 : 재화, 단위, 소수점, 날짜 표현 형식으로 변환시켜줌  
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
}
