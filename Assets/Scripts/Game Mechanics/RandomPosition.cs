using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RandomPosition : MonoBehaviourPunCallbacks
{
    public Vector3[] positions;
    void Start()
    {
        if (FindObjectsOfType<Player>().Length == 1)
        {
            photonView.RPC(nameof(SetPosition), RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void SetPosition()
    {
        int randomNumber = Random.Range(0, positions.Length);
        transform.position = positions[randomNumber];
    }
}
