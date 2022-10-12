using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePlate : MonoBehaviour
{
    PlayerController thePlayerController;

    private void Awake()
    {
        thePlayerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            thePlayerController.dragCnt = 2;
        }
    }
}
