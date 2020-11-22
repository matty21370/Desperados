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
	/// the player money
	/// </summary> 
    private int currency;

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

    Shop shop;
    Leaderboard leaderboard;
    GameManager manager;
    bool canMove;
    
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
        playerName = PlayerPrefs.GetString("Name");
    }

    /// <summary>
    /// This method is called as soon as the player enters the main scene. 
    /// It is responsible for setting up all of the required variables that cannot be set in the editor.
    /// </summary>
    void Start()
    {
        shop = FindObjectOfType<Shop>();
        shop.gameObject.SetActive(false);
        currency = 0;
        leaderboard = FindObjectOfType<Leaderboard>();
        leaderboard.player = this;
        manager = FindObjectOfType<GameManager>();

        canMove = true;

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

    /// <summary>
    /// This method is called once per frame, framerate dependent.
    /// Movement related commands are not recommended to go in here.
    /// </summary> //
    void Update()
    {
        if (!photonView.IsMine)
        {
            return; 
        }

        if (Input.GetMouseButtonDown(0) && photonView.IsMine && canShoot)
        {
            photonView.RPC("Shoot", RpcTarget.All); 
        }

        if (Input.GetKeyDown(KeyCode.G)) 
        {
            hitDetected(1); 
        }
        
        if(Input.GetKeyDown(KeyCode.X))
        {
            AddExperience(10); 
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            photonView.RPC("DropMine", RpcTarget.All);

        }
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            shop.setEnabled();

            if (shop.shopEnabled)
            {
                shop.gameObject.SetActive(true);
                UnityEngine.Cursor.visible = true;
                UnityEngine.Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                shop.gameObject.SetActive(false);
                UnityEngine.Cursor.visible = false;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            }
        }


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

        pingText.text = "Latency: " + PhotonNetwork.GetPing(); 
    }

    /// <summary>
    /// This Update method is framerate independent. Movement commands should go in here
    /// </summary>
    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        transform.forward = -GetComponentInChildren<Camera>().transform.forward; 

        HandleInput();
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
        if (canMove)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += -transform.forward * movementSpeed * Time.deltaTime;
                camera.fieldOfView += Time.deltaTime * 6;

                if (Input.GetKey(KeyCode.Space))
                {
                    movementSpeed = 11f;
                    camera.GetComponent<CameraMovement>().SetMaxFOV(95f);
                }
                else
                {
                    movementSpeed = 5f;
                    camera.GetComponent<CameraMovement>().SetMaxFOV(90f);
                }
            }
            else
            {
                camera.fieldOfView -= Time.deltaTime * 6;
            }

            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                transform.position += transform.right * movementSpeed * Time.deltaTime;
                float z = Input.GetAxis("Horizontal") * 15.0f;
                Vector3 euler = transform.localEulerAngles;
                euler.z = Mathf.Lerp(euler.z, z, 25.0f * Time.deltaTime);
                transform.localEulerAngles = euler;
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.position += transform.forward * movementSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                transform.position += -transform.right * movementSpeed * Time.deltaTime;
                float z = Input.GetAxis("Horizontal") * 15.0f;
                Vector3 euler = transform.localEulerAngles;
                euler.z = Mathf.Lerp(euler.z, z, 25.0f * Time.deltaTime);
                transform.localEulerAngles = euler;
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                transform.position += -transform.up * movementSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position += transform.up * movementSpeed * Time.deltaTime;
            }
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
        foreach (Gun g in guns)
        {
            GameObject bullet = Instantiate(g.getBullet(), g.getGunPosition().position, transform.rotation);
            bullet.GetComponent<Bullet>().InitializeBullet(gameObject, -transform.forward);
            Destroy(bullet, 5f);
        }
    }

    /// <summary>
    /// This RPC function allows the player to drop mines.
    /// </summary>
    [PunRPC]
    private void DropMine()
    {
        Gun g = guns[0];
        GameObject mine = Instantiate(g.getMine(), g.getGunPosition().position, transform.rotation);
        mine.GetComponent<Mine>().InitializeMine(gameObject); 
        mineNumber++; 
        Destroy(mine, 5f);
    }

    /// <summary>
    /// When the mine is destroyed, this method is called.
    /// </summary>
    public void lostMine()
    {
        mineNumber--; 
    }

    /// <summary>
    /// If we want to apply damage to a player. We call this method.
    /// </summary>
    /// <param name="damage"></param> The amount of damage we want to apply to the player.
    [PunRPC]
    public void hitDetected(int damage)
    {
        playerHealth -= damage;
        healthSlider.value = playerHealth; 
        UpdateHealthBar(); 
        if (playerHealth <= 0) 
        {
            photonView.RPC("Despawn", RpcTarget.All);
        }
    }

    /// <summary>
    /// We call this method after we modify the players health. This allows the healthbar to sync up.
    /// </summary>
    public void UpdateHealthBar()
    {
        if (photonView.IsMine)
        {
            healthSlider.value = playerHealth / 10; 
            healthText.text = playerHealth + "/" + maxHealth; 
        }
    }

    /// <summary>
    /// This method begins the death process.
    /// It is responsible for deactivating all of the necessary components to hide the player and prevent other players from interacting with it.
    /// </summary>
    [PunRPC]
    public void Despawn()
    {
        GameObject p = Instantiate(explosionParticle, transform.position, Quaternion.identity); 
        transform.position = Vector3.zero; 
        Destroy(p, 5f); 
        canShoot = false;
        canMove = false;
        deaths += 1;
        killStreak = 0;
        displayedKillstreakText = false;

        foreach (Transform child in transform)
        {
            if(child.GetComponent<MeshRenderer>()) 
            {
                child.GetComponent<MeshRenderer>().enabled = false; 
            }
        }

        GetComponent<BoxCollider>().enabled = false;
        mineNumber = 0;
        
        if(photonView.IsMine)
        {
            PlayerPrefs.SetInt("Deaths", PlayerPrefs.GetInt("Deaths") + 1);
        }

        StartCoroutine("respawn");
    }

    /// <summary>
    /// This method simply adds a delay to the respawn process.
    /// This prevents players from immediately respawning after despawning
    /// </summary>
    /// <returns></returns> Returns back to the method that called this coroutine for a set amount of seconds.
    private IEnumerator respawn()
    {
        yield return new WaitForSeconds(3f); 

        photonView.RPC("Respawn", RpcTarget.All);
    }

    /// <summary>
    /// This RPC is called when we want the player to respawn. 
    /// It is responsible for reactivating all the necessary components to get the player back into the action.
    /// </summary>
    [PunRPC]
    public void Respawn()
    {
        playerHealth = maxHealth; 
        UpdateHealthBar(); 
        foreach (Transform child in transform) 
        {
            if (child.GetComponent<MeshRenderer>())
            {
                child.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        GetComponent<BoxCollider>().enabled = true; 
        canShoot = true;
        canMove = true;
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
            addCurrency();
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

    /// <summary>
    /// add funds
    /// </summary>

    private void addCurrency()
    {
        currency = currency + 20;
    }

    public int getCurrency()
	{
        return currency;
	}

    public void purchaseMade(int price)
	{
        currency = currency + price;
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




    public void upgradePurchasedHealth()
	{
        maxHealth = 20;
        playerHealth = maxHealth;
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
