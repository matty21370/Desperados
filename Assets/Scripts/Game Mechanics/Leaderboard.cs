using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviourPunCallbacks
{
    public Transform content;
    public GameObject prefab;

    public Player player;

    private void Start()
    {
        
    }

    public void ShowLeaderBoard()
    {
        foreach(Transform t in content)
        {
            Destroy(t.gameObject);
        }
        GetComponent<CanvasGroup>().alpha = 1;
        foreach (Player player in FindObjectsOfType<Player>())
        {
            GameObject tmp = Instantiate(prefab, content);
            tmp.GetComponent<PlayerCard>().levelText.text = player.GetLevel().ToString();
            tmp.GetComponent<PlayerCard>().nameText.text = player.GetName().ToString();
            tmp.GetComponent<PlayerCard>().killsText.text = player.GetKills().ToString();
            tmp.GetComponent<PlayerCard>().deathsText.text = player.GetDeaths().ToString();


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
