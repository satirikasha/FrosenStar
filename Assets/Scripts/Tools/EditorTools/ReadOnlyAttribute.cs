using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

[AttributeUsage(AttributeTargets.Field)]
public class ReadOnlyAttribute : PropertyAttribute {

    public readonly string PropertyName;
    public readonly bool Invert;

    public ReadOnlyAttribute(string propertyName = null, bool invert = true) {
        PropertyName = propertyName;
        Invert = invert;
    }
}
