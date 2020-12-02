using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    public Text youText, enemyText;

    private List<Player> otherPlayers = new List<Player>();

    private void Update()
    {
        CalculateScore();
    }

    public void CalculateScore()
    {
        otherPlayers.Clear();

        foreach(Player player in FindObjectsOfType<Player>())
        {
            if(player.photonView.IsMine)
            {
                youText.text = "You: " + player.GetKills();
            }
            else
            {
                otherPlayers.Add(player);

                if(player.GetKills() >= 25)
                {
                    //End game logic goes here
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
