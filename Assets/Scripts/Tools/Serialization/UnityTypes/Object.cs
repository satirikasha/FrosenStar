using System.Collections;
using System.Runtime.Serialization;
using System;
using UnityEngine;

namespace Tools.Serialization {

    public class GameObjectSurrogate : SerializationSurrogate<GameObject> {

        public override void GetObjectData(GameObject obj, SerializationInfo info, StreamingContext context) {

            int id;

            id = PrefabRegistry.GetPrefabID(obj);
            if (id != 0) {
                info.AddValue("id", id); return;
            }

            throw new Exception("Serialization of GameObject failed");
        }


        public override GameObject SetObjectData(GameObject obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) {

            GameObject go;

            go = PrefabRegistry.GetPrefab(info.GetInt32("x"));
            if(go != null) {
                return go;
            }

            throw new Exception("Deserialization of GameObject failed");
        }
    }
}
