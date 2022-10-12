using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notice : MonoBehaviour
{
    bool check;
    TextMeshProUGUI notice;

    PlayerController thePlayerController;

    private void Awake()
    {
        thePlayerController = FindObjectOfType<PlayerController>();
        notice = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        check = false;
        notice.alpha = 1;
    }

    private void Update()
    {
        if(!check && thePlayerController.joy.Horizontal != 0 || thePlayerController.joy.Vertical != 0)
        {
            check = true;
            StartCoroutine("NoticeFadeOut");
        }
    }

    IEnumerator NoticeFadeOut()
    {
        while (notice.alpha > 0)
        {
            notice.alpha = Mathf.Lerp(notice.alpha, 0, 0.25f);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
