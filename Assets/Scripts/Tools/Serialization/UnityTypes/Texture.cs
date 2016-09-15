using System.Collections;
using System.Runtime.Serialization;
using System;
using UnityEngine;

namespace Tools.Serialization {

    public class TextureSurrogate<T> : SerializationSurrogate<T> where T : Texture {

        public override void GetObjectData(T obj, SerializationInfo info, StreamingContext context) {

            int id;

            id = ObjectRegistry.GetTextureID(obj);
            if (id != 0) {
                info.AddValue("id", id); return;
            }

            throw new Exception("Serialization of Texture failed");
        }


        public override T SetObjectData(T obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) {

            T tex;

            tex = (T)ObjectRegistry.GetTexture(info.GetInt32("id"));
            if(tex != null) {
                return tex;
            }

            throw new Exception("Deserialization of Texture failed");
        }
    }

    public class TextureSurrogate : TextureSurrogate<Texture> { }

    public class Texture2DSurrogate : TextureSurrogate<Texture2D> { }
}
