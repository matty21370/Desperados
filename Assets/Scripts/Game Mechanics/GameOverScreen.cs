using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text yourScore, firstScore, secondScore, thirdScore;

    public void SetScores(string your, string first, string second, string third)
    {
        yourScore.text = your;
        firstScore.text = first;
        secondScore.text = second;
        thirdScore.text = third;
    }
}
