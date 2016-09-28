using UnityEngine;
using System.Collections;
using System;

namespace Tools {


    public static partial class Utils {

        public static void WaitUntil(this MonoBehaviour behaviour, Func<bool> condition, Action callback) {
            behaviour.StartCoroutine(WaitUntilTask(condition, callback));
        }

        private static IEnumerator WaitUntilTask(Func<bool> condition, Action callback) {
            yield return new WaitUntil(condition);
            callback();
        }
    }
}
