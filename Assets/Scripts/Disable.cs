﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
