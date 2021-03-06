﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeGraph {


    [Serializable]
    public class Node {
        public List<Slot> Slots = new List<Slot>();

        public Rect Position;

#if UNITY_EDITOR
        [NonSerialized]
        private Editor.NodeView _View;
        public Editor.NodeView GetView() {
            if (_View == null)
                _View = Editor.NodeView.Instantiate<Editor.NodeView>(this);
            return _View;
        }
#endif
    }
}
 