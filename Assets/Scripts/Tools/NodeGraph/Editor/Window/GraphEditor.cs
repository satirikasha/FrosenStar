using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace NodeGraph.Editor {

    public class GraphEditor : EditorWindow {

        public Graph SelectedGraph { get; private set; }

        private GraphView GraphView {
            get {
                if (SelectedGraph == null) {
                    _GraphView = null;
                }
                else {
                    if (_GraphView == null || _GraphView.Graph != SelectedGraph)
                        _GraphView = GraphView.Instantiate<GraphView>(SelectedGraph);
                }

                return _GraphView;
            }
        }
        private GraphView _GraphView;

        void OnEnable() {
            var canvasIcon = (Texture)Resources.Load("CanvasIcon");
            titleContent = new GUIContent("Graph", canvasIcon);
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
            return GetWindow<GraphEditor>(new System.Type[] { typeof(SceneView) });
        }

        void OnSelectionChanged() {
            DetectGraphFromSelection();
            Repaint();
        }

        void OnInspectorUpdate() {
            Repaint();
        }

        void OnGUI() {

            if (EditorApplication.isCompiling) {
                return;
            }

            if (GraphView != null) {
                GraphView.BeginGraphGUI(this, new Rect(Vector2.zero, position.size));
                GraphView.OnGraphGUI();
                GraphView.EndGraphGUI();
            }
        }

        private void Init() {

        }

        private void DetectGraphFromSelection() {
            if (Selection.activeObject is Graph && EditorUtility.IsPersistent(Selection.activeObject)) {
                SelectedGraph = (Graph)Selection.activeObject;
            }
        }
    }
}