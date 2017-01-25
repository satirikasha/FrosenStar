using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeGraph {


    [Serializable]
    public class Edge {

#if UNITY_EDITOR
        private Editor.EdgeView _View;
        public Editor.EdgeView GetView() {
            if (_View == null)
                _View = Editor.EdgeView.Instantiate<Editor.EdgeView>(this);

            return _View;
        }
#endif
    }
}
 