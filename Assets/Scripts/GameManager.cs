using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script created by: Matthew Burke
/// </summary>
public class GameManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// This is the killstreak the players need to get an effect
    /// </summary>
    public static int killstreakForEffect = 5;

    /// <summary>
    /// This is a list of all the spawnpoints in the scene
    /// </summary>
    public List<Transform> spawnPoints = new List<Transform>();

    /// <summary>
    /// This is the prefab we want to instantiate when a player joins the game.
    /// </summary>
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Text neededPlayersText;

    public enum GameStates { LOBBY, GAME }

    void Start()
    {
        Spawn();

    }

    public void Spawn()
    {
        Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].position;

        PhotonNetwork.Instantiate("Player", spawnPoint, Quaternion.identity, 0);
    }

    private void Update()
    {
        
    }

    public void RespawnPlayer()
    {
        foreach(Player player in FindObjectsOfType<Player>())
        {
            if(player.photonView.IsMine)
            {
                player.RespawnButton();
            }
        }
    }

    /// <summary>
    /// When we want to leave the server, we call this method.
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// This is called when the PhotonNetwork takes us out of the room
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }
}
