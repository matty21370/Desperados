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
    private bool checkCalled = false;
    private  float Xpos;
     private  float Ypos;
     private  float Zpos;
    private bool positionSet = false;

    // Start is called before the first frame update

    public void spawn()
    {
    }
    public void start()
	{
        Xpos = Random.Range(-160, 280);
        Ypos = Random.Range(-160, 280);
        Zpos = Random.Range(-160, 280);

    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(obstacleHealth);
            stream.SendNext(notDestroyed);
            stream.SendNext(movement);

            stream.SendNext(Xpos);
            stream.SendNext(Ypos);
            stream.SendNext(Zpos);

        }
        else if (stream.IsReading)
        {
            obstacleHealth = (int)stream.ReceiveNext();
            notDestroyed = (bool)stream.ReceiveNext();
            movement = (float)stream.ReceiveNext();
            Xpos=(float)stream.ReceiveNext();
            Ypos = (float)stream.ReceiveNext();
            Zpos = (float)stream.ReceiveNext();
        }
        
    }


    // Update is called once per frame
    void Update()
    {
		/*if (!positionSet)
		{
            setPosition();
            positionSet = true;
        }*/
        if (!notDestroyed && !checkCalled)
        {
            photonView.RPC("Despawn", RpcTarget.All);
            checkCalled = true;
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

    private void setPosition()
	{
         transform.position = new Vector3(Xpos,Ypos,Zpos);
	}


}
