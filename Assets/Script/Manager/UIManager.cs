using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Play")]
    [SerializeField] GameObject note = null;
    [SerializeField] GameObject effect = null;
    [SerializeField] GameObject score = null;
    [SerializeField] GameObject button = null;

    [Header("Temp UI")]
    [SerializeField] GameObject noteTemp = null;
    [SerializeField] GameObject scoreTemp = null;
    [SerializeField] GameObject buttonTemp = null;
    [SerializeField] TextMeshProUGUI txtScoreTemp = null;

    ScoreManager theScoreManager;

    private void Awake()
    {
        theScoreManager = FindObjectOfType<ScoreManager>();
    }

    public void TempEnable(bool active)
    {
        note.SetActive(!active);
        effect.SetActive(!active);
        score.SetActive(!active);
        button.SetActive(!active);

        noteTemp.SetActive(active);
        scoreTemp.SetActive(active);
        buttonTemp.SetActive(active);

        int t_currentScore = theScoreManager.GetCurrentScore();

        txtScoreTemp.text = string.Format("{0:#,##0}", t_currentScore);
    }
}
