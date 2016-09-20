using UnityEngine;
using System.Collections;

public class DefaultPlayerStart : PlayerStart {

    public override void Awake() {
        //Shouldn't  register in the Instances list
        _Default = this;
    }

    public override void OnDestroy() {
        //Shouldn't  unregister in the Instances list
        if (_Default == this)
            _Default = null;
    }
}