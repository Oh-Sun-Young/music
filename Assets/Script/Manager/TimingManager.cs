using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>(); // 생성된 노트를 담는 List, 판정 범위에 있는 지 모든 노트를 비교

    int[] judgementRecord = new int[5]; // 판정 갯수 기록

    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timmingRect = null; // 판정 범위 (Perfect, Cool, Good, Bad)
    Vector2[] timingBoxs = null; // 판정 범위의 최소값(x), 최대값(y)

    EffectManager theEffectManager;
    ScoreManager theScoreManager;
    StageManager theStageManager;
    PlayerController thePlayer;
    AudioManager theAudioManager;

    void Start()
    {
        theEffectManager = FindObjectOfType<EffectManager>();
        theScoreManager = FindObjectOfType<ScoreManager>();
        theStageManager = FindObjectOfType<StageManager>();
        thePlayer = FindObjectOfType<PlayerController>();
        theAudioManager = FindObjectOfType<AudioManager>();

        // 타이밍 박스 설정
        timingBoxs = new Vector2[timmingRect.Length];
        for(int i = 0; i< timmingRect.Length; i++)
        {
            // 각각의 판정 범위
            // 최소값 = 중심 - (이미지의 너비 / 2)
            // 최대값 = 중심 + (이미지의 너비 / 2)
            timingBoxs[i].Set(Center.localPosition.x - timmingRect[i].rect.width / 2, Center.localPosition.x + timmingRect[i].rect.width / 2);
        }
    }
    public bool CheckTiming()
    {
        for(int i = 0; i<boxNoteList.Count; i++)
        {
            // 판정범위 최소값 <= 노트의 x값 <= 판점범위 최대값
            float t_notePosX = boxNoteList[i].transform.localPosition.x;
            // 판정 범위만큼 반복 => 어느 판정 범위에 있는 지 확인
            for(int x = 0; x < timingBoxs.Length; x++)
            {
                if(timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y)
                {
                    // 노트 제거
                    // Destroy(boxNoteList[i]); 가운데 지점을 지나가지 않을 경우 음악이 안들리는 이슈 발생 => 보이지 않게 처리 
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);

                    // 이펙트 연출
                    if (x < timingBoxs.Length - 1)
                    {
                        theEffectManager.NoteHitEffect();
                    }

                    if (CheckCanNextPlate())
                    {
                        theScoreManager.IncreaseScore(x); // 점수 증가
                        theStageManager.ShowNextPlate(); // 발판 활성화
                        theEffectManager.JudgementEffect(x); // 판정 연출
                        judgementRecord[x]++; // 판정 기록
                    }
                    else
                    {
                        theEffectManager.JudgementEffect(5);
                    }


                    theAudioManager.PlayeSFX("Clap");

                    return true;
                }
            }
        }
        theEffectManager.JudgementEffect(timingBoxs.Length);
        MissRecore(); // 판정 기록
        return false;
    }

    bool CheckCanNextPlate()
    {
        if(Physics.Raycast(thePlayer.destPos, Vector3.down, out RaycastHit t_hitInfo, 1.1f)) // Physics.Raycast(광선 위치, 방향, 충돌 정보, 길이) : 가상의 광선을 쏴서 맞은 대상의 정보를 가져오는 함수
        {
            if (t_hitInfo.transform.CompareTag("BasicPlate"))
            {
                Plate t_plate = t_hitInfo.transform.GetComponent<Plate>();
                if (t_plate.flag)
                {
                    t_plate.flag = false;
                    return true;
                }
            }
        }
        return false;
    }

    public int[] GetJudgementRecore()
    {
        return judgementRecord;
    }
    public void MissRecore()
    {
        judgementRecord[4]++;
    }


    public void Initialized()
    {
        for(int i = 0; i < judgementRecord.Length; i++)
        {
            judgementRecord[i] = 0;
        }
    }
}
