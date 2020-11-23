using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : MonoBehaviourPunCallbacks
{
    public string playerName;
    public int level;
    public int kills;
    public int deaths;
    public float kdRatio;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(playerName);
            stream.SendNext(level);
            stream.SendNext(kills);
            stream.SendNext(deaths);
            stream.SendNext(kdRatio);
        }
        else if(stream.IsReading)
        {
            playerName = (string)stream.ReceiveNext();
            level = (int)stream.ReceiveNext();
            kills = (int)stream.ReceiveNext();
            deaths = (int)stream.ReceiveNext();
            kdRatio = (float)stream.ReceiveNext();
        }
    }

    private void Start()
    {
        level = PlayerPrefs.GetInt("Level");
    }
}
