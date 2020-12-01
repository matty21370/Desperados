using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine;

public class Obstacles : MonoBehaviour//PunCallbacks
{
    private int obstacleHealth;
    [SerializeField] private GameObject explosionParticle;
    private float movement;
    // Start is called before the first frame update
    void Start()
    {
        obstacleHealth = Random.Range(10,30);
        movement = Random.Range(1, 4);
        transform.position = new Vector3(Random.Range(-160,250), Random.Range(-160, 250), Random.Range(-160, 250));

    }
  
   
    // Update is called once per frame
    void Update()
    {
        // rb.velocity = new Vector3(0, movement, 0);

        // transform.Rotate((Vector3.right *2* Time.deltaTime));
        if (movement == 1)
        {
            transform.Rotate(0, 2 * Time.deltaTime, 0);
        }else if (movement == 2)
		{
            transform.Rotate(0, 0, 2 * Time.deltaTime);
        }
        else if (movement == 3)
		{
            transform.Rotate(2 * Time.deltaTime, 0, 0);

        }
    }

    /// <summary>
	/// if anything hits the object
	/// </summary>
	/// <param name="collision">
	/// the object that has hit the object
	/// </param>
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collide");
        Debug.Log(collision.transform.tag);
          if (collision.transform.tag == "Player")
            //if the player has collided
            {
            
                collision.gameObject.GetComponent<Player>().hitDetected(1);
            //give the player damage
                               
            }
          else if (collision.transform.tag == "Bullet")
		{
            reduceHealth(collision.gameObject.GetComponent<Bullet>().getDamage());
		}
                 
        
    }

    /// <summary>
    /// if object is shot. sound will be played
    /// </summary>
    /// <param name="collision">
    /// the object that has hit the object
    /// </param>
    private void reduceHealth(int damage)
	{
        obstacleHealth = obstacleHealth - damage;
        if (obstacleHealth <= 0)
		{
            GameObject p = Instantiate(explosionParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
            FindObjectOfType<AudioManager>().Play("MeteorExplode");

        }
	}
}
