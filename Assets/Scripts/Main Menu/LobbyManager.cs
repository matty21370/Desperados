using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject lobbyPlayer;
    public Transform content;
    public TMP_Text hostText;
    public Button settingsButton;
    public Button[] buttons;
    public int scoreToWin = 10;
    public TMP_Text scoreToWinText;

    float timer = 1.0f;

    bool started = false;

    public bool isHost = false;

    private void Start()
    {
        UpdateLobby();

        if(Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            settingsButton.gameObject.SetActive(true);
        }
        else
        {
            DisableButtons();
        }

        scoreToWinText.text = scoreToWin.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            UpdateLobby();
            timer = 1.0f;
        }

        if (!started)
        {
            
        }
    }

    void UpdateLobby()
    {
        int numToStart = FindObjectsOfType<Player>().Length / 2 + 1;
        int currentNum = 0;

        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
            currentNum = 0;
        }

        foreach (Player player in FindObjectsOfType<Player>())
        {
            GameObject g = Instantiate(lobbyPlayer, content);
            g.GetComponent<LobbyPlayer>().levelText.text = player.GetLevel().ToString();
            g.GetComponent<LobbyPlayer>().nameText.text = player.GetName();

            if (player.isReady)
            {
                g.GetComponent<LobbyPlayer>().ready.color = Color.green;
            }
            else
            {
                g.GetComponent<LobbyPlayer>().ready.color = Color.red;
            }

            if(player.isReady)
            {
                currentNum++;
            }

            if(player.masterClient)
            {
                hostText.text = player.GetName();
            }
        }

        if(currentNum >= numToStart && FindObjectsOfType<Player>().Length >= 0)
        {
            foreach(Player player in FindObjectsOfType<Player>())
            {
                //player.photonView.RPC("SetToGame", Photon.Pun.RpcTarget.AllBuffered);
                player.SetToGame();
                player.isReady = true;
                FindObjectOfType<ScoreTracker>().scoreToWin = scoreToWin;
                DisableButtons();
                //player.lobbyScreen.GetComponent<CanvasGroup>().alpha = 0;
            }

            started = true;

            FindObjectOfType<AudioManager>().Stop("Music");
        }
    }

    public void DisableButtons()
    {
        foreach (Button b in buttons)
        {
            b.gameObject.SetActive(false);
        }

        settingsButton.gameObject.SetActive(false);
    }

    public void SettingsButtonPressed()
    {
        foreach(Button b in buttons)
        {
            b.gameObject.SetActive(true);
        }
    }

    public void UpdateScorePlus()
    {
        if (scoreToWin <= 50)
        {
            photonView.RPC("RPCUpdateScores", RpcTarget.AllBuffered, 1);
        }
    }

    public void UpdateScoreMinus()
    {
        if (scoreToWin > 0)
        {
            photonView.RPC("RPCUpdateScores", RpcTarget.AllBuffered, -1);
        }
    }

    [PunRPC]
    private void RPCUpdateScores(int amt)
    {
        scoreToWin += amt;
        scoreToWinText.text = scoreToWin.ToString();
    }
}
