using UnityEngine;
using System.Collections;

namespace Tools {


    public static partial class Utils {

        public static float GetRandomValue(this Vector2 vector) {
            return Random.Range(vector.x, vector.y);
        }

        public static float GetMaxValue(this Vector3 vector) {
            return Mathf.Max(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
        }
    }
}
