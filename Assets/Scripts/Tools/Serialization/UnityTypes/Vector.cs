using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System;

namespace Tools.Serialization {

    public class Vector2Surrogate : SerializationSurrogate<Vector2> {

        public override void GetObjectData(Vector2 obj, SerializationInfo info, StreamingContext context) {
            info.AddValue("x", obj.x);
            info.AddValue("y", obj.y);
        }

        public override Vector2 SetObjectData(Vector2 obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) {
            obj.x = (float)info.GetValue("x", typeof(float));
            obj.y = (float)info.GetValue("y", typeof(float));
            return obj;
        }
    }


    public class Vector3Surrogate : SerializationSurrogate<Vector3> {

        public override void GetObjectData(Vector3 obj, SerializationInfo info, StreamingContext context) {
            info.AddValue("x", obj.x);
            info.AddValue("y", obj.y);
            info.AddValue("z", obj.z);
        }

        public override Vector3 SetObjectData(Vector3 obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) {
            obj.x = (float)info.GetValue("x", typeof(float));
            obj.y = (float)info.GetValue("y", typeof(float));
            obj.z = (float)info.GetValue("z", typeof(float));
            return obj;
        }
    }


    public class Vector4Surrogate : SerializationSurrogate<Vector4> {

        public override void GetObjectData(Vector4 obj, SerializationInfo info, StreamingContext context) {
            info.AddValue("x", obj.x);
            info.AddValue("y", obj.y);
            info.AddValue("z", obj.z);
            info.AddValue("w", obj.w);
        }

        public override Vector4 SetObjectData(Vector4 obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) {
            obj.x = (float)info.GetValue("x", typeof(float));
            obj.y = (float)info.GetValue("y", typeof(float));
            obj.z = (float)info.GetValue("z", typeof(float));
            obj.w = (float)info.GetValue("w", typeof(float));
            return obj;
        }
    }
}
