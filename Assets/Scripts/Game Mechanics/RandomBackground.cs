using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomBackground : MonoBehaviour
{
    public Image Background;

    public Sprite[] Sprites;

    public int x; 

    void Start()
    {
        x = Random.Range(0,3);

        Background.sprite = Sprites[x];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
