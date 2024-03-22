using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scores : MonoBehaviour
{
    public float PlayerScore = 0f;

    public void SetScore(float score)
    { PlayerScore += score; }

}
