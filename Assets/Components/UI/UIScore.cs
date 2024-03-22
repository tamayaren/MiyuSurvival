using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{
    [SerializeField] private Scores score;
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        text.text = $"SCORE: {score.PlayerScore}";
    }
}
