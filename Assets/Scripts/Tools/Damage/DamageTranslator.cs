using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.Damage {


    public class DamageTranslator : MonoBehaviour, IDamagable {

        public GameObject Target;

        private IDamagable _Target;

        void Awake() {
            _Target = Target.GetComponent<IDamagable>();
        }

        public void ApplyDamage(Damage damage) {
            _Target.ApplyDamage(damage);
        }
    }
}