using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class QuestTask {

    public string Name = "Task name";
    public bool Mandatory = false;
    public bool Visible = false;
    public bool IndependentComplete = false;

    public QuestCondition OpenCondition = new QuestCondition();
    public QuestCondition CompleteCondition = new QuestCondition();
    public QuestCondition FailCondition = new QuestCondition();
    public QuestCondition CancelCondition = new QuestCondition();
}

public enum QuestStatus {
    NotActive,
    InProgress,
    Complete,
    Canceled,
    Failed
}
