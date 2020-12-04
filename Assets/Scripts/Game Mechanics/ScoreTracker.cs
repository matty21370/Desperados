using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    public Text youText, enemyText;

    private void Update()
    {
        CalculateScore();
    }

    public void CalculateScore()
    {
        foreach(Player player in FindObjectsOfType<Player>())
        {
            if(player.photonView.IsMine)
            {
                youText.text = "Your score: " + player.GetKills();
            } 
            else
            {
                if(player.GetKills() >= 10)
                {
                    //End game
                }
            }
        }
    }
}
