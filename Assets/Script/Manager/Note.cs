using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float noteSpeed = 400;

    CanvasGroup noteImage;

    [System.Obsolete]
    private void OnEnable()
    {
        if(noteImage == null)
        {
            noteImage = GetComponent<CanvasGroup>();
        }

        noteImage.alpha = 1;

    }

    private void Update()
    {
        transform.localPosition += Vector3.right * noteSpeed * Time.deltaTime;
    }

    public void HideNote()
    {
        noteImage.alpha = 0;
    }

    public bool GetNoteFlag()
    {
        return noteImage.alpha == 1 ? true : false;
    }
}
