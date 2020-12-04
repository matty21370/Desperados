using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePickup : MonoBehaviour
{
    public int minCoinsToGive = 10, maxCoinsToGive = 20;
    private int scoreToGive;

    // Start is called before the first frame update
    void Start()
    {
        scoreToGive = Random.Range(minCoinsToGive, maxCoinsToGive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
