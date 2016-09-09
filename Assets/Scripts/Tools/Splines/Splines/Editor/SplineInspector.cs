using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Spline))]
public class SplineInspector : Editor {
    private bool showNodeArray = true;
    private int numTransformNodes;

    private Spline spline;
    private Spline.ControlNodeMode nMode;
    private Spline.RotationMode rMode;
    private Spline.TangentMode tMode;
    private Spline.InterpolationMode iMode;
    private Spline.UpdateMode uMode;

    private int accuarcy;

    private int deltaFrames;
    private float deltaSeconds;

    private float tension;
    private bool autoClose;

    private Vector3 tanUpVector;

    private const string errorNoChildren = "No transform nodes assigned to spline! Parent some gameobjects to the spline!";
    private const string errorNoNodes = "No transform nodes assigned to spline! Drag some gameobjects onto the spline inspector!";
    private const string errorNullNodes = "The spline references non-existing nodes! Check the transform array!";
    private const string errorBezier = "Bezier Splines are defined by 4 + x * 3 (4, 7, 10, ...) control points. They need at least 4 control points! Add some control points to your spline!";

    private bool errorNoChildrenShown = false;
    private bool errorNoNodesShown = false;
    private bool errorNullNodesShown = false;

    public override void OnInspectorGUI() {
        spline = (Spline)target;

        EditorGUILayout.BeginVertical();
        uMode = (Spline.UpdateMode)EditorGUILayout.EnumPopup("   Update Mode", spline.updateMode);

        if (uMode == Spline.UpdateMode.EveryXFrames) {
            deltaFrames = EditorGUILayout.IntField("   Delta Frames", spline.deltaFrames);
            EditorGUILayout.Space();
        }
        else if (uMode == Spline.UpdateMode.EveryXSeconds) {
            deltaSeconds = EditorGUILayout.FloatField("   Delta Seconds", spline.deltaSeconds);
            EditorGUILayout.Space();
        }

        nMode = (Spline.ControlNodeMode)EditorGUILayout.EnumPopup("   Control Nodes", spline.nodeMode);
        iMode = (Spline.InterpolationMode)EditorGUILayout.EnumPopup("   Interpolation", spline.interpolationMode);

        if (iMode == Spline.InterpolationMode.Hermite)
            tMode = (Spline.TangentMode)EditorGUILayout.EnumPopup("   Tangent Mode", spline.tangentMode);

        rMode = (Spline.RotationMode)EditorGUILayout.EnumPopup("   Rotation Mode", spline.rotationMode);

        EditorGUILayout.Space();

        accuarcy = Mathf.Clamp(EditorGUILayout.IntField("   Accuracy", spline.interpolationAccuracy), 1, 20);

        EditorGUILayout.Space();

        if (iMode == Spline.InterpolationMode.Hermite)
            tension = EditorGUILayout.FloatField("   Spline Tension", spline.tension);

        if (rMode == Spline.RotationMode.Tangent) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("   Up-Vector");
            tanUpVector.x = EditorGUILayout.FloatField(spline.tanUpVector.x, GUILayout.MinWidth(10));
            tanUpVector.y = EditorGUILayout.FloatField(spline.tanUpVector.y, GUILayout.MinWidth(10));
            tanUpVector.z = EditorGUILayout.FloatField(spline.tanUpVector.z, GUILayout.MinWidth(10));
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        if (iMode != Spline.InterpolationMode.Bezier)
            autoClose = EditorGUILayout.Toggle("   Auto Close", spline.autoClose);

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        bool isAddNodePressed = GUILayout.Button("Add Spline Node", GUILayout.MinWidth(250f));
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();

        bool isAddNodeControllersPressed = false;

        if (spline.nodeMode == Spline.ControlNodeMode.UseChildren) {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            isAddNodeControllersPressed = GUILayout.Button("Add CtrlNode-Component To Children", GUILayout.MinWidth(250f));
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //Draw Array-GUI
        if (nMode == Spline.ControlNodeMode.UseArray) {
            showNodeArray = EditorGUILayout.Foldout(showNodeArray, " Spline Nodes");

            if (showNodeArray) {
                numTransformNodes = EditorGUILayout.IntField("      Size", spline.splineNodesTransform.Length);

                for (int i = 0; i < spline.splineNodesTransform.Length; i++)
                    spline.splineNodesTransform[i] = (Transform)EditorGUILayout.ObjectField("      Element " + i.ToString(), spline.splineNodesTransform[i], typeof(Transform), true);
            }

            DragAndDrop.activeControlID = 0;
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (Event.current.type == EventType.DragPerform && DragAndDrop.objectReferences[0].GetType() == typeof(GameObject)) {
                DragAndDrop.AcceptDrag();

                Undo.RegisterUndo(target, "Add Node To " + spline.name);

                AddTransformNode(((GameObject)DragAndDrop.objectReferences[0]).transform);

                UpdateSplineComponents();

                EditorUtility.SetDirty(target);
            }
        }




        EditorGUILayout.Space();

        EditorGUILayout.EndVertical();

        if (isAddNodePressed)
            AddSplineNode();

        if (isAddNodeControllersPressed)
            AddCtrlCmpntToChildren();

        //Apply changes
        if (GUI.changed) {
            Undo.RegisterUndo(target, "Change Spline Settings");

            spline.updateMode = uMode;
            spline.nodeMode = nMode;
            spline.rotationMode = rMode;
            spline.tangentMode = tMode;
            spline.interpolationMode = iMode;

            spline.interpolationAccuracy = accuarcy;

            if (uMode == Spline.UpdateMode.EveryXFrames)
                spline.deltaFrames = deltaFrames;
            else if (uMode == Spline.UpdateMode.EveryXSeconds)
                spline.deltaSeconds = deltaSeconds;

            if (iMode == Spline.InterpolationMode.Hermite)
                spline.tension = tension;

            if (iMode != Spline.InterpolationMode.Bezier)
                spline.autoClose = autoClose;

            if (rMode == Spline.RotationMode.Tangent)
                spline.tanUpVector = tanUpVector;

            //check for errors
            if (nMode == Spline.ControlNodeMode.UseChildren) {
                if (spline.transform.childCount == 0) {
                    if (!errorNoChildrenShown)
                        Debug.LogWarning(errorNoChildren, spline.gameObject);

                    errorNoChildrenShown = true;
                }
                else {
                    errorNoChildrenShown = false;
                }
            }
            else {
                if (numTransformNodes == 0) {
                    if (!errorNoNodesShown)
                        Debug.LogWarning(errorNoNodes, spline.gameObject);
                    errorNoNodesShown = true;
                }
                else {
                    errorNoNodesShown = false;
                }
            }

            if (nMode == Spline.ControlNodeMode.UseArray && showNodeArray) {
                ResizeTransformNodeArray();

                bool nullReference = false;

                //Check for null references and inform the user if any have been found
                foreach (Transform item in spline.splineNodesTransform)
                    if (item == null)
                        nullReference = true;

                if (nullReference) {
                    if (!errorNullNodesShown)
                        Debug.LogWarning(errorNullNodes, spline.gameObject);
                    errorNullNodesShown = true;
                }
                else {
                    errorNullNodesShown = false;
                }
            }

            UpdateSplineComponents();

            EditorUtility.SetDirty(target);
        }
    }

    private void AddSplineNode() {
        Undo.RegisterSceneUndo("Add Node To " + spline.name);

        GameObject gObject = new GameObject();

        Transform tmpTransform = spline.transform;

        Transform[] splineNodes = spline.SplineNodeTransforms;

        if (splineNodes != null)
            if (splineNodes.Length > 0)
                if (splineNodes[splineNodes.Length - 1] != null)
                    tmpTransform = splineNodes[splineNodes.Length - 1];

        gObject.transform.parent = spline.gameObject.transform;
        gObject.transform.localPosition = tmpTransform.localPosition + spline.transform.InverseTransformDirection(tmpTransform.forward);
        gObject.transform.localRotation = Quaternion.identity;
        gObject.transform.localScale = Vector3.one;

        gObject.name = GetNodeName(spline.gameObject.transform.GetChildCount());

        if (nMode == Spline.ControlNodeMode.UseChildren)
            gObject.AddComponent(typeof(SplineControlNode));

        if (nMode == Spline.ControlNodeMode.UseArray)
            AddTransformNode(gObject.transform);

        EditorUtility.SetDirty(target);
    }

    private void AddCtrlCmpntToChildren() {
        Undo.RegisterSceneUndo("Add CtrlNode-Component To " + spline.name + "'s children");

        Transform[] children = spline.GetComponentsInChildren<Transform>();

        foreach (Transform child in children) {
            if (child != spline.transform)
                if (child.gameObject.GetComponent<SplineControlNode>() == null)
                    child.gameObject.AddComponent<SplineControlNode>();
        }

        spline.UpdateSplineNodes();

        EditorUtility.SetDirty(target);
    }

    private void AddTransformNode(Transform newNode) {
        numTransformNodes++;

        Transform[] newTransform = new Transform[numTransformNodes];

        for (int i = 0; i < spline.splineNodesTransform.Length; i++)
            newTransform[i] = spline.splineNodesTransform[i];

        newTransform[numTransformNodes - 1] = newNode;

        spline.splineNodesTransform = newTransform;
    }

    private void ResizeTransformNodeArray() {
        Transform[] newTransform = new Transform[numTransformNodes];

        if (numTransformNodes <= spline.splineNodesTransform.Length) {
            for (int i = 0; i < numTransformNodes; i++)
                newTransform[i] = spline.splineNodesTransform[i];
        }
        else if (spline.splineNodesTransform.Length > 0) {
            for (int i = 0; i < spline.splineNodesTransform.Length; i++)
                newTransform[i] = spline.splineNodesTransform[i];

            for (int i = spline.splineNodesTransform.Length; i < numTransformNodes; i++)
                newTransform[i] = spline.splineNodesTransform[spline.splineNodesTransform.Length - 1];
        }

        spline.splineNodesTransform = newTransform;
    }

    public void OnSceneGUI() {
        Spline spline = (Spline)target;

        Transform[] splineNodes = spline.SplineNodeTransforms;

        if (splineNodes == null)
            return;

        Handles.lighting = true;

        GUIStyle gStyle = EditorStyles.miniTextField;

        gStyle.alignment = TextAnchor.MiddleCenter;

        if (Event.current.alt && Event.current.shift) {
            foreach (SplineSegment item in spline.SplineSegments)
                Handles.Label(spline.GetPositionOnSpline(item.ConvertSegmentToSplineParamter(.5f)) + Camera.current.transform.up, item.Length.ToString(), gStyle);

            Handles.Label(spline.transform.position + Camera.current.transform.up * 1.5f, "Length: " + spline.Length.ToString(), gStyle);

            return;
        }

        foreach (Transform node in splineNodes) {
            if (Event.current.alt && !Event.current.shift) {
                Handles.Label(node.position + Camera.current.transform.up, node.name, gStyle);
                continue;
            }

            switch (UnityEditor.Tools.current) {
                case Tool.None:
                    break;

                case Tool.Rotate:
                    CreateSnapshot();

                    Undo.SetSnapshotTarget((Object)node, "Rotate Spline Node: " + node.name);

                    Quaternion newRot;

                    if (UnityEditor.Tools.pivotRotation == PivotRotation.Local)
                        newRot = Handles.RotationHandle(node.rotation, node.position);
                    else
                        newRot = Handles.RotationHandle(node.localRotation, node.position);

                    Handles.color = Color.blue;
                    Handles.ArrowCap(0, node.position, Quaternion.LookRotation(node.forward), HandleUtility.GetHandleSize(node.position) * 0.5f);
                    Handles.color = Color.white;

                    if (!GUI.changed)
                        break;

                    if (UnityEditor.Tools.pivotRotation == PivotRotation.Global)
                        node.rotation = newRot;
                    else
                        node.localRotation = newRot;

                    EditorUtility.SetDirty(target);

                    break;

                case Tool.Scale:
                case Tool.Move:
                    CreateSnapshot();

                    Undo.SetSnapshotTarget((Object)node, "Move Spline Node: " + node.name);

                    Quaternion rotation;

                    if (UnityEditor.Tools.pivotRotation == PivotRotation.Local)
                        rotation = node.rotation;
                    else
                        rotation = node.localRotation;

                    Vector3 newPos = Handles.PositionHandle(node.position, rotation);

                    if (!GUI.changed)
                        break;

                    node.position = newPos;
                    EditorUtility.SetDirty(target);

                    break;
            }

        }

        if (GUI.changed)
            UpdateSplineComponents();

    }

    private void UpdateSplineComponents() {
        Spline spline = (Spline)target;

        spline.UpdateSplineNodes();

        if (spline.GetComponent<SplineMesh>()) {
            spline.GetComponent<SplineMesh>().UpdateMesh();
        }
    }

    private void CreateSnapshot() {
        if (Input.GetMouseButtonDown(0)) {
            Undo.CreateSnapshot();
            Undo.RegisterSnapshot();
        }
    }

    private static string GetNodeName(int num) {
        string res = "";

        for (int i = 1; i < 4; i++)
            if (num < Mathf.Pow(10, i))
                res += "0";

        return (res + num.ToString());
    }

    [MenuItem("GameObject/Create Other/Spline/Hermite")]
    static void CreateHermiteSpline() {
        Undo.RegisterSceneUndo("Create new spline");

        GameObject gObject = new GameObject();

        gObject.name = "New Spline";

        gObject.transform.localPosition = Vector3.zero;
        gObject.transform.localRotation = Quaternion.identity;
        gObject.transform.localScale = Vector3.one;

        Spline spline = gObject.AddComponent<Spline>();

        spline.interpolationMode = Spline.InterpolationMode.Hermite;
        spline.splineNodesTransform = new Transform[0];

        SetupChildren(gObject);

        Selection.activeGameObject = gObject;
    }

    [MenuItem("GameObject/Create Other/Spline/Bezier")]
    static void CreateBezierSpline() {
        Undo.RegisterSceneUndo("Create new spline");

        GameObject gObject = new GameObject();

        gObject.name = "New Spline";

        gObject.transform.localPosition = Vector3.zero;
        gObject.transform.localRotation = Quaternion.identity;
        gObject.transform.localScale = Vector3.one;

        Spline spline = gObject.AddComponent<Spline>();

        spline.interpolationMode = Spline.InterpolationMode.Bezier;
        spline.splineNodesTransform = new Transform[0];

        SetupChildren(gObject);

        Selection.activeGameObject = gObject;
    }

    [MenuItem("GameObject/Create Other/Spline/B-Spline")]
    static void CreateBSpline() {
        Undo.RegisterSceneUndo("Create new spline");

        GameObject gObject = new GameObject();

        gObject.name = "New Spline";

        gObject.transform.localPosition = Vector3.zero;
        gObject.transform.localRotation = Quaternion.identity;
        gObject.transform.localScale = Vector3.one;

        Spline spline = gObject.AddComponent<Spline>();

        spline.interpolationMode = Spline.InterpolationMode.BSpline;
        spline.splineNodesTransform = new Transform[0];

        SetupChildren(gObject);

        Selection.activeGameObject = gObject;
    }

    private static void SetupChildren(GameObject gObject) {
        GameObject[] childs = new GameObject[4];

        for (int i = 0; i < childs.Length; i++) {
            childs[i] = new GameObject();
            childs[i].name = GetNodeName(i);
            childs[i].transform.parent = gObject.transform;
            childs[i].transform.localPosition = -Vector3.forward * 1.5f + Vector3.forward * i;
            childs[i].transform.localRotation = Quaternion.identity;
            childs[i].transform.localScale = Vector3.one;

            childs[i].AddComponent<SplineControlNode>();
        }
    }
}

