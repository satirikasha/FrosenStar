#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace NodeGraph.Editor {

    public class GraphEditor : EditorWindow {

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

        private EditorWindow _Host;
        private Vector2 _Position;
        private float _Scale = 1;
        private Rect _GraphRect;
        private Rect _GraphArea;
        private Rect _GraphExtents;
        private Rect _LastGraphExtents;
        private Matrix4x4 _MatrixGUI;

        void OnEnable() {
            var canvasIcon = (Texture)Resources.Load("CanvasIcon");
            titleContent = new GUIContent("Canvas", canvasIcon);
            wantsMouseMove = true;
            minSize = new Vector2(700, 300);
            Selection.selectionChanged += OnSelectionChanged;

            //Debug Area
            _GraphRect = new Rect(0, 0, position.width, position.height);
            _GraphExtents = new Rect(0, 0, position.width * 1.5f, position.height * 1.5f);
            //End Debug

            Repaint();
        }

        void OnDisable() {
            Selection.selectionChanged -= OnSelectionChanged;
        }

        [MenuItem("Window/NodeGraph")]
        public static GraphEditor OpenWindow() {
            return GetWindow<GraphEditor>();
        }

        void OnSelectionChanged() {

        }

        void OnInspectorUpdate() {
            Repaint();
        }

        void OnGUI() {

            if (EditorApplication.isCompiling) {
                return;
            }

            BeginGraphGUI(this, _GraphRect);
            OnGraphGUI();
            EndGraphGUI();
        }

        public void BeginGraphGUI(EditorWindow host, Rect position) {
            _Host = host;
            _GraphArea = position;

            GUIStyle background = "flow background";
            if (Event.current.type == EventType.Repaint) {
                background.Draw(_GraphRect, false, false, false, false);
            }

            _Position = GUI.BeginScrollView(position, _Position, _GraphExtents, GUIStyle.none, GUIStyle.none);
            //BeginZoomArea(position);
            DrawGrid();
        }

        private Rect winRect = new Rect(300, 300, 120, 50);
        public virtual void OnGraphGUI() {
            _Host.BeginWindows();
            //foreach (Node current in this.m_Graph.nodes) {
            //    Node n2 = current;
            //    bool on = this.selection.Contains(current);
            //    Styles.Color color = (!current.nodeIsInvalid) ? current.color : Styles.Color.Red;
            /*current.position =*/
            winRect = GUILayout.Window(110, winRect, _ => {
                if (GUILayout.Button("Click me a lot, bitch!"))
                    Debug.Log("Got a click");
                GUI.DragWindow();
            }, "Node");
            //}
            _Host.EndWindows();
            //this.edgeGUI.DoEdges();
            //this.edgeGUI.DoDraggedEdge();
            //this.DragSelection(new Rect(-5000f, -5000f, 10000f, 10000f));
            //this.ShowContextMenu();
            //this.HandleMenuEvents();
        }

        public void EndGraphGUI() {
            //this.UpdateGraphExtents();
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

            for (float num = _GraphExtents.xMin - _GraphExtents.xMin % gridSize; num < _GraphExtents.xMax; num += gridSize) {
                Handles.DrawLine(new Vector2(num, _GraphExtents.yMin), new Vector2(num, _GraphExtents.yMax));
            }

            for (float num2 = _GraphExtents.yMin - _GraphExtents.yMin % gridSize; num2 < _GraphExtents.yMax; num2 += gridSize) {
                Handles.DrawLine(new Vector2(_GraphExtents.xMin, num2), new Vector2(_GraphExtents.xMax, num2));
            }
        }
    }
}

#endif