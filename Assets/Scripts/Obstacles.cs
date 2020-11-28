using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine;

public class Obstacles : MonoBehaviour//PunCallbacks
{
    private int obstacleHealth;
    [SerializeField] private GameObject explosionParticle;
    // Start is called before the first frame update
    void Start()
    {
        obstacleHealth = 10;
    }
    private Rigidbody rb;
    // Update is called once per frame
    void Update()
    {
       // transform.position += movement* Time.deltaTime;
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
