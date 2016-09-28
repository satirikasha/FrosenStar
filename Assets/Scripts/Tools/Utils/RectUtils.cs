using UnityEngine;
using System.Collections;

namespace Tools {


    public static partial class Utils {

        public static bool Contains(this Rect target, Rect rect) {
            return target.Contains(rect.max) && target.Contains(rect.min);
        }
    }
}
