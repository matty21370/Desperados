using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine;

public class Obstacles : MonoBehaviourPunCallbacks
{
    private int obstacleHealth =10;
    [SerializeField] private GameObject explosionParticle;
    private float movement;//= Random.Range(1, 4);
    /*  private const float Xpos;
     private const float Ypos;
     private const float Zpos;
    */
 
   // Start is called before the first frame update
  
    public void spawn()
	{
    }
   

    // Update is called once per frame
    void Update()
    {
        movement = Random.Range(1, 4);
		
        if (movement == 1)
        {
            transform.Rotate(0, 2 * Time.deltaTime, 0);
        }
        else if (movement == 2)
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
	/// if anything hits the object
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
