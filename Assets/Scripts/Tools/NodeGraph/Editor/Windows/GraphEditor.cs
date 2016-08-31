#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


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

        private Vector2 _ScrollPosition;
        private Rect _GraphRect;
        private Rect _GraphExtents;

        void OnEnable() {
#if UNITY_5
            var canvasIcon = (Texture)Resources.Load("CanvasIcon");
            titleContent = new GUIContent("Canvas", canvasIcon);
#else
	        title = "Canvas";
#endif

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

            //if (EditorApplication.isCompiling) {
            //    ShowNotification(new GUIContent("Compiling Please Wait..."));
            //    return;
            //}

            GUIStyle background = "flow background";
            _GraphRect = new Rect(0, 0, position.width, position.height);
            _GraphExtents = new Rect(0, 0, position.width * 2, position.height * 2);
            if (Event.current.type == EventType.Repaint) {
                background.Draw(_GraphRect, false, false, false, false);
            }


            _ScrollPosition = GUI.BeginScrollView(_GraphRect, _ScrollPosition, _GraphExtents, GUIStyle.none, GUIStyle.none);
            DrawGrid();
            GUI.EndScrollView();
        }

        public void BeginGraphGUI(EditorWindow host, Rect position) {
            //this.m_GraphClientArea = position;
            //this.m_Host = host;
            GUIStyle background = "flow background";
            if (Event.current.type == EventType.Repaint) {
                background.Draw(position, false, false, false, false);
            }
            //this.m_ScrollPosition = GUI.BeginScrollView(position, this.m_ScrollPosition, this.m_Graph.graphExtents, GUIStyle.none, GUIStyle.none);
            this.DrawGrid();
        }

        public void EndGraphGUI() {
            //this.UpdateGraphExtents();
            //this.UpdateScrollPosition();
            //this.DragGraph();
            //GUI.EndScrollView();
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
                Handles.DrawLine(new Vector2(num, _GraphExtents.xMin), new Vector2(num, _GraphExtents.xMax));
            }

            for (float num2 = _GraphExtents.yMin - _GraphExtents.yMin % gridSize; num2 < _GraphExtents.yMax; num2 += gridSize) {
                Handles.DrawLine(new Vector2(_GraphExtents.yMin, num2), new Vector2(_GraphExtents.yMax, num2));
            }
        }
    }
}

#endif