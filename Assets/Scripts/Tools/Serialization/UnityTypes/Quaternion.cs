using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System;

namespace Tools.Serialization {

    public class QuaternionSurrogate : SerializationSurrogate<Quaternion> {

        public override void GetObjectData(Quaternion obj, SerializationInfo info, StreamingContext context) {
            info.AddValue("x", obj.x);
            info.AddValue("y", obj.y);
            info.AddValue("z", obj.z);
            info.AddValue("w", obj.w);
        }

        public override Quaternion SetObjectData(Quaternion obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) {
            obj.x = info.GetFloat("x");
            obj.y = info.GetFloat("y");
            obj.z = info.GetFloat("z");
            obj.w = info.GetFloat("w");
            return obj;
        }
    }
}
