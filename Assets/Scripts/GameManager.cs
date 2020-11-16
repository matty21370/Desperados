using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private GameObject playerPrefab;

    /// <summary>
    /// This is the team selection screen that is activated when we join the game
    /// </summary>
    [SerializeField]
    private GameObject teamSelection;

    /// <summary>
    /// This is a list of teams the player is able to choose
    /// </summary>
    public enum Teams { Red, Blue };

    // Start is called before the first frame update
    void Start()
    {
        //teamSelection.SetActive(true);
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0); //Instantiate the player object over the network
        player.transform.name = "Player" + PhotonNetwork.CountOfPlayers + 1; //Set the players name in the scene
        player.GetComponent<Player>().InitiatePlayer(Vector3.zero); //Grab the Player.cs script off the instantiated object and call the InitiatePlayer method.
    }

    /// <summary>
    /// When we want to leave the server, we call this method.
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(); //Tell the PhotonNetwork that we want to leave the room
    }

    /// <summary>
    /// This is called when the PhotonNetwork takes us out of the room
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0); //We want to load the main menu scene
    }
}
