using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerController>().ResetFalling(); // GetComponentInParent : 부모 객체의 특정 컴포넌트를 가져옴
        }
    }
}
