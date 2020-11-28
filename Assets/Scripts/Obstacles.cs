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
        obstacleHealth = 10;
        movement = Random.Range(1, 4);
       
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


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collide");
        Debug.Log(collision.transform.tag);
          if (collision.transform.tag == "Player")
            {
            
                collision.gameObject.GetComponent<Player>().hitDetected(1);
                               
            }
                 
        
    }


    public void reduceHealth(int damage)
	{
        obstacleHealth = obstacleHealth - damage;
        if (obstacleHealth <= 0)
		{
            GameObject p = Instantiate(explosionParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }
	}
}
