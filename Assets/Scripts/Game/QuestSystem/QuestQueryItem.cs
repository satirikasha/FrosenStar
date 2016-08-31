using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Heist.Quests;

[ExecuteInEditMode]
public class QuestQueryItem : MonoBehaviour {

    private static List<QuestQueryItem> _RegistredItems = new List<QuestQueryItem>();

    public IObjectStateProvider QueryItem { get; private set; }

    public string QueryItemID;

    public Transform MarkerOverridePosition;

    void OnEnable() {
        QueryItem = this.GetComponent<IObjectStateProvider>();
        _RegistredItems.Add(this);
    }

    void OnDisable() {
        _RegistredItems.Remove(this);
    }

    public static List<string> GetItemIDs() {
        return _RegistredItems
            .Where(_ => !String.IsNullOrEmpty(_.QueryItemID))
            .Select(_ => _.QueryItemID)
            .Distinct()
            .ToList();
    }

    public static QuestQueryItem GetItem(string id) {
        return _RegistredItems.FirstOrDefault(_ => _.QueryItemID == id);
    }
}
