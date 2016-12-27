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
        private Vector2 _ScrollPosition;
        private Rect _GraphRect;
        private Rect _GraphArea;
        private Rect _GraphExtents;
        private Rect _LastGraphExtents;

        void OnEnable() {
            var canvasIcon = (Texture)Resources.Load("CanvasIcon");
            titleContent = new GUIContent("Canvas", canvasIcon);
            wantsMouseMove = true;
            minSize = new Vector2(700, 300);
            Selection.selectionChanged += OnSelectionChanged;
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

            //Debug Area
            _GraphRect = new Rect(0, 0, position.width, position.height);
            _GraphExtents = new Rect(0, 0, position.width * 1.5f, position.height * 1.5f);
            //End Debug

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

            _ScrollPosition = GUI.BeginScrollView(position, _ScrollPosition, _GraphExtents, GUIStyle.none, GUIStyle.none);
            DrawGrid();
        }

        public virtual void OnGraphGUI() {
            //_Host.BeginWindows();
            //foreach (Node current in this.m_Graph.nodes) {
            //    Node n2 = current;
            //    bool on = this.selection.Contains(current);
            //    Styles.Color color = (!current.nodeIsInvalid) ? current.color : Styles.Color.Red;
            //    current.position = GUILayout.Window(current.GetInstanceID(), current.position, delegate {
            //        this.NodeGUI(n2);
            //    }, current.title, Styles.GetNodeStyle(current.style, color, on), new GUILayoutOption[]
            //    {
            //        GUILayout.Width(0f),
            //        GUILayout.Height(0f)
            //    });
            //}
            //_Host.EndWindows();
            //this.edgeGUI.DoEdges();
            //this.edgeGUI.DoDraggedEdge();
            //this.DragSelection(new Rect(-5000f, -5000f, 10000f, 10000f));
            //this.ShowContextMenu();
            //this.HandleMenuEvents();
        }

        public void EndGraphGUI() {
            //this.UpdateGraphExtents();
            UpdateScrollPosition();
            DragGraph();
            GUI.EndScrollView();
        }

        private void UpdateScrollPosition() {
            _ScrollPosition.x = _ScrollPosition.x + (_LastGraphExtents.x - _GraphExtents.x);
            _ScrollPosition.y = _ScrollPosition.y + (_LastGraphExtents.y - _GraphExtents.y);
            _LastGraphExtents = _GraphExtents;
            //if (this.m_CenterGraph && Event.current.type == EventType.Layout) {
            //    this.m_ScrollPosition = new Vector2(this.graph.graphExtents.width / 2f - this.m_Host.position.width / 2f, this.graph.graphExtents.height / 2f - this.m_Host.position.height / 2f);
            //    this.m_CenterGraph = false;
            //}
        }

        private void DragGraph() {
            if (Event.current.button == 2 && Event.current.type == EventType.MouseDrag) {
                _ScrollPosition -= Event.current.delta;
                Event.current.Use();
            }
        }

        private void DrawGrid() {
            if (Event.current.type != EventType.Repaint) {
                return;
            }

            Profiler.BeginSample("DrawGrid");
            this.DrawGridLines(12f, GridMinorColor);
            this.DrawGridLines(120f, GridMajorColor);
            Profiler.EndSample();
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

        private void TransformGraph() {

        }
    }
}

#endif