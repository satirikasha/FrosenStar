using UnityEngine;
using System.Collections;

namespace Tools.Damage {


    public class Damage {
        public float Ammount;
        public DamageType Type;
        public GameObject Instigator;
        public GameObject Source;
    }

    public enum DamageType {
        Physical,
        Impact,
        Energy
    }
}
