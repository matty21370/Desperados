using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script created by: Matthew Burke
/// </summary>
public class GoToMenu : MonoBehaviour
{
    /// <summary>
    /// This method is called as soon as the player hits the play button
    /// </summary>
    private void Awake()
    {
        if(!PhotonNetwork.IsConnected) //If we are not connected to a PhotonNetwork
        {
            SceneManager.LoadScene("Lobby"); //Take the player back to the main menu scene
        }
    }
}
