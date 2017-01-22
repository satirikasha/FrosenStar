﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planets_rotation : MonoBehaviour
{
    void Update()
    {
        // Rotate the object around its local X axis at 1 degree per second
        transform.Rotate(Vector3.up * Time.deltaTime * 0.25f);

        // ...also rotate around the World's Y axis
       // transform.Rotate(Vector3.up * Time.deltaTime, Space.World);
    }
}
