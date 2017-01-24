using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeGraph {


    public class Graph : ScriptableObject {

        public List<Node> Nodes = new List<Node>();
        public List<Edge> Edges = new List<Edge>();
    }
}
 