using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFrame : MonoBehaviour
{
    public float timer;

    bool musicStart = false;
    public string bgmName = "";


    private void Update()
    {
        timer += Time.deltaTime;
    }

    public void Initialized()
    {
        timer = 0;
    }

    public void ResetMusic()
    {
        musicStart = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (timer < 1f)
        {
            AudioManager.instance.StopBGM();
            musicStart = false;
        }
        else
        {
            if (!musicStart && collision.CompareTag("Note"))
            {
                AudioManager.instance.PlayeBGM(bgmName);
                musicStart = true;
            }
        }
    }
}
