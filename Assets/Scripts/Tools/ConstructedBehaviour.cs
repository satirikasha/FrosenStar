using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ConstructedBehaviour : MonoBehaviour {

    [HideInInspector]
    public bool RequiresReconstruction = true;

    public virtual void Construct() {
        Debug.Log("Construct");
        RequiresReconstruction = false;
    }

    public virtual void OnValidate() {
        if (RequiresReconstruction)
            Construct();
    }
}
