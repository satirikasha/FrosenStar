using UnityEngine;
using System.Collections;
using System;

public interface IObjectStateProvider {

    string[] GetObjectStates();
    bool CheckState(int state);

}
