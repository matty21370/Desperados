using Photon.Pun;
using UnityEngine;

/// <summary>
/// Script created by: Matthew Burke, Andrew Viney
/// </summary>
public class Bullet : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// This is the speed we want the bullet to move at
    /// </summary>
    [SerializeField] private float speed;

    /// <summary>
    /// This is the damage we want the bullet to apply to the player when collided
    /// </summary>
    //  [SerializeField] private int damage;
    public int damage;
    /// <summary>
    /// The owner of this bullet.
    /// Neccessary to avoid the player collider with their own bullet.
    /// </summary>
    private GameObject owner;

    /// <summary>
    /// We want to store the bullets movement in a variable so we can modify it.
    /// </summary>
    Vector3 movement;

    /// <summary>
    /// This method is called when a player fires the bullet.
    /// It sets the required variables to get the bullet moving.
    /// </summary>
    /// <param name="owner">The player who fired the bullet</param> 
    /// <param name="movement">The movement we want to apply to the bullet</param> 
    public void InitializeBullet(GameObject owner, Vector3 movement, int damage)
    {
        this.owner = owner;
        this.movement = movement;
        this.damage = damage;
    }

    /// <summary>
    /// This method is called once per frame
    /// </summary>
    private void Update()
    {
       transform.position += movement * speed * Time.deltaTime; 
    }

    /// <summary>
    /// Call this method if we want to grab the damage of this bullet
    /// </summary>
    public int getDamage()
    {
        return damage;
    }

    public void damageIncrease()
	{
        damage = 2;
	}

    /// <summary>
    /// Call this method if we want to grab the speed of the bullet
    /// </summary>
    public float getSpeed()
    {
        return speed; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != owner) 
        {
            if (collision.transform.tag == "Player")
            {

                collision.gameObject.GetComponent<Player>().hitDetected(this.getDamage());
                Debug.Log(this.getDamage());
                if (collision.gameObject.GetComponent<Player>().getHealth() <= 0)
                {
                    owner.GetComponent<Player>().addKill(); 
                }
            }
            else if (collision.transform.tag == "Mine")
            {
                collision.gameObject.GetComponent<Mine>().hit(); 
            }

            owner.GetComponent<Player>().RpcCreateHitParticle(transform.position); 
            Destroy(gameObject); 
        }
    }
}
