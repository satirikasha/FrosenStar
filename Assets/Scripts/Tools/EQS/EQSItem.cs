using UnityEngine;
using System.Collections;

namespace Tools.EQS {


    public class EQSItem : MonoBehaviour {

        public bool Visible { get; private set; }

        public Vector3 Delta { get; set; }

        void OnEnable() {
            EQS.RegisterItem(this);
        }

        void OnDisable() {
            EQS.UnregisterItem(this);
        }

        public void OnBecameVisible() {
            Visible = true;
        }

        public void OnBecameInvisible() {
            Visible = false;
        }
    }
}
