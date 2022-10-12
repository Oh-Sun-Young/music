using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    AudioSource theAudio;
    NoteManager theNoteManager;

    Result theResult;

    private void Start()
    {
        theAudio = GetComponent<AudioSource>();
        theNoteManager = FindObjectOfType<NoteManager>();
        theResult = FindObjectOfType<Result>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            theAudio.Play();
            PlayerController.s_canPressKey = false;
            theNoteManager.RemoveNote();
            theResult.ShowResult();
        }
    }
}
