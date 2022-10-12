using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Song
{
    public string name;
    public string composer;
    public int bpm;
    public float time;
    public Sprite sprite;
}

public class StageMenu : MonoBehaviour
{
    [SerializeField] Song[] SongList = null;
    [SerializeField] TextMeshProUGUI txtSongName = null;
    [SerializeField] TextMeshProUGUI txtSongComposer = null;
    [SerializeField] Image imgDisk = null;

    [SerializeField] GameObject titleUI = null;

    int currentSong = 0;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public int GetCurrentSong()
    {
        return currentSong;
    }

    public float GetMusicTime()
    {
        return SongList[currentSong].time;
    }

    public void BtnNext()
    {
        AudioManager.instance.PlayeSFX("Touch");

        if (++currentSong > SongList.Length - 1)
        {
            currentSong = 0;
        }
        SettingSong();
    }

    public void BtnPrev()
    {
        AudioManager.instance.PlayeSFX("Touch");

        if (--currentSong < 0)
        {
            currentSong = SongList.Length - 1;
        }
        SettingSong();
    }

    public void SettingSong()
    {
        txtSongName.text = SongList[currentSong].name;
        txtSongComposer.text = SongList[currentSong].composer;
        imgDisk.sprite = SongList[currentSong].sprite;

        AudioManager.instance.PlayeBGM("BGM" + currentSong);
    }

    public void BtnBack()
    {
        AudioManager.instance.StopBGM();
        titleUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void BtnPlay()
    {
        int t_bpm = SongList[currentSong].bpm;

        GameManager.instance.GameStart(true, currentSong, t_bpm);
        this.gameObject.SetActive(false);
    }
}
