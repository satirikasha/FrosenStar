using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NodeGraph.Editor {


    public class GraphView : ScriptableObject {

        private static Color GridMinorColor {
            get {
                if (EditorGUIUtility.isProSkin) {
                    return GridMinorColorDark;
                }
                return GridMinorColorLight;
            }
        }

        private static Color GridMajorColor {
            get {
                if (EditorGUIUtility.isProSkin) {
                    return GridMajorColorDark;
                }
                return GridMajorColorLight;
            }
        }

        private static readonly Color GridMinorColorDark = new Color(0f, 0f, 0f, 0.18f);
        private static readonly Color GridMajorColorDark = new Color(0f, 0f, 0f, 0.28f);
        private static readonly Color GridMinorColorLight = new Color(0f, 0f, 0f, 0.1f);
        private static readonly Color GridMajorColorLight = new Color(0f, 0f, 0f, 0.15f);

        public Graph Graph { get; private set; }

        private EditorWindow _Host;
        private Vector2 _Position;
        private float _Scale = 1;
        private Rect _GraphRect;
        private Rect _GraphArea;
        private Rect _LastGraphExtents;
        private Matrix4x4 _MatrixGUI;

        public static T Instantiate<T>(Graph graph) where T : GraphView {
            var result = ScriptableObject.CreateInstance<T>();
            result.Graph = graph;
            result.hideFlags = HideFlags.HideAndDontSave;
            return result;
        }

        void OnEnable() {
            Debug.Log("Enabled : GraphView");
        }

        public void BeginGraphGUI(EditorWindow host, Rect position) {
            _Host = host;
            _GraphArea = position;
            //Debug
            Graph.GraphExtents = new Rect(0, 0, _GraphArea.width * 2.5f, _GraphArea.height * 2.5f);
            //End debug

            GUIStyle background = "flow background";
            if (Event.current.type == EventType.Repaint) {
                background.Draw(position, false, false, false, false);
            }

            _Position = GUI.BeginScrollView(position, _Position, Graph.GraphExtents, GUIStyle.none, GUIStyle.none);
            DrawGrid();
        }

        private Rect winRect = new Rect(300, 300, 120, 50);
        public virtual void OnGraphGUI() {
           
            _Host.BeginWindows();
            foreach (Node node in Graph.Nodes) {
                node.GetView().OnNodeGUI();
            }
            for (int i = 1; i < Graph.Nodes.Count; i++) {
                var prePos = Graph.Nodes[i - 1].Position.center;
                var curPos = Graph.Nodes[i].Position.center;
                Handles.DrawBezier(prePos, curPos, prePos + Vector2.up * 100, curPos + Vector2.down * 100, Color.gray, null, 3);
            }
            _Host.EndWindows();
            //this.edgeGUI.DoEdges();
            //this.edgeGUI.DoDraggedEdge();
            //this.DragSelection(new Rect(-5000f, -5000f, 10000f, 10000f));
            //this.ShowContextMenu();
            //this.HandleMenuEvents();
        }

        public void EndGraphGUI() {
            UpdateGraphExtents();
            DragGraph();
            ScaleGraph();

            //EndZoomArea();
            GUI.EndScrollView();
        }

        void BeginZoomArea(Rect position) {
            _MatrixGUI = GUI.matrix;
            var scale = Matrix4x4.Scale(new Vector3(_Scale, _Scale, 1));
            GUI.matrix *= scale;
            Handles.matrix = GUI.matrix;
        }

        void EndZoomArea() {
            GUI.matrix = _MatrixGUI;
            Handles.matrix = _MatrixGUI;
        }

        private void UpdateGraphExtents() {

        }

        private void DragGraph() {
            if (Event.current.button == 2 && Event.current.type == EventType.MouseDrag) {
                _Position -= Event.current.delta;
                Event.current.Use();
            }
        }

        private void ScaleGraph() {
            if (Event.current.type == EventType.ScrollWheel) {
                _Scale -= Event.current.delta.y / 25f;
                Event.current.Use();
            }
        }

        private void DrawGrid() {
            if (Event.current.type != EventType.Repaint) {
                return;
            }
            this.DrawGridLines(12f, GridMinorColor);
            this.DrawGridLines(120f, GridMajorColor);
        }

        private void DrawGridLines(float gridSize, Color gridColor) { 
            Handles.color = gridColor;

            for (float num = Graph.GraphExtents.xMin - Graph.GraphExtents.xMin % gridSize; num < Graph.GraphExtents.xMax; num += gridSize) {
                Handles.DrawLine(new Vector2(num, Graph.GraphExtents.yMin), new Vector2(num, Graph.GraphExtents.yMax));
            }

            for (float num2 = Graph.GraphExtents.yMin - Graph.GraphExtents.yMin % gridSize; num2 < Graph.GraphExtents.yMax; num2 += gridSize) {
                Handles.DrawLine(new Vector2(Graph.GraphExtents.xMin, num2), new Vector2(Graph.GraphExtents.xMax, num2));
            }
        }

    }
}
