using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text yourScore, firstScore;

    public void SetScores(string whoWon, int yourScore)
    {
        firstScore.text = whoWon + " won the game!";
        this.yourScore.text = "You scored: " + yourScore;
    }
}
