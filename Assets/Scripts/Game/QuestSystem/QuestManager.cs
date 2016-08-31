using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Heist.Quests;
using NodeCanvas.Framework;

[ExecuteInEditMode]
public class QuestManager : MonoBehaviour {

    public static QuestManager Instance { get; private set; }

    public event Action OnQuestFailed;
    public event Action OnQuestComplete;

    private bool _QuestCompleteState;
    private bool _QuestFailedState;

    public QuestTree QuestTree;

    void Awake() {
        Instance = this;
    }

    void Start() {
        QuestTree.StartGraph();
    }

    // Update is called once per frame
    void Update() {
        if (Application.isPlaying && QuestTree) {
            QuestTree.UpdateGraph();
            if(!_QuestCompleteState && QuestTree.QuestComplete) {
                if (OnQuestComplete != null)
                    OnQuestComplete();
                _QuestCompleteState = true;
            }
            if (!_QuestFailedState && QuestTree.QuestFailed) {
                if (OnQuestFailed != null)
                    OnQuestFailed();
                _QuestFailedState = true;
            }
        }
    }

    public TaskState[] GetTasks() {
        return QuestTree.Tasks;
    }
}
