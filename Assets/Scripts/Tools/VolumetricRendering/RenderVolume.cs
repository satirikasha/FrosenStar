﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VolumetricRendering {


    public class RenderVolume {

        private VolumeVertex[] _Vertices;
        private VolumeEdge[] _Edges;

        private static readonly Vector3[] Increments = new Vector3[] { Vector3.right, Vector3.up, Vector3.forward };

        public RenderVolume() {
            _Vertices = new VolumeVertex[8];
            _Edges = new VolumeEdge[12];

            for (int i = 0; i < 8; i++) {
                _Vertices[i] = new VolumeVertex();
            }
            for (int i = 0; i < 12; i++) {
                _Edges[i] = new VolumeEdge();
            }
        }

        public void RefreshVolume(Bounds volume, Transform transform) {
            //_Vertices[0].Position = Vector3.zero;
            //for (int i = 1; i <= 3; i++) {
            //    _Vertices[i] 
            //    for (int j = 1; j <= 3; j++) {

            //    }
            //}

            for (int i = 0; i < 8; i++) {
                _Vertices[i] = new VolumeVertex() {
                    Position = transform.TransformPoint(volume.center + Vector3.Scale(volume.extents, new Vector3(i % 2 * 2 - 1, (i / 2) % 2 * 2 - 1, (i / 4) % 2 * 2 - 1)))
                };
            }

            foreach (var vertex in _Vertices) {
                Debug.Log(vertex.Position);
            }
        }

        private class VolumeVertex {
            public Vector3 Position;
            public VolumeEdge[] Edges = new VolumeEdge[3];
            public VolumeEdge[] OrderedEdges = new VolumeEdge[3];
        }

        private class VolumeEdge {
            public VolumeVertex[] Vertices = new VolumeVertex[2];
            public VolumeVertex TargetVertex;
        }
    }
}