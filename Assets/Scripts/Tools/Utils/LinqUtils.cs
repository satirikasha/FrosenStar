using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Tools {


    public static partial class Utils {

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action) {
            foreach (var item in collection) {
                action(item);
            }
        }
    }
}
