using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class GizmoDrawer : SingletonBehaviour<GizmoDrawer> {

    private List<Action> Tasks = new List<Action>();

    void OnDrawGizmos() {
        Tasks.ForEach(_ => _());
        Tasks.Clear();
    }

    public static void AddTask(Action task) {
        Instance.Tasks.Add(task);
    }
}
