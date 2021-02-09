using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    Vector3 parent;

    public void Init(Vector3 start, Vector3 parent)
    {
        transform.position = start;
        this.parent = parent;
        Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += parent * Time.deltaTime * 200;
    }
}
