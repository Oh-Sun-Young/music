using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] GameObject stageUI = null;
    [SerializeField] AudioClip clip = null;
    [SerializeField] AudioSource bgmPlayer = null;

    private void OnEnable()
    {
        bgmPlayer.PlayOneShot(clip);
    }

    public void BtnPlay()
    {
        stageUI.SetActive(true);
        this.gameObject.SetActive(false);
        FindObjectOfType<AudioManager>().StopBGM();
        FindObjectOfType<StageMenu>().SettingSong();
    }
}
