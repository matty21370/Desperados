using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script created by: Matthew Burke, Andrew Viney
/// </summary>
public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    /// <summary>
    /// This is a reference to the name of the player
    /// </summary>
    private string playerName;
    private string userName;

    /// <summary>
    /// This is the base speed of the player, can be upgraded later
    /// </summary>
   // [SerializeField] private float baseSpeed = 5f;

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
    private bool outOfBounds;
    /// <summary>
    /// This is a reference to the camera object so it can be modified later
    /// </summary>
    private Camera camera;

    /// <summary>
    /// This is a list of all the guns currently attached to the player
    /// </summary>
    [SerializeField] private List<Gun> guns = new List<Gun>();

    /// <summary>
    /// check if the user can use mines
    /// </summary>
    private bool minesEnabled=false;

    /// <summary>
    /// The below variables are references to all the UI components for the player
    /// </summary>
    private Text levelText;
    private Text healthText;
    private Text joinText;
    private Text pingText;
    private UnityEngine.UI.Slider healthSlider;
    private UnityEngine.UI.Slider expSlider;

    private GameObject levelUpNotification;

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

    private Shop shop;
    Leaderboard leaderboard;
    GameManager manager;
    bool canMove;
    private int speedIncrease = 1;
    private int bulletDamage = 1;
    private GameObject pauseMenu;
    private CanvasGroup pauseMenuGroup;

   private Text overHeatText;
   
    private int shotsLeft = 12;
    private float weaponCool = 2f;
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
    /// This is called multiple times per second to ensure proper synchronisation between players
    /// </summary>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerHealth);
            stream.SendNext(canShoot);
            stream.SendNext(level);
            stream.SendNext(killCount);
            stream.SendNext(killsThisGame);
            stream.SendNext(team);
            stream.SendNext(playerName);
           
        }
        else if (stream.IsReading) 
        {
            playerHealth = (float)stream.ReceiveNext();
            canShoot = (bool)stream.ReceiveNext();
            level = (int)stream.ReceiveNext();
            killCount = (int)stream.ReceiveNext();
            killsThisGame = (int)stream.ReceiveNext();
            team = (int)stream.ReceiveNext();
            playerName = (string)stream.ReceiveNext();
            
        }
    }

    /// <summary>
    /// This method is called as soon as the player enters the main scene. 
    /// It is responsible for setting up all of the required variables that cannot be set in the editor.
    /// </summary>
    void Start()
    {
        if(photonView.IsMine)
        {
            userName = PlayerPrefs.GetString("Name"); 
            playerName = userName;
        }

        levelUpNotification = GameObject.Find("Level Up");
      
        currency = 100;
        //set starting currency
        shop = FindObjectOfType<Shop>();
        shop.gameObject.SetActive(false);
        //set the players shop and hide it
        overHeatText = FindObjectOfType<Text>();
        overHeatText.text = "";
        //set player warning text and hide it
        outOfBounds = false;
       
        leaderboard = FindObjectOfType<Leaderboard>();
        leaderboard.player = this;
        manager = FindObjectOfType<GameManager>();
        pauseMenu = GameObject.Find("Menu");
        pauseMenuGroup = pauseMenu.GetComponent<CanvasGroup>();

        canMove = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        healthText = GameObject.Find("Health Text").GetComponent<Text>(); 
        healthSlider = GameObject.Find("Health Background").GetComponent<Slider>();
        levelText = GameObject.Find("Level Text").GetComponent<Text>();
        expSlider = FindObjectOfType<Slider>();

        playerHealth = maxHealth; 
        UpdateHealthBar();

        level = PlayerPrefs.GetInt("Level"); 
        levelText.text = "Level: " + level; 

        exp = PlayerPrefs.GetFloat("Exp");
        expSlider.value = exp / 100; 

        pingText = GameObject.Find("Ping Text").GetComponent<Text>();

        camera = GetComponentInChildren<Camera>(); 

        team = UnityEngine.Random.Range(1, 2);

        if(!photonView.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
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
            if (shotsLeft > 0)
            {
                photonView.RPC("Shoot", RpcTarget.All);
                shotsLeft--;
			}
			else
			{
                weaponOverheat();
			}
        }

        if (Input.GetKeyDown(KeyCode.G)) 
        {
            //hitDetected(1); 
        }
        
        if(Input.GetKeyDown(KeyCode.X))
        {
            AddExperience(10); 
        }

        if (Input.GetKeyDown(KeyCode.T) && (minesEnabled))
        {
            photonView.RPC("DropMine", RpcTarget.All);

        }

        if (Input.GetKeyDown(KeyCode.I)) 
        {
            shop.setEnabled();

            if (shop.shopEnabled)
            {
                shop.gameObject.SetActive(true);
                shop.updateText(currency);
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
            }
            else
            {
                leaderboard.HideLeaderboard();
                leaderboardOpen = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindObjectOfType<AudioManager>().Play("BoostNoise");
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            FindObjectOfType<AudioManager>().Stop("BoostNoise");
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

    /// <summary>
    /// This method handles the player movement based on keyboard input.
    /// </summary>
    void HandleInput()
    {
        if (canMove)
        {
            CheckOutOfBounds();
            if (Input.GetKey(KeyCode.W) )
            {
                
                transform.position += -transform.forward * (speedIncrease *movementSpeed) * Time.deltaTime;
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

            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) )
            {
                transform.position += transform.right * (speedIncrease * movementSpeed) * Time.deltaTime;
                float z = Input.GetAxis("Horizontal") * 15.0f;
                Vector3 euler = transform.localEulerAngles;
                euler.z = Mathf.Lerp(euler.z, z, 25.0f * Time.deltaTime);
                transform.localEulerAngles = euler;
            }
            

            if (Input.GetKey(KeyCode.S))
            {
                transform.position += transform.forward * (speedIncrease * movementSpeed) * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                transform.position += -transform.right * (speedIncrease * movementSpeed) * Time.deltaTime;
                float z = Input.GetAxis("Horizontal") * 15.0f;
                Vector3 euler = transform.localEulerAngles;
                euler.z = Mathf.Lerp(euler.z, z, 25.0f * Time.deltaTime);
                transform.localEulerAngles = euler;
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                transform.position += -transform.up * (speedIncrease * movementSpeed) * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position += transform.up * (speedIncrease * movementSpeed) * Time.deltaTime;
            }
        }
    }

    private void ToggleMenu()
    {
        if(pauseMenuGroup.alpha == 0)
        {
            pauseMenuGroup.alpha = 1;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(pauseMenuGroup.alpha == 1)
        {
            pauseMenuGroup.alpha = 0;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void LeaveSession()
    {
        manager.LeaveRoom();
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
            bullet.GetComponent<Bullet>().InitializeBullet(gameObject, -transform.forward, bulletDamage);
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
        
        Destroy(mine, 10f);
    }

    [PunRPC]
    public void NetworkAddKill()
    {
        killCount += 1;

        if(photonView.IsMine)
        {
            PlayerPrefs.SetInt("Kills", PlayerPrefs.GetInt("Kills") + 1);
            AddExperience(10);
            addCurrency();
        }
    }

    [PunRPC]
    public void NetworkAddDeath()
    {
        deaths += 1;

        if(photonView.IsMine)
        {
            PlayerPrefs.SetInt("Deaths", PlayerPrefs.GetInt("Deaths") + 1);
        }
    }

    /// <summary>
    /// If we want to apply damage to a player. We call this method.
    /// </summary>
    /// <param name="damage"> The amount of damage we want to apply to the player.</param>
    public void hitDetected(int damage, Player who)
    {
        if (photonView.IsMine)
        {
            playerHealth = playerHealth - damage;//damage;
            UpdateHealthBar();

            if (playerHealth <= 0)
            {
                photonView.RPC("dropPack", RpcTarget.All);
                photonView.RPC("Despawn", RpcTarget.All);
                who.photonView.RPC("NetworkAddKill", RpcTarget.AllBuffered);
            }
        }
    }

    /// <summary>
    /// We call this method after we modify the players health. This allows the healthbar to sync up.
    /// </summary>
    public void UpdateHealthBar()
    {
        if (photonView.IsMine)
        {
            healthSlider.value = playerHealth / maxHealth; 
            healthText.text = playerHealth + "/" + maxHealth; 
        }
    }

    public Shop getShop()
	{
        return shop;
	}

    /// <summary>
    /// This method begins the death process.
    /// It is responsible for deactivating all of the necessary components to hide the player and prevent other players from interacting with it.
    /// </summary>
    [PunRPC]
    public void Despawn()
    {
        GameObject p = Instantiate(explosionParticle, transform.position, Quaternion.identity);
        transform.position = manager.spawnPoints[UnityEngine.Random.Range(0, manager.spawnPoints.Count)].position;
        Destroy(p, 5f);

        if (photonView.IsMine)
        {
            canShoot = false;
            canMove = false;
            photonView.RPC("NetworkAddDeath", RpcTarget.AllBuffered);
            killStreak = 0;
            displayedKillstreakText = false;
        }

        foreach (Transform child in transform)
        {
            if (child.GetComponent<MeshRenderer>())
            {
                child.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        GetComponent<BoxCollider>().enabled = false;
     
       
        if (photonView.IsMine)
        {
            PlayerPrefs.SetInt("Deaths", PlayerPrefs.GetInt("Deaths"));// + 1);
       }
       
        StartCoroutine("RespawnTimer");
    }

    /// <summary>
    /// This method simply adds a delay to the respawn process.
    /// This prevents players from immediately respawning after despawning
    /// </summary>
    /// <returns>Returns back to the method that called this coroutine for a set amount of seconds.</returns> 
    private IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(3f);

        Respawn();
    }

    /// <summary>
    /// This RPC is called when we want the player to respawn. 
    /// It is responsible for reactivating all the necessary components to get the player back into the action.
    /// </summary>
    public void Respawn()
    {
        playerHealth = maxHealth; 
        UpdateHealthBar();
        shotsLeft = 12;
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
    /// When we want to add experience to the player, we call this method.
    /// </summary>
    /// <param name="amt">The amount of experience we want to give the player</param> 
    public void AddExperience(float amt)
    {
        exp += amt; 
        expSlider.value = exp / 100;

        if (exp >= 100) 
        {
            photonView.RPC("RpcCreateLevelUpParticle", RpcTarget.All); 
            level += 1;
            StartCoroutine(levelUpPopup());
            levelText.text = "Level: " + level;
            exp = 0;
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1); 
            expSlider.value = 0; 
        }

        PlayerPrefs.SetFloat("Exp", exp); 
    }

    private IEnumerator levelUpPopup()
    {
        levelUpNotification.GetComponent<CanvasGroup>().alpha = 1;
        levelUpNotification.GetComponent<LevelUpNotification>().SetText("Rank: " + level);
        yield return new WaitForSeconds(2f);
        levelUpNotification.GetComponent<CanvasGroup>().alpha = 0;
    }

    /// <summary>
    /// We call this method if we want to know if the player is on a killstreak
    /// </summary>
    public bool onKillStreak()
    {
        return killStreak >= GameManager.killstreakForEffect; 
    }

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

    /// <summary>
    /// We call this method if we want to get the players current health
    /// </summary>
    /// <returns>the players current health</returns> 
    public float getHealth()
    {
        return playerHealth;
    }

    /// <summary>
    /// We call this method if we want to get the players maximum health
    /// </summary>
    /// <returns>the players maxiumum health</returns> 
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
    /// <summary>
    //get the amount of points the player has
    /// </summary>
    public int getCurrency()
	{
        return currency;
	}
    /// <summary>
    //take points for a purchase
    /// </summary>
    public void purchaseMade(int price)
	{
        currency = currency - price;
	}
    /// <summary>
    //increase player health
    /// </summary>
    public void upgradePurchasedHealth()
	{
        maxHealth = 20;
        playerHealth = maxHealth;   
        //this may need to be change as will heal player if they purchase upgrade
        UpdateHealthBar();
    }
    /// <summary>
    //speed the player up
    /// </summary>
    public void upgradePurchasedSpeed()
    {
        speedIncrease=2 ;
    }
    /// <summary>
    //allow mines to be used
    /// </summary>
    public void unlockMines()
	{
        minesEnabled = true;
	}
    /// <summary>
    //reduce the cool down
    /// </summary>
    public void cooldownUpgrade()
	{
        weaponCool = weaponCool / 2;
        //half the cooldown
	}


    /// <summary>
    ///set up text and start timer for cool down 
    /// </summary>
    [PunRPC]
    private void weaponOverheat()
	{
        Debug.Log("Weapon Overheating ");

        overHeatText.color = new Color(255, 0, 0, 1);
        //set text colour to red
        overHeatText.text = "Weapon Overheating";
        //set warning message
        StartCoroutine("CoolDownTimer");
        //run timmer

        FindObjectOfType<AudioManager>().Play("Overheat");

    }
    /// <summary>
    ///reset the weapon so that it can fire again
    /// </summary>
    [PunRPC]
    private void weaponCoolDown()
	{
       
        overHeatText.text = "";
        //make the text invisible
        shotsLeft = 12;
        //reset the number of shots
    }
    /// <summary>
    ///runs while the weapon is unable to fire
    /// </summary>
    private IEnumerator CoolDownTimer()
    {
        yield return new WaitForSeconds(2f);
        
        weaponCoolDown();
    }


    public void HealthBoost(int boost)
	{
        if (playerHealth <= maxHealth-5)
        {
            playerHealth = playerHealth + boost;
		}
		else
		{
            playerHealth = maxHealth;
		}
        UpdateHealthBar();
	}
    [PunRPC]
    private void dropPack()
	{
        Gun g = guns[0];
     GameObject pack = Instantiate(g.getHealthPack(), g.getGunPosition().position, transform.rotation);
       
        pack.GetComponent<HealthPack>().InitializePack(5);

        Destroy(pack,20f);
    }


    public void CheckOutOfBounds()
	{
        if (transform.position.z <= -180) {
            transform.position = new Vector3(transform.position.x, transform.position.y  , -170);
        }
        else if (transform.position.z >= 300)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -290);         
        }
        if(transform.position.y <= -180)
		{
            transform.position = new Vector3(transform.position.x, -170, transform.position.z);
        }
        else if (transform.position.y >= 300)
        {
            transform.position = new Vector3(transform.position.x, 290, transform.position.z);
        }
        if (transform.position.x >= 300)
        {
            transform.position = new Vector3(280, transform.position.y, transform.position.y);   
        }
        else if (transform.position.x <= -180)
        {
            transform.position = new Vector3(170, transform.position.y, transform.position.y);
            }
    }
}

[Serializable]//create gun to fire weapons.
public class Gun
{
    [SerializeField] private Transform gunPosition;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject mine;
    [SerializeField] private GameObject healthPack;

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
    public GameObject getHealthPack()
    {
        return healthPack;
    }

}
