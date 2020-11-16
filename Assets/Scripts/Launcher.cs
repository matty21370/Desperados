using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

/// <summary>
/// Script created by: Matthew Burke
/// </summary>
public class Launcher : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// This is a reference to the current game version.
    /// Can be used to avoid connecting to players with conflicing versions
    /// </summary>
    string gameVersion = "1";

    /// <summary>
    /// This is the maxiumum amount of players we want in a server
    /// </summary>
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    /// <summary>
    /// This is a reference to the control panel object in the scene.
    /// This object contains all of the buttons on the main menu
    /// </summary>
    [SerializeField]
    private GameObject controlPanel;

    /// <summary>
    /// This is a reference to the progress text,
    /// Will be used to let the player know their connection progress
    /// </summary>
    [SerializeField]
    private GameObject progressLabel;

    /// <summary>
    /// A reference to the input field responsible for entering the players name
    /// </summary>
    [SerializeField]
    private InputField inputField;

    /// <summary>
    /// The below variables are references to all of the different pages in the main menu
    /// </summary>
    [SerializeField] private GameObject playPage;
    [SerializeField] private GameObject statsPage;
    [SerializeField] private GameObject shopPage;
    [SerializeField] private GameObject aboutPage;
    [SerializeField] private GameObject settingsPage;

    /// <summary>
    /// We want to know if the player is connecting to a game or now
    /// </summary>
    bool isConnecting;

    /// <summary>
    /// This method is called as soon as the play button is pressed
    /// </summary>
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //We want the network to automatically sync the scenes for all players when in game
    }

    // Start is called before the first frame update
    void Start()
    {
        controlPanel.SetActive(true); //We want to activate the control panel so the player can interact with the menu
        progressLabel.SetActive(false); //We want to hide the progress label because the player is not currently connecting to a game
    }

    /// <summary>
    /// When the user presses the play button, this method is called.
    /// </summary>
    public void Connect()
    {
        if (inputField.text != null) //If the player has entered something into the input field
        {
            PhotonNetwork.NickName = inputField.text; //We set the players nickname to the value of the input field

            progressLabel.SetActive(true); //We want to show the players connection status, so we display the text responsible for showing it.
            playPage.SetActive(false); //We want to hide the page the player is currently in

            if (PhotonNetwork.IsConnected) //If the player is connected to the network
            {
                PhotonNetwork.JoinRandomRoom(); //Join a random room
            }
            else //If the player is not currently connected to the network
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings(); //Connect to the network
                PhotonNetwork.GameVersion = gameVersion; //Tell the network our current game version
            }
        }
    }

    /// <summary>
    /// This method is called when the player is connected to the master server
    /// </summary>
    public override void OnConnectedToMaster()
    {
        if (isConnecting) //If the player has pressed the join button
        {
            Debug.Log("Connected to master server.");
            progressLabel.GetComponent<Text>().text = "Finding session..."; //Update the progress text to let the player know the network is searching for a session
            PhotonNetwork.JoinRandomRoom(); //Join a random room
            isConnecting = false; //We are no longer connecting
        }
    }

    /// <summary>
    /// This method is called when the player cannot find any rooms to join
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom }); //We want the player to create their own room
    }

    /// <summary>
    /// When the player has joined a room
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully joined room");
        PhotonNetwork.LoadLevel("SampleScene"); //Load the main game scene
    }

    /// <summary>
    /// When the player has been disconnected from the master server
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        controlPanel.SetActive(true); //We want to activate the control panel
        progressLabel.SetActive(false); //We want to deactivate the progress label

        isConnecting = false; //We are not connecting to anything

        Debug.Log("Disconnect caused by: " + cause); 
    }

    /// <summary>
    /// The below methods are called depending on the button pressed in the main menu
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenPlayPage()
    {
        controlPanel.SetActive(false);
        playPage.SetActive(true);
    }

    public void ClosePlayPage()
    {
        controlPanel.SetActive(true);
        playPage.SetActive(false);
    }

    public void OpenStatsPage()
    {
        controlPanel.SetActive(false);
        statsPage.SetActive(true);
    }

    public void CloseStatsPage()
    {
        controlPanel.SetActive(true);
        statsPage.SetActive(false);
    }

    public void OpenAboutPage()
    {
        controlPanel.SetActive(false);
        aboutPage.SetActive(true);
    }

    public void CloseAboutPage()
    {
        controlPanel.SetActive(true);
        aboutPage.SetActive(false);
    }

    public void OpenShopPage()
    {
        controlPanel.SetActive(false);
        shopPage.SetActive(true);
    }

    public void CloseShopPage()
    {
        controlPanel.SetActive(true);
        shopPage.SetActive(false);
    }

    public void OpenSettingsPage()
    {
        controlPanel.SetActive(false);
        settingsPage.SetActive(true);
    }

    public void CloseSettingsPage()
    {
        controlPanel.SetActive(true);
        settingsPage.SetActive(false);
    }
}
