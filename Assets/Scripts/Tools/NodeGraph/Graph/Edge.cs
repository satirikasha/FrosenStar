using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeGraph {


    [Serializable]
    public class Edge {

        public int SourceSlotID;
        public int TargetSlotID;

        public Slot SourceSlot;
        public Slot TargetSlot;

#if UNITY_EDITOR
        [NonSerialized]
        private Editor.EdgeView _View;
        public Editor.EdgeView GetView() {
            if (_View == null)
                _View = Editor.EdgeView.Instantiate<Editor.EdgeView>(this);

            return _View;
        }
#endif
    }
}
 