using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// Script created by: Matthew Burke, Andrew Viney
/// </summary>
public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
   
    public List<Player> allPlayers = new List<Player>();

    /// <summary>
    /// This is a reference to the name of the player
    /// </summary>
    private string playerName;

    /// <summary>
    /// This is the base speed of the player, can be upgraded later
    /// </summary>
    [SerializeField] private float baseSpeed = 5f;

    /// <summary>
    /// This is the speed of the player when boosting, can be upgraded later
    /// </summary>
    [SerializeField] private float boostSpeed = 8f;

    /// <summary>
    /// This is the maximum health the player can have, can be upgraded layer
    /// </summary>
    [SerializeField] private float maxHealth = 10;
   
    /// <summary>
    /// This is the speed the player is currently moving at
    /// </summary>
    private float movementSpeed = 5f;

    /// <summary>
    /// This is the health the player currently has
    /// </summary>
    private float playerHealth = 10;

    /// <summary>
    /// The below variables reflect the players stats in game
    /// </summary>
    private float exp;
    private int level;
    private int killCount;
    private int deaths;
    private int killStreak;
    private int killsThisGame;
    private bool canShoot = true;

    private int team;

    private bool leaderboardOpen;

    /// <summary>
    /// This is a reference to the camera object so it can be modified later
    /// </summary>
    private Camera camera;

    /// <summary>
    /// This is a list of all the guns currently attached to the player
    /// </summary>
    [SerializeField] private List<Gun> guns = new List<Gun>();

    /// <summary>
    /// This is the amount of mines the player has placed
    /// </summary>
    private int mineNumber = 1;

    /// <summary>
    /// The below variables are references to all the UI components for the player
    /// </summary>
    private Text levelText;
    private Text healthText;
    private Text joinText;
    private Text pingText;
    private UnityEngine.UI.Slider healthSlider;
    private UnityEngine.UI.Slider expSlider;

    /// <summary>
    /// A boolean showing if we have displayed the killstreak text or not
    /// </summary>
    bool displayedKillstreakText = false;

    /// <summary>
    /// The below variables are references to all the particle effects we want in the game
    /// </summary>
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private GameObject levelUpEffect;
    [SerializeField] private GameObject hitParticle;
    [SerializeField] private GameObject explosiveParticle;
    [SerializeField] private GameObject boostTrail;

   

    [SerializeField] private GameObject shop;
    Leaderboard leaderboard;

    public int GetTeam()
    {
        return team;
    }

    public string GetName()
    {
        return playerName;
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetKills()
    {
        return killCount;
    }

    public int GetDeaths()
    {
        return deaths;
    }

    /// <summary>
    /// This is the method for syncing all of the players variables to the other clients on the server.
    /// This is called multiple times per second to ensure proper synchronisation between players.
    /// </summary>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //If our client is controlling this object, then we send our variables to the stream for other clients
        {
            stream.SendNext(playerHealth);
            stream.SendNext(canShoot);
            stream.SendNext(level);
            stream.SendNext(killCount);
            stream.SendNext(killsThisGame);
            stream.SendNext(team);
        }
        else if (stream.IsReading) //If our client is not controlling this object, then we recieve the variables from the client who is and apply them.
        {
            playerHealth = (float)stream.ReceiveNext();
            canShoot = (bool)stream.ReceiveNext();
            level = (int)stream.ReceiveNext();
            killCount = (int)stream.ReceiveNext();
            killsThisGame = (int)stream.ReceiveNext();
            team = (int)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// When we join the game, we want to grab the nickname from the PhotonNetwork and apply it to the object.
    /// The PhotonNetwork nickname is set in the main menu.
    /// </summary>
    [PunRPC]
    private void SyncName()
    {
        playerName = PhotonNetwork.NickName;
    }

    /// <summary>
    /// This method is called as soon as the player enters the main scene. 
    /// It is responsible for setting up all of the required variables that cannot be set in the editor.
    /// </summary>
    void Start()
    {
      

        leaderboard = FindObjectOfType<Leaderboard>();
        leaderboard.player = this;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked; //We want to lock the cursor to the centre of the screen to prevent accidentally clicking off the game.
        UnityEngine.Cursor.visible = false; //Hide the mouse cursor

        photonView.RPC("SyncName", RpcTarget.AllBuffered); //Immediately send an RPC to all current and future clients to sync our nickname.
        photonView.RPC("updatePlayerList", RpcTarget.AllBuffered);

        //Due to prefab limitations, we are not able to directly set these four variables. Instead we have to find the objects by name when we enter the scene.
        healthText = GameObject.Find("Health Text").GetComponent<Text>(); 
        healthSlider = GameObject.Find("Health Background").GetComponent<UnityEngine.UI.Slider>();
        levelText = GameObject.Find("Level Text").GetComponent<Text>();
        expSlider = FindObjectOfType<UnityEngine.UI.Slider>();

        playerHealth = maxHealth; //The player will start with their maximum health, 10 by default and will have the opportunity to upgrade.
        UpdateHealthBar(); //We want to sync the healthbar to reflect the health change we just applied.

        level = PlayerPrefs.GetInt("Level"); //Automatically grab the players level from the registry, this allows the user to keep their progress when they exit the game.
        levelText.text = "Level: " + level; //Adjust the level text to reflect the above change.

        exp = PlayerPrefs.GetFloat("Exp"); //Similarly to the level, we want to grab the players level from their registry.
        expSlider.value = exp / 100; //Rather than setting the text like we did for the level, we adjust the slider to reflect the players experience.

        pingText = GameObject.Find("Ping Text").GetComponent<Text>(); //We want to grab the ping text from the scene

        camera = GetComponentInChildren<Camera>(); //We want to be able to adjust some camera settings depending on the players movement, so we need to grab the camera object 

        team = UnityEngine.Random.Range(1, 2);

        if(!photonView.IsMine) //If we are not controlling this object 
        {
            Destroy(GetComponentInChildren<Camera>().gameObject); //We destroy the camera to avoid confusion (if we didn't do this then we could have 40 different cameras in the scene)
        }
    }

    [PunRPC]
    private void UpdateColour()
    {
        
    }

    /// <summary>
    /// This method is called by the server to place the player in the scene. 
    /// </summary>
    /// <param name="position"></param> This is the position where the player will be placed in the scene.
    public void InitiatePlayer(Vector3 position)
    {
        transform.position = position; //Set our position to the Vector3 passed in to the method
    }

    /// <summary>
    /// This method is called once per frame, framerate dependent.
    /// Movement related commands are not recommended to go in here.
    /// </summary> //
    void Update()
    {
       
        //If we are not controlling this object (another player is)
        if (!photonView.IsMine)
        {
            return; //Exit out of this method to avoid multiple clients controlling the same object
        }

        if (Input.GetMouseButtonDown(0) && photonView.IsMine && canShoot) //If we click the mouse button and we are ready to shoot
        {
            photonView.RPC("Shoot", RpcTarget.All); //Send an RPC to all clients in the server letting them know we are going to shoot. This then executes the method for every player so everything is synchronised.
        }

        if (Input.GetKeyDown(KeyCode.G)) //If we press the G button on the keyboard 
        {
            hitDetected(1); //Take 1 damage (this is for testing and will be removed)
        }
        
        if(Input.GetKeyDown(KeyCode.X)) //If we press the X button on the keyboard
        {
            AddExperience(10); //Add 10 experience (similarly to the method above, this will be removed)
        }

        if (Input.GetKeyDown(KeyCode.T)) //If the user presses the T key on the keyboard
        {
            photonView.RPC("DropMine", RpcTarget.All); //Send an RPC to all clients to execute the DropMine function on this particular player. Similarly to the shoot function, this allows synchronisation.

        }
        /*    if (Input.GetKeyDown(KeyCode.I)) //If the user presses the T key on the keyboard
            {
           // GameObject.Find("shop").GetComponent<Shop>().setEnabled();
          //shop.setEnabled();
            }*/


                if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(!leaderboardOpen)
            {
                leaderboard.ShowLeaderBoard();
                leaderboardOpen = true;
                photonView.RPC("updatePlayerList", RpcTarget.AllBuffered);
            }
            else
            {
                leaderboard.HideLeaderboard();
                leaderboardOpen = false;
            }
        }

      //  pingText.text = "Latency: " + PhotonNetwork.GetPing(); //We set the text component of the pingText variable to the current ping of the PhotonNetwork
    }

    /// <summary>
    /// This Update method is framerate independent. Movement commands should go in here
    /// </summary>
    private void FixedUpdate()
    {
        if (!photonView.IsMine) //Similar to the Update method above, exit out of this method if we are not controlling this player object.
        {
            return;
        }

        transform.forward = -GetComponentInChildren<Camera>().transform.forward; //Face the player in the direction the camera is faced. This allows the player to rotate with the mouse.

        HandleInput(); //To avoid clutter, we created a seperate method to handle input. It is still framerate independent due to it being called in this FixedUpdate method.
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        photonView.RPC("updatePlayerList", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// This method handles the player movement based on keyboard input.
    /// </summary>
    void HandleInput()
    {
        if (Input.GetKey(KeyCode.W)) //When the user presses the W key on the keyboard
        {
            transform.position += -transform.forward * movementSpeed * Time.deltaTime; //We want to move the player forward. Due to a mistake in the camera script, backwards is forwards which is why we use -transform.forward.
            camera.fieldOfView += Time.deltaTime * 6; //Adjust the cameras field of view when moving, this creates a cool effect and makes the camera feel more responsive to what the player is doing.

            if (Input.GetKey(KeyCode.Space)) //When the user presses Space
            {
                movementSpeed = 11f; //Increase the movement speed, this needs to be set to a variable in the future to allow for upgrades
                camera.GetComponent<CameraMovement>().SetMaxFOV(95f); //Increase the maximum field of view in the camera in response to the player moving faster, looks cool.
            }
            else //When the player isn't pressing space
            {
                movementSpeed = 5f; //Set the players movement speed to 5, the default movement speed.
                camera.GetComponent<CameraMovement>().SetMaxFOV(90f); //Set the cameras maximum field of view to 90
            }
        }
        else
        {
           // camera.fieldOfView -= Time.deltaTime * 6; //When the player lets go of the W and Space key, slowly decrease the field of view to reflect this.
        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) //When the user presses the A key and is not pressing the D key (to avoid bugs with movement)
        {
            transform.position += transform.right * movementSpeed * Time.deltaTime; //Move the player to the right 
            float z = Input.GetAxis("Horizontal") * 15.0f; 
            Vector3 euler = transform.localEulerAngles;
            euler.z = Mathf.Lerp(euler.z, z, 25.0f * Time.deltaTime);
            transform.localEulerAngles = euler;
        }

        if (Input.GetKey(KeyCode.S)) //When the user presses the S key
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime; //Move the player backwards 
        }

        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * movementSpeed * Time.deltaTime;
            float z = Input.GetAxis("Horizontal") * 15.0f;
            Vector3 euler = transform.localEulerAngles;
            euler.z = Mathf.Lerp(euler.z, z, 25.0f * Time.deltaTime);
            transform.localEulerAngles = euler;
        }

        if(Input.GetKey(KeyCode.LeftControl)) //When the user presses the control key
        {
           transform.position += -transform.up * movementSpeed * Time.deltaTime; //Move the player downwards
        }

        if (Input.GetKey(KeyCode.LeftShift)) //When the user presses the shift key
        {
            transform.position += transform.up * movementSpeed * Time.deltaTime; //Move the player upwards
        }
    }

    private void OnDestroy()
    {
        photonView.RPC("updatePlayerList", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// This RPC function allows the player to shoot.
    /// </summary>
    [PunRPC]
    void Shoot()
    {
        foreach (Gun g in guns) //We want access to every gun attached to the player, so we use a foreach loop
        {
            GameObject bullet = Instantiate(g.getBullet(), g.getGunPosition().position, transform.rotation); //We want to instantiate the bullet and set its position to the position of the current gun in the loop
            bullet.GetComponent<Bullet>().InitializeBullet(gameObject, -transform.forward); //Get the bullet script and call the InitializeBullet function, this allows us to set some variables in the bullet to get it moving properly.
            Destroy(bullet, 5f); //After 5 seconds, destroy the bullet.
        }
    }

    /// <summary>
    /// This RPC function allows the player to drop mines.
    /// </summary>
    [PunRPC]
    private void DropMine()
    {
        Gun g = guns[0]; //Get the first gun from the guns list
        GameObject mine = Instantiate(g.getMine(), g.getGunPosition().position, transform.rotation); //Instantiate the mine and set its position to the position of the gun we selected from the list
        mine.GetComponent<Mine>().InitializeMine(gameObject); //Grab the Mine.cs script from the instantiated object and call the InitializeMine method.
        mineNumber++; //Increase the number of mines we have placed
        Destroy(mine, 5f); //Destroy the mine after 5 seconds
    }

    /// <summary>
    /// When the mine is destroyed, this method is called.
    /// </summary>
    public void lostMine()
    {
        mineNumber--; //Decrease the number of mines the player has active.
    }

    /// <summary>
    /// If we want to apply damage to a player. We call this method.
    /// </summary>
    /// <param name="damage"></param> The amount of damage we want to apply to the player.
    public void hitDetected(int damage)
    {
        playerHealth -= damage; //Decrease our health depending on the damage parameter passed in
        healthSlider.value = playerHealth; //Change the health slider to reflect the decreased health
        UpdateHealthBar(); //Call the update health bar method
        if (playerHealth <= 0) //If the players health hits 0
        {
            Despawn(); //Despawn the player to start the death process
        }
    }

    /// <summary>
    /// We call this method after we modify the players health. This allows the healthbar to sync up.
    /// </summary>
    public void UpdateHealthBar()
    {
        if (photonView.IsMine) //We only execute the code below if we are controlling the object, this prevents other players from changing our healthbar
        {
            healthSlider.value = playerHealth / 10; //The maximum slider value is 1. So we need to divide the players health by 10 to allow the slider to accurately represent our health
            healthText.text = playerHealth + "/" + maxHealth; //Update the health text located inside of the healthbar
        }
    }

    /// <summary>
    /// This method begins the death process.
    /// It is responsible for deactivating all of the necessary components to hide the player and prevent other players from interacting with it.
    /// </summary>
    public void Despawn()
    {
        GameObject p = Instantiate(explosionParticle, transform.position, Quaternion.identity); //We want to create an explosion effect at the position where the player has been despawned.
        transform.position = Vector3.zero; //Set the players position to Vector3.zero (0, 0, 0) (The centre of the scene)
        Destroy(p, 5f); //We want to destroy the exposion particle effect after 5 seconds to avoid clutter in the scene
        canShoot = false; //The player should not be able to shoot when despawned, so we update the canShoot boolean to reflect that

        foreach(Transform child in transform) //We want to get all of the child objects of this object, so we use a foreach loop to iterate through them all
        {
            if(child.GetComponent<MeshRenderer>()) //If the child object has a MeshRenderer component attached to it
            {
                child.GetComponent<MeshRenderer>().enabled = false; //We want the player to be invisible so we simply turn the renderer off.
            }
        }

        GetComponent<BoxCollider>().enabled = false; //Disable the box collider component to avoid other players from interacting with the despawned object
        mineNumber = 0; //Reset the amount of mines we have active
        onDeath(); //Call the onDeath method, the next stage of the death process
    }

    /// <summary>
    /// This is the second step of the death process.
    /// It is responsible for updating the necessary variables to reflect the players death.
    /// </summary>
    private void onDeath()
    {
        StartCoroutine("respawn"); //We want to start the respawn corouting to get the player back into the game after dying.

        if (photonView.IsMine) //Only execute the code below if we are controlling this object
        {
            PlayerPrefs.SetInt("Deaths", PlayerPrefs.GetInt("Deaths") + 1); //Update the death statistic, this saves to the players registry so it stays with them forever. This could be used for calculating the K/D ratio.
            deaths += 1; //Increment the amount of deaths the player has
            killStreak = 0; //Reset the killstreak
            displayedKillstreakText = false; //Allow the killstreak text to be shown again
        }
    }

    /// <summary>
    /// This method simply adds a delay to the respawn process.
    /// This prevents players from immediately respawning after despawning
    /// </summary>
    /// <returns></returns> Returns back to the method that called this coroutine for a set amount of seconds.
    private IEnumerator respawn()
    {
        yield return new WaitForSeconds(3f); //Wait for 3 seconds

        photonView.RPC("Respawn", RpcTarget.All); //Initiate the respawn RPC so it updates for all players in the scene
    }

    /// <summary>
    /// This RPC is called when we want the player to respawn. 
    /// It is responsible for reactivating all the necessary components to get the player back into the action.
    /// </summary>
    [PunRPC]
    public void Respawn()
    {
        playerHealth = maxHealth; //Reset the players health to their max health
        UpdateHealthBar(); //Update the healthbar to reflect the health change
        foreach (Transform child in transform) //Similarly to the despawn method, we want to iterate over all of the child objects
        {
            if (child.GetComponent<MeshRenderer>()) //If the object has a MeshRenderer component
            {
                child.GetComponent<MeshRenderer>().enabled = true; //We want to enable the component so it is visible
            }
        }

        GetComponent<BoxCollider>().enabled = true; //Reactivate the box collider to other players can interact with this player
        canShoot = true; //Allow the player to shoot again
    }

    /// <summary>
    /// This method is called when the player successfully kills another player.
    /// </summary>
    public void addKill()
    {
        if (photonView.IsMine) //Only execute the below code if we are controlling this object
        {
            killCount++; //Increment the amount of kills this player has
            killStreak++; //Increement the players current killstreak

            if (killStreak >= GameManager.killstreakForEffect && !displayedKillstreakText) //If the players killstreak is above a certain value
            {
                displayedKillstreakText = true; //Let the script know what we have displayed the killstreak text
                StartCoroutine("killstreakText"); //Start the corouting responsible for displaying the killstreak text
            }

            PlayerPrefs.SetInt("Kills", PlayerPrefs.GetInt("Kills") + 1); //Increment the kill count saved in the players registry
            AddExperience(10); //Add 10 experience to the player
        }
    }

    /// <summary>
    /// When we want to add experience to the player, we call this method.
    /// </summary>
    /// <param name="amt"></param> The amount of experience we want to give the player
    public void AddExperience(float amt)
    {
        exp += amt; //Increment the players experience by the amount set in the parameter
        expSlider.value = exp / 100; //Update the experience bar to reflect the above change

        if (exp >= 100) //If the players XP hits 100
        {
            photonView.RPC("RpcCreateLevelUpParticle", RpcTarget.All); //We want to create a particle effect to show the player has levelled up
            level += 1; //Increment the players level
            levelText.text = "Level: " + level; //Update the level text to reflect the above change
            exp = 0; //Reset the players XP back to 0
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1); //Increment the level in the players registry
            expSlider.value = 0; //Reset the XP bar back to 0
        }

        PlayerPrefs.SetFloat("Exp", exp); //We want to save the current XP the player has, so we set it in the registry
    }

    /// <summary>
    /// We call this method if we want to know if the player is on a killstreak
    /// </summary>
    /// <returns></returns> true or false
    public bool onKillStreak()
    {
        return killStreak >= GameManager.killstreakForEffect; //If the players killstreak is above the necessary amount to trigger effects 
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerator killstreakText()
    {
        GameObject.Find("KillstreakText").GetComponent<CanvasGroup>().alpha = 1;
        yield return new WaitForSeconds(3f);
        GameObject.Find("KillstreakText").GetComponent<CanvasGroup>().alpha = 0;
    }

    //////////////Instantiating particles///////////////////

    //Hit particle
    public void RpcCreateHitParticle(Vector3 position)
    {
        GameObject p = Instantiate(hitParticle, position, Quaternion.identity);
        Destroy(p, 5f);
    }

    //Level up particle
    [PunRPC]
    public void RpcCreateLevelUpParticle()
    {
        GameObject p = Instantiate(levelUpEffect, transform.position, Quaternion.identity);
        Destroy(p, 5f);
    }
    //Hit particle
    [PunRPC]
    public void RpcExplosiveParticle(Vector3 position)
    {
        GameObject p = Instantiate(explosiveParticle, position, Quaternion.identity);
        Destroy(p, 5f);
    }

    [PunRPC]
    public void RpcCreateBoostTrail(Vector3 position1, Vector3 position2, int id)
    {
        GameObject p = null;
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            if (player.GetComponent<PhotonView>().ViewID == id)
            {
                p = player.gameObject;
                return;
            }
        }

        GameObject p1 = Instantiate(boostTrail, position1, Quaternion.identity);
        GameObject p2 = Instantiate(boostTrail, position2, Quaternion.identity);

        p1.transform.parent = p.transform;
        p2.transform.parent = p.transform;
    }

    [PunRPC]
    public void RpcStopBoostTrail(int id)
    {
        GameObject p = null;
        Player[] players = FindObjectsOfType<Player>();
        foreach(Player player in players)
        {
            if(player.GetComponent<PhotonView>().ViewID == id)
            {
                p = player.gameObject;
                return;
            }
        }

        foreach(Transform child in p.transform)
        {
            if(child.GetComponent<ParticleSystem>())
            {
                child.GetComponent<ParticleSystem>().Stop();
                Destroy(child.GetComponent<ParticleSystem>().gameObject, 3f);
            }
        }
    }

    /// <summary>
    /// We call this method if we want to get the players current health
    /// </summary>
    /// <returns></returns> the players current health
    public float getHealth()
    {
        return playerHealth;
    }

    /// <summary>
    /// We call this method if we want to get the players maximum health
    /// </summary>
    /// <returns></returns> the players maxiumum health
    public float getMaxHealth()
    {
        return maxHealth;
    }

    /// <summary>
    /// We call this method if we want to grab the PhotonView component attached to this object.
    /// </summary>
    /// <returns></returns>
    public PhotonView GetPhotonView()
    {
        return photonView;
    }

    [PunRPC]
    public void updatePlayerList()
    {
        allPlayers.Clear();
        foreach (Player player in FindObjectsOfType<Player>())
        {
            if (!allPlayers.Contains(player))
            {
                allPlayers.Add(player);
            }
        }
    }
}

[Serializable]//create gun to fire weapons.
public class Gun
{
    [SerializeField] private Transform gunPosition;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject mine;

    public Transform getGunPosition()
    {
        return gunPosition;
    }

    public GameObject getBullet()
    {
        return bullet;
    }
    public GameObject getMine()
    {
        return mine;
    }
}
