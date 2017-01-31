using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace NodeGraph.Editor {


    public class NodeView : ScriptableObject {

        protected virtual GUISkin Skin {
            get {
                if (LoadedSkin == null)
                    LoadedSkin = (GUISkin)Resources.Load(EditorGUIUtility.isProSkin ? "NodeGraphSkin" : "NodeGraphSkinLight");
                return LoadedSkin;
            }
        }
        protected GUISkin LoadedSkin;

        public Node Node { get; private set; }

        public static T Instantiate<T>(Node node) where T : NodeView {
            var result = ScriptableObject.CreateInstance<T>();
            result.Node = node;
            result.hideFlags = HideFlags.HideAndDontSave;
            return result;
        }

        void OnEnable() {
            Debug.Log("Enabled: NodeView");
        }

        public void OnNodeGUI() {
            Node.Position = GUILayout.Window(this.GetInstanceID(), new Rect(Node.Position.position, Vector2.zero), NodeWindowGUI, "", Skin.GetStyle("Window"));
        }

        public void NodeWindowGUI(int id) {
            GUILayout.Label("Window", Skin.GetStyle("Label"));
            GUILayout.Button("И так сойдет!");
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Button("", Skin.GetStyle("NodePortEmpty"));
            GUILayout.Button("", Skin.GetStyle("NodePortEmpty"));
            GUILayout.Button("", Skin.GetStyle("NodePortEmpty"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }
    }
}
