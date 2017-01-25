﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeGraph {

    [Serializable]
    [CreateAssetMenu(fileName = "NodeGraph", menuName = "NodeGraph", order = 1)]
    public class Graph : ScriptableObject {

        public List<Node> Nodes = new List<Node>();
        public List<Edge> Edges = new List<Edge>();

        public Rect GraphExtents;

#if UNITY_EDITOR
        private Editor.GraphView _View;
        public Editor.GraphView GetView() {
            if (_View == null)
                _View = Editor.GraphView.Instantiate<Editor.GraphView>(this);

            return _View;
        }
#endif
    }
}
 