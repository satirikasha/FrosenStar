﻿using UnityEngine;
using System.Collections;

public static partial class Utils {

    public static float GetRandomValue(this Vector2 vector) {
        return Random.Range(vector.x, vector.y);
    }

}
