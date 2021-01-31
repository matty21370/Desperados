using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(FindObjectOfType<Camera>().transform.position);
        transform.localScale = new Vector3(-1, 1, 1);
    }
}
