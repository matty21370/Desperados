using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    private int heal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitializePack(int heal)
    {
        this.heal = heal;
    }

    private void OnCollisionEnter(Collision collision)
    {
     
            if (collision.transform.tag == "Player")
            {
                   collision.gameObject.GetComponent<Player>().HealthBoost(heal);
            Destroy(gameObject);

        }
           
        
    }
}
