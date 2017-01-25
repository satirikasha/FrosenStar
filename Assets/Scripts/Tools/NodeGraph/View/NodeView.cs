using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace NodeGraph.Editor {


    public class NodeView : ScriptableObject {

        public Node Node { get; private set; }

        public static T Instantiate<T>(Node node) where T : NodeView {
            var result = ScriptableObject.CreateInstance<T>();
            result.Node = node;
            result.hideFlags = HideFlags.HideAndDontSave;
            return result;
        }

        public void OnNodeGUI() {
            Node.Position = GUILayout.Window(this.GetInstanceID(), Node.Position, NodeWindowGUI, "a window", (GUIStyle)"window");
        }

        public void NodeWindowGUI(int id) {
            if (GUILayout.Button("Hello World"))
                Debug.Log("Got a click");

            GUI.DragWindow();
        }
    }
}
