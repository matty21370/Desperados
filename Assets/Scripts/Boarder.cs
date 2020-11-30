using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boarder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collide");
       // Debug.Log(collision.transform.tag);
        if (collision.transform.tag == "Player")
        //if the player has collided
        {

          //  collision.gameObject.GetComponent<Player>().setOutOfBounds();
            //give the player damage

        }


    }
}
