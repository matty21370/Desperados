using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine;

public class Obstacles : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private int obstacleHealth ;
    [SerializeField] private GameObject explosionParticle;

    private float movement;

    private bool notDestroyed = true;
    private bool checkCalled = false;

    private float Xpos;
    private float Ypos;
    private float Zpos;

    private bool positionSet = false;

    Player host;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(obstacleHealth);
            stream.SendNext(notDestroyed);
        }
        else if (stream.IsReading)
        {
            obstacleHealth = (int)stream.ReceiveNext();
            notDestroyed = (bool)stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!checkCalled && !notDestroyed)
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
        if (collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().hitDetected(1, null);
        }
    }

    /// <summary>
	/// if anything hits the object
	/// </summary>
	/// <param name="collision">
	/// the object that has hit the object
	/// </param>
    public void reduceHealth(int damage)
	{
        obstacleHealth = obstacleHealth - damage;

        if (obstacleHealth <= 0)
		{
            FindObjectOfType<AudioManager>().Play("MeteorExplode", transform.position);
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
            photonView.RPC("Despawn", RpcTarget.All);
        }
	}

    [PunRPC]
    public void Despawn()
    {
        notDestroyed = false;
        GetComponent<Obstacles>().GetComponent<MeshRenderer>().enabled=false;
        GetComponent<MeshCollider>().enabled = false;
        Invoke("Respawn", 50);
    }

    public void Respawn()
    {
        notDestroyed = true;
        GetComponent<Obstacles>().GetComponent<MeshRenderer>().enabled = true;
        GetComponent<MeshCollider>().enabled = true;
        obstacleHealth = 20;
    }

    private void setPosition()
	{
        Xpos = Random.Range(-160, 280);
        Ypos = Random.Range(-160, 280);
        Zpos = Random.Range(-160, 280);
        transform.position = new Vector3(Xpos,Ypos,Zpos);
        positionSet = true;
	}

    //facilitating testing
    public int getHealth()
	{
     return   obstacleHealth;
	}
}
