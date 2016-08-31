using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class QuestCondition {

    public virtual bool Check() {
        return false;
    }

    public virtual void OnInspectorGUI() {

    }
}
