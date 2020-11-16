using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script created by: Matthew Burke
/// </summary>
public class KeepRotation : MonoBehaviour
{
    Quaternion rotation; //This is a reference to the rotation of the object this script is attached to

    /// <summary>
    /// This method is called as soon as the player enters the scene
    /// </summary>
    private void Awake()
    {
        rotation = transform.rotation; //We want to set the rotation variable to the objects current rotation
    }

    /// <summary>
    /// This is called just after the Update method is called
    /// </summary>
    private void LateUpdate()
    {
        transform.rotation = rotation; //We want the object to keep to the rotation specified in the Awake function.
    }
}
