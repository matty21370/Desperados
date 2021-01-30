using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    public Text youText, scoreToWinText;

    public int scoreToWin;

    private Player yourPlayer;
    private GameOverScreen gameOverScreen;

    private void Start()
    {
        scoreToWinText.text = "Score to win: " + scoreToWin;
        gameOverScreen = FindObjectOfType<GameOverScreen>();
    }

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
                yourPlayer = player;
            }

            if (player.GetKills() >= scoreToWin)
            {
                gameOverScreen.GetComponent<CanvasGroup>().alpha = 1;
                gameOverScreen.SetScores(player.GetName(), yourPlayer.GetKills());
                yourPlayer.canMove = false;
                yourPlayer.canShoot = false;
                //Invoke("RestartGame", 5f);
                RestartGame();
            }
        }
    }

    public void RestartGame()
    {
        if (yourPlayer.isDead)
        {
            yourPlayer.Respawn();
        }
        yourPlayer.canMove = true;
        yourPlayer.canShoot = true;
        yourPlayer.photonView.RPC("ResetPlayer", RpcTarget.All);
        //gameOverScreen.GetComponent<CanvasGroup>().alpha = 0;
    }
}
