using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine;

public class Obstacles : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private int obstacleHealth ;
    [SerializeField] private GameObject explosionParticle;
    private float movement;//= Random.Range(1, 4);
    private bool notDestroyed = true;
    /*  private const float Xpos;
     private const float Ypos;
     private const float Zpos;
    */

    // Start is called before the first frame update

    public void spawn()
    {
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(obstacleHealth);
            stream.SendNext(notDestroyed);
            stream.SendNext(movement);

        }
        else if (stream.IsReading)
        {
            obstacleHealth = (int)stream.ReceiveNext();
            notDestroyed = (bool)stream.ReceiveNext();
            movement = (int)stream.ReceiveNext();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!notDestroyed)
        {
            photonView.RPC("Despawn", RpcTarget.All);
        }
        else
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
         FindObjectOfType<AudioManager>().Play("MeteorExplode");
            GameObject p = Instantiate(explosionParticle, transform.position, Quaternion.identity);
            photonView.RPC("Despawn", RpcTarget.All);
           

        }
	}



    [PunRPC]
    public void Despawn()
    {

        notDestroyed = false;
        
        // Destroy(gameObject);
        this.GetComponent<Obstacles>().GetComponent<MeshRenderer>().enabled=false;
        GetComponent < CapsuleCollider > ().enabled = false;


    }

    [PunRPC]
    void SetAll()
    {
        gameObject.GetComponent<Obstacles>().obstacleHealth = obstacleHealth;
    }


}
