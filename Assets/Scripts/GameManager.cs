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

    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].position;

      //  SpawnObstacles();

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint, Quaternion.identity, 0);
    }

    /*private void SpawnObstacles()
    {
        GameObject[] objs;
        objs = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in objs)
        {
            obstacle.GetComponent<Obstacles>().spawn();
        }
    }*/

    private void Update()
    {
        if(PhotonNetwork.CountOfPlayers == 1)
        {
            neededPlayersText.gameObject.SetActive(true);
        }
        else
        {
            neededPlayersText.gameObject.SetActive(false);
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
}
