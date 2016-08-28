using UnityEngine;
using System.Collections;
using Tools;

namespace Tools.EQS {


    public class EQSManager : SingletonBehaviour<EQSManager> {

        private const int Capacity = 1000;
        private const float ItemRadius = 0.5f;

        private CullingGroup _CullingGroup;
        private BoundingSphere[] _BoundingSpheres;

        public override void Awake() {
            base.Awake();
            CullingGroup _CullingGroup = new CullingGroup();
            _CullingGroup.targetCamera = Camera.main;
            _BoundingSpheres = new BoundingSphere[Capacity];
            
        }

        public override void OnDestroy() {
            base.OnDestroy();
            _CullingGroup.Dispose();
            _CullingGroup = null;
        }
    }
}
