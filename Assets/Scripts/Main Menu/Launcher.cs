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
    /// The below variables are references to all of the different pages in the main menu
    /// </summary>
    [SerializeField] private GameObject playPage;
    [SerializeField] private GameObject statsPage;
    [SerializeField] private GameObject shopPage;
    [SerializeField] private GameObject aboutPage;
    [SerializeField] private GameObject settingsPage;
    [SerializeField] private GameObject namePrompt;

    [SerializeField] private InputField nameField;

    /// <summary>
    /// We want to know if the player is connecting to a game or now
    /// </summary>
    bool isConnecting;

    /// <summary>
    /// This method is called as soon as the play button is pressed
    /// </summary>
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; 
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("Name"))
        {
            namePrompt.SetActive(true);
        }

        controlPanel.SetActive(true); 
        progressLabel.SetActive(false); 
    }

    /// <summary>
    /// When the user presses the play button, this method is called.
    /// </summary>
    public void Connect()
    {
        progressLabel.SetActive(true); 
        playPage.SetActive(false); 

        if (PhotonNetwork.IsConnected) 
        {
            PhotonNetwork.JoinRandomRoom(); 
        }
        else //If the player is not currently connected to the network
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings(); 
            PhotonNetwork.GameVersion = gameVersion; 
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
            progressLabel.GetComponent<Text>().text = "Finding session..."; 
            PhotonNetwork.JoinRandomRoom(); 
            isConnecting = false; 
        }
    }

    /// <summary>
    /// This method is called when the player cannot find any rooms to join
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom }); 
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
        controlPanel.SetActive(true); 
        progressLabel.SetActive(false); 

        isConnecting = false; //We are not connecting to anything

        Debug.Log("Disconnect caused by: " + cause); 
    }

    public void SubmitName()
    {
        if(nameField.text != "")
        {
            PlayerPrefs.SetString("Name", nameField.text);
            namePrompt.SetActive(false);
        }
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
