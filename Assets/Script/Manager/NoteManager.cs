using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public static NoteManager instance;
    public int bpm = 0;
    double currentTime = 0d;

    [SerializeField] Transform tfNoteAppear = null; // 생성될 위치
    // [SerializeField] GameObject goNote = null; // 노트 프리팹
    [SerializeField] BoxCollider2D[] myCollider = null;
    [SerializeField] CanvasGroup notePollCanvas = null;

    TimingManager theTimingManager;
    GameManager theGameManager;
    EffectManager theEffectManager;

    GameObject timingRect;

    private void Awake()
    {
        if(instance == null) instance = this;

        theTimingManager = GetComponent<TimingManager>();
        theGameManager = FindObjectOfType<GameManager>();
        theEffectManager = FindObjectOfType<EffectManager>();

        timingRect = GameObject.Find("TimingRect");
    }

    private void OnEnable()
    {
        if (!theGameManager.isPauseCheck)
        {
            StartCoroutine("NoteColliderEnable");
        }
    }

    void Update()
    {
        if (GameManager.instance.isStartGame)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 60d / bpm) // 60s/BPM = 1Beat 시간
            {
                // 오브젝트 풀링 : 생성, 삭제는 성능에 불리 => 미리 필요한 만큼 생성 후 대여, 반납
                // GameObject t_note = Instantiate(goNote, tfNoteAppear.position, Quaternion.identity);
                // t_note.transform.SetParent(this.transform);
                GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
                t_note.transform.position = tfNoteAppear.position;
                t_note.SetActive(true);
                theTimingManager.boxNoteList.Add(t_note);
                currentTime -= 60d / bpm; // currentTime = 0 으로 초기화 할 경우 오차가 발생
            }
        }

    }
    // 화면 내의 노트를 제거하기 위하여 OnTriggerExit2D가 아닌 OnTriggerStay2D로 사용
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            /*Miss 이펙트 연출 : Miss 이펙트 연출과 노트 제거 위치를 다르게 설정하고자 Miss 이펙트는 MissEffect에서 적용
            if (collision.GetComponent<Note>().GetNoteFlag())
            {
                theTimingManager.MissRecore();
                theEffectManager.JudgementEffect(4);
            }
            */

            // 노트 제거
            theTimingManager.boxNoteList.Remove(collision.gameObject);
            //Destroy(collision.gameObject);
            ObjectPool.instance.noteQueue.Enqueue(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }

    public void NoteCollider(bool active)
    {
        myCollider[0].enabled = active;
        myCollider[1].enabled = !active;
    }

    IEnumerator NoteColliderEnable()
    {
        theEffectManager.Initialized();

        timingRect.SetActive(false);
        notePollCanvas.alpha = 0;
        NoteCollider(false);

        yield return new WaitForSeconds(1f);

        timingRect.SetActive(true);
        NoteCollider(true);
        notePollCanvas.alpha = 1;
    }

    public void RemoveNote()
    {
        GameManager.instance.isStartGame = false;

        for (int i = 0; i < theTimingManager.boxNoteList.Count; i++)
        {
            theTimingManager.boxNoteList[i].SetActive(false);
            ObjectPool.instance.noteQueue.Enqueue(theTimingManager.boxNoteList[i]);
        }

        theTimingManager.boxNoteList.Clear();
    }
}
