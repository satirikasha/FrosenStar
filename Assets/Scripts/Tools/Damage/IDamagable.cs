using UnityEngine;
using System.Collections;

namespace Tools.Damage {


    public interface IDamagable {
        void ApplyDamage(Damage damage);
    }
}
