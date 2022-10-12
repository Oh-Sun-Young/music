using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicPlate : MonoBehaviour
{
    public string[] text;
    public TextMeshProUGUI textMesh;

    private void Awake()
    {
        int num = Random.Range(0, text.Length);
        textMesh.text = text[num];
    }
}
