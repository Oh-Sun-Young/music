using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/* 참고
 * - 발동된 트리거 취소 : https://is03.tistory.com/54
 */

public class EffectManager : MonoBehaviour
{
    [SerializeField] Animator noteHitAnimator = null;
    string hit = "Hit";

    [SerializeField] Animator judgementAnimator = null;
    [SerializeField] TextMeshProUGUI judgementText = null;
    [SerializeField] string[] judgementString = null;

    int currentCombo = 0;
    int maxCombo = 0;

    public void NoteHitEffect()
    {
        noteHitAnimator.SetTrigger(hit);
    }

    public void JudgementEffect(int p_num)
    {
        // 파라미터 값에 맞는 판정 이미지 스프라이트로 교체
        judgementText.text = (string)judgementString[p_num];
        judgementAnimator.SetTrigger(hit);

        // 콤보 증가
        if(p_num == 0)
        {
            IncreaseCombo();
        }
        else
        {
            ResetCombo();
        }
    }

    public void IncreaseCombo(int p_num = 1)
    {
        currentCombo += p_num;

        if(maxCombo < currentCombo)
        {
            maxCombo = currentCombo;
        }

        if(currentCombo > 2)
        {
            judgementText.text += " x " + currentCombo;
        }
    }

    public void ResetCombo()
    {
        currentCombo = 0;
    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }

    public int GetMaxCombo()
    {
        return maxCombo;
    }

    public void Initialized()
    {
        currentCombo = 0;
        maxCombo = 0;
        for (int i = 0; i < noteHitAnimator.gameObject.transform.childCount; i++)
        {
            noteHitAnimator.gameObject.transform.Find("CenterFrame_" + (i + 1)).localScale = new Vector3(1, 1, 1);
        }
        judgementText.text = null;
    }
}
