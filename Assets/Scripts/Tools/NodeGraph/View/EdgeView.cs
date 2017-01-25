using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeGraph.Editor {


    public class EdgeView : ScriptableObject {

        public Edge Edge { get; private set; }

        public static T Instantiate<T>(Edge edge) where T : EdgeView {
            var result = ScriptableObject.CreateInstance<T>();
            result.Edge = edge;
            result.hideFlags = HideFlags.HideAndDontSave;
            return result;
        }
    }
}
