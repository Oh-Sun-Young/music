using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissEffect : MonoBehaviour
{
    TimingManager theTimingManager;
    EffectManager theEffectManager;

    private void Awake()
    {
        theTimingManager = FindObjectOfType<TimingManager>();
        theEffectManager = FindObjectOfType<EffectManager>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            if (collision.GetComponent<Note>().GetNoteFlag())
            {
                theTimingManager.MissRecore();
                theEffectManager.JudgementEffect(4);
            }
        }
    }
}
