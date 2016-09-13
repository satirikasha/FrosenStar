using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Tools.Serialization {


    public abstract class SerializationSurrogate<T> : SerializationSurrogate {

        public override Type Type {
            get {
                return typeof(T);
            }
        }

        public override void GetObjectData(object obj, SerializationInfo info, StreamingContext context) {
            GetObjectData((T)obj, info, context);
        }

        public override object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) {
            return SetObjectData((T)obj, info, context, selector);
        }

        public abstract void GetObjectData(T obj, SerializationInfo info, StreamingContext context);

        public abstract T SetObjectData(T obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector);
    }


    public abstract class SerializationSurrogate : ISerializationSurrogate {

        public abstract Type Type { get; }

        public abstract void GetObjectData(object obj, SerializationInfo info, StreamingContext context);

        public abstract object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector);


        public static SurrogateSelector SurrogateSelector {
            get {
                if (_SurrogateSelector == null)
                    _SurrogateSelector = GatherSurrogates();
                return _SurrogateSelector;
            }
        }
        private static SurrogateSelector _SurrogateSelector;

        private static SurrogateSelector GatherSurrogates() {
            var surrogateTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t => t.IsClass && !t.IsGenericType && t.IsSubclassOf(typeof(SerializationSurrogate)));

            var surrogateInstances = surrogateTypes
                .Select(_ => Activator.CreateInstance(_))
                .OfType<SerializationSurrogate>();

            var surrogateSelector = new SurrogateSelector();

            foreach (var instance in surrogateInstances) {
                surrogateSelector.AddSurrogate(instance.Type, new StreamingContext(StreamingContextStates.All), instance);
            }

            return surrogateSelector;
        }
    }
}
