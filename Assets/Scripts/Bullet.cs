using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField] private int damage;

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
    /// <param name="owner"></param> The player who fired the bullet
    /// <param name="movement"></param> The movement we want to apply to the bullet
    public void InitializeBullet(GameObject owner, Vector3 movement)
    {
        this.owner = owner; //Set the bullets owner to the owner passed into the method
        this.movement = movement; //Set the movement to the movement passed into the method
    }

    /// <summary>
    /// This method is called once per frame
    /// </summary>
    private void Update()
    {
       transform.position += movement * speed * Time.deltaTime; //We want to constantly move the bullet 
    }

    /// <summary>
    /// Call this method if we want to grab the damage of this bullet
    /// </summary>
    /// <returns></returns>
    public int getDamage()
    {
        return damage; //Returns the damage of this bullet
    }

    /// <summary>
    /// Call this method if we want to grab the speed of the bullet
    /// </summary>
    /// <returns></returns>
    public float getSpeed()
    {
        return speed; //Returns the speed of the bullet
    }

    /// <summary>
    /// This method is called when the bullet collides with another object
    /// </summary>
    /// <param name="collision"></param> The object we have collided with.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != owner) //If the collided object is not our owner
        {
            if (collision.transform.tag == "Player") //If the collided object has the 'Player' tag applied to it
            {
                collision.gameObject.GetComponent<Player>().hitDetected(damage); //We want to grab the player script off the collider and call the hitDetected method
                if (collision.gameObject.GetComponent<Player>().getHealth() <= 0)//if this bullet was responsible for killing the player it collided with
                {
                    owner.GetComponent<Player>().addKill(); //We want to tell the owners player script that they killed another player
                }
            }
            else if (collision.transform.tag == "Mine")//If the bullet hits a mine
            {
                collision.gameObject.GetComponent<Mine>().hit(); //We want to tell the mine script that it has been hit.
            }

            owner.GetComponent<Player>().RpcCreateHitParticle(transform.position); //We want the owner to create a hit particle that all clients can see
            Destroy(gameObject); //Destroy the bullet
        }
    }
}
