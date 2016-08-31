#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using System;
using NodeCanvas;
using NodeCanvas.StateMachines;

namespace Heist.Quests {

    public interface IState {

        //The name of the state
        string name { get; }

        //The tag of the state
        string tag { get; }

        //The elapsed time of the state
        float elapsedTime { get; }

        //The QuestTree this state belongs to
        QuestTree QuestTree { get; }

        //An array of the state's transition connections
        QuestTreeConnection[] GetTransitions();

        //Evaluates the state's transitions and returns true if a transition has been performed
        bool CheckTransitions();
    }

    /// <summary>
    /// Super base class for QuestTree state nodes that live within an QuestTree Graph.
    /// </summary>
    abstract public class QuestTreeState : Node, IState {

        private float _elapsedTime;

        public override int maxInConnections { get { return -1; } }
        public override int maxOutConnections { get { return -1; } }
        sealed public override System.Type outConnectionType { get { return typeof(QuestTreeConnection); } }
        public override bool allowAsPrime { get { return true; } }
        sealed public override bool showCommentsBottom { get { return true; } }

        public float elapsedTime {
            get { return _elapsedTime; }
            private set { _elapsedTime = value; }
        }

        public QuestTree QuestTree {
            get {
                return QuestManager.Instance.QuestTree;
            }
        }

        ///Returns all transitions of the state
        public QuestTreeConnection[] GetTransitions() {
            return outConnections.Cast<QuestTreeConnection>().ToArray();
        }

        ///Declares that the state has finished
        public void Finish() {
            Finish(true);
        }

        ///Declares that the state has finished
        public void Finish(bool inSuccess) {
            status = inSuccess ? Status.Success : Status.Failure;
        }

        sealed public override void OnGraphStarted() {
            OnInit();
        }

        sealed public override void OnGraphStoped() {
            status = Status.Resting;
        }

        sealed public override void OnGraphPaused() {
            if (status == Status.Running)
                OnPause();
        }

        //OnEnter...
        sealed protected override Status OnExecute(Component agent, IBlackboard bb) {

            if (status == Status.Resting || status == Status.Running) {
                status = Status.Running;

                for (int i = 0; i < outConnections.Count; i++) {
                    if (((QuestTreeConnection)outConnections[i]).condition != null) {
                        ((QuestTreeConnection)outConnections[i]).condition.Enable(agent, bb);
                    }
                }

                OnEnter();
            }

            return status;
        }

        //OnUpdate...
        public void Update() {

            elapsedTime += Time.deltaTime;

            if (status == Status.Running)
                OnUpdate();
        }

        ///Returns true if a transitions was valid and thus made
        public bool CheckTransitions() {

        //    for (var i = 0; i < outConnections.Count; i++) {

        //        var connection = (QuestTreeConnection)outConnections[i];
        //        var condition = connection.condition;

        //        if (!connection.isActive)
        //            continue;

        //        if ((condition != null && condition.CheckCondition(graphAgent, graphBlackboard)) || (condition == null && status != Status.Running)) {
        //            QuestTree.EnterState((QuestTreeState)connection.targetNode);
        //            connection.connectionStatus = Status.Success; //purely for editor feedback
        //            return true;
        //        }

        //        connection.connectionStatus = Status.Failure;
        //    }

            return false;
        }

        //OnExit...
        sealed protected override void OnReset() {
            status = Status.Resting;
            elapsedTime = 0;

            OnExit();
        }

        public void OnEnterState() {
            status = Status.Running;
        }

        public void OnLeaveState() {
            status = Status.Success;
        }

        //Converted
        virtual protected void OnInit() { }
        virtual protected void OnEnter() { }
        virtual protected void OnUpdate() { }
        virtual protected void OnExit() { }
        virtual protected void OnPause() { }
        //




        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        private static GUIPort clickedPort { get; set; }

        private static int dragDropMisses;

        class GUIPort {

            public readonly QuestTreeState parent;
            public readonly Vector2 pos;

            public GUIPort(QuestTreeState parent, Vector2 pos) {
                this.parent = parent;
                this.pos = pos;
            }
        }

        sealed public override void DrawNodeConnections(Vector2 canvasMousePos, float zoomFactor) {

            var e = Event.current;

            var portRectBottom = new Rect(0, 0, 20, 20);

            //if (maxOutConnections == 0) {
            //    if (clickedPort != null && e.type == EventType.MouseUp) {
            //        dragDropMisses++;
            //        if (dragDropMisses == graph.allNodes.Count) {
            //            Debug.Log("Exec");
            //            //clickedPort = null;
            //        }
            //    }
            //}
            //else {

                portRectBottom.center = new Vector2(nodeRect.center.x, nodeRect.yMax + 11);

                EditorGUIUtility.AddCursorRect(portRectBottom, MouseCursor.ArrowPlus);

                GUI.color = new Color(1, 1, 1, 0.3f);
                GUI.Box(portRectBottom, "", "arrowBottom");
                GUI.color = Color.white;

                if (e.type == EventType.MouseDown && e.button == 0) {

                    if (portRectBottom.Contains(e.mousePosition)) {
                        clickedPort = new GUIPort(this, portRectBottom.center);
                        dragDropMisses = 0;
                        e.Use();
                    }
                }

                if (clickedPort != null && clickedPort.parent == this)
                    Handles.DrawBezier(clickedPort.pos, e.mousePosition, clickedPort.pos, e.mousePosition, new Color(0.5f, 0.5f, 0.8f, 0.8f), null, 2);
            //}

            if (clickedPort != null && e.type == EventType.MouseUp && e.button == 0) {

                if (nodeRect.Contains(e.mousePosition)) {
                    foreach (QuestTreeConnection connection in inConnections) {
                        if (connection.sourceNode == clickedPort.parent) {
                            Debug.LogWarning("State is already connected to target state. Consider using a 'ConditionList' on the existing transition if you want to check multiple conditions");
                            clickedPort = null;
                            e.Use();
                            return;
                        }
                    }

                    graph.ConnectNodes(clickedPort.parent, this);
                    clickedPort = null;
                    e.Use();

                }
                else {

                    dragDropMisses++;

                    if (dragDropMisses == graph.allNodes.Count && clickedPort != null) {
                        var source = clickedPort.parent;
                        var pos = Event.current.mousePosition;
                        var menu = new GenericMenu();
                        clickedPort = null;

                        menu.AddItem(new GUIContent("Add Action State"), false, () => {
                            var newState = graph.AddNode<ActionState>(pos);
                            graph.ConnectNodes(source, newState);
                        });

                        //PostGUI cause of zoom factors
                        Graph.PostGUI += () => { menu.ShowAsContext(); };
                        Event.current.Use();
                        e.Use();
                    }
                }
            }

            for (var i = 0; i < outConnections.Count; i++) {

                var connection = outConnections[i] as QuestTreeConnection;
                var targetState = connection.targetNode as QuestTreeState;
                if (targetState == null) {
                    continue;
                }
                var targetPos = targetState.GetConnectedInPortPosition(connection);
                var sourcePos = portRectBottom.center;

                connection.DrawConnectionGUI(sourcePos, targetPos);
            }
        }

        Vector2 GetConnectedInPortPosition(Connection connection) {
            return new Vector2(nodeRect.center.x, nodeRect.yMin);
        }

        protected override void OnNodeGUI() {
            if (inIconMode)
                GUILayout.Label("<i>" + name + "</i>");
        }

        protected override void OnNodeInspectorGUI() {
            ShowBaseQuestTreeInspectorGUI();
            DrawDefaultInspector();
        }

        protected override GenericMenu OnContextMenu(GenericMenu menu) {

            if (allowAsPrime) {
                if (Application.isPlaying) {
                    menu.AddItem(new GUIContent("Enter State"), false, delegate { QuestTree.EnterState(this); });
                }
                else {
                    menu.AddDisabledItem(new GUIContent("Enter State"));
                }
            }
            return menu;
        }

        protected void ShowBaseQuestTreeInspectorGUI() {

            EditorUtils.CoolLabel("Transitions");

            if (outConnections.Count == 0) {
                GUI.backgroundColor = new Color(1, 1, 1, 0.5f);
                GUILayout.BeginHorizontal("box");
                GUILayout.Label("No Transitions");
                GUILayout.EndHorizontal();
                GUI.backgroundColor = Color.white;
            }

            var onFinishExists = false;
            EditorUtils.ReorderableList(outConnections, delegate (int i) {

                var connection = (QuestTreeConnection)outConnections[i];
                GUI.backgroundColor = new Color(1, 1, 1, 0.5f);
                GUILayout.BeginHorizontal("box");
                if (connection.condition != null) {
                    GUILayout.Label(connection.condition.summaryInfo, GUILayout.MinWidth(0), GUILayout.ExpandWidth(true));
                }
                else {
                    GUILayout.Label("OnFinish" + (onFinishExists ? " (exists)" : ""), GUILayout.MinWidth(0), GUILayout.ExpandWidth(true));
                    onFinishExists = true;
                }

                GUILayout.FlexibleSpace();
                GUILayout.Label("--> '" + connection.targetNode.name + "'");
                if (GUILayout.Button("►"))
                    Graph.currentSelection = connection;

                GUILayout.EndHorizontal();
                GUI.backgroundColor = Color.white;
            });

            EditorUtils.BoldSeparator();
        }

#endif
    }
}