using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    public Text youText, enemyText;

    private List<Player> otherPlayers = new List<Player>();
    Player yourPlayer;

    private void Update()
    {
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            CalculateScore();
            GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            Debug.LogWarning("Need 1 more player to start game");
            GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    public void CalculateScore()
    {
        otherPlayers.Clear();

        foreach(Player player in FindObjectsOfType<Player>())
        {
            if(player.photonView.IsMine)
            {
                yourPlayer = player;
                youText.text = "You: " + player.GetKills();
            }
            else
            {
                otherPlayers.Add(player);

                if(player.GetKills() >= 10)
                {
                    FindObjectOfType<GameOverScreen>().GetComponent<CanvasGroup>().alpha = 1;
                    FindObjectOfType<GameOverScreen>().SetScores("Your score: " + yourPlayer.GetKills(), "1. " + otherPlayers[0].GetName() + otherPlayers[0].GetKills(), "2. " + otherPlayers[1].GetName() + otherPlayers[1].GetKills(), "3. " + otherPlayers[2].GetName() + otherPlayers[2].GetKills());
                }
            }
        }

        otherPlayers.Sort(SortByKills);
        enemyText.text = otherPlayers[0].GetName() + ": " + otherPlayers[0].GetKills();
    }

    static int SortByKills(Player p1, Player p2)
    {
        return p1.GetKills().CompareTo(p2.GetKills());
    }
}
