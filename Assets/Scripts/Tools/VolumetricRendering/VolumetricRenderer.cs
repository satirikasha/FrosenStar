using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VolumetricRendering {

    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class VolumetricRenderer : MonoBehaviour {

        public Bounds Volume = new Bounds(Vector3.zero, Vector3.one);

        private Mesh _Mesh;
        private MeshFilter _MeshFilter;
        private MeshRenderer _MeshRenderer;

        void Awake() {
            _Mesh = new Mesh();
            _Mesh.MarkDynamic();
            _MeshFilter = this.GetComponent<MeshFilter>();
            _MeshRenderer = this.GetComponent<MeshRenderer>();
        }

        void OnWillRenderObject() {
        }

        private void GenerateSlices() {

        }



#if UNITY_EDITOR
        void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            var urf = this.transform.TransformPoint(Volume.center + Vector3.Scale(Volume.extents, new Vector3(1, 1, 1)));
            var drf = this.transform.TransformPoint(Volume.center + Vector3.Scale(Volume.extents, new Vector3(-1, 1, 1)));
            var ulf = this.transform.TransformPoint(Volume.center + Vector3.Scale(Volume.extents, new Vector3(1, -1, 1)));
            var dlf = this.transform.TransformPoint(Volume.center + Vector3.Scale(Volume.extents, new Vector3(-1, -1, 1)));
            var urb = this.transform.TransformPoint(Volume.center + Vector3.Scale(Volume.extents, new Vector3(1, 1, -1)));
            var drb = this.transform.TransformPoint(Volume.center + Vector3.Scale(Volume.extents, new Vector3(-1, 1, -1)));
            var ulb = this.transform.TransformPoint(Volume.center + Vector3.Scale(Volume.extents, new Vector3(1, -1, -1)));
            var dlb = this.transform.TransformPoint(Volume.center + Vector3.Scale(Volume.extents, new Vector3(-1, -1, -1)));

            Gizmos.DrawLine(urf, drf);
            Gizmos.DrawLine(urf, ulf);
            Gizmos.DrawLine(urf, urb);
            Gizmos.DrawLine(drf, dlf);
            Gizmos.DrawLine(drf, drb);
            Gizmos.DrawLine(ulf, ulb);
            Gizmos.DrawLine(ulf, dlf);
            Gizmos.DrawLine(urb, drb);
            Gizmos.DrawLine(urb, ulb);
            Gizmos.DrawLine(dlb, ulb);
            Gizmos.DrawLine(dlb, drb);
            Gizmos.DrawLine(dlb, dlf);
        }
#endif
    }
}