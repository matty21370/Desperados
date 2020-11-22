using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviourPunCallbacks
{
    public Transform content;
    public GameObject prefab;

    private GameManager manager;

    public Player player;

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    public void ShowLeaderBoard()
    {
        foreach(Transform t in content)
        {
            Destroy(t.gameObject);
        }
        GetComponent<CanvasGroup>().alpha = 1;
        foreach(Player player in player.allPlayers)
        {
            GameObject tmp = Instantiate(prefab, content);
            tmp.GetComponentInChildren<Text>().text = player.GetName() + "          " + player.GetKills();
        }
    }

    public void HideLeaderboard()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        foreach(Transform t in content)
        {
            Destroy(t.gameObject);
        }
    }
}
