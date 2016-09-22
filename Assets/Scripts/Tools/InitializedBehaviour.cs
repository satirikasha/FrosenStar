using UnityEngine;
using System.Collections;

public class InitializedBehaviour : MonoBehaviour {

    public bool Initialized { get; private set; }

    public void Initialize() {
        if (!Initialized) {
            Init();
            Initialized = true;
        }
    }

    protected virtual void Init() {}
}
