using UnityEngine;
using System.Collections;

public class IntervalAttribute : PropertyAttribute {

    public readonly float max;
    public readonly float min;

    public IntervalAttribute(float min, float max) {
        this.min = min;
        this.max = max;
    }
}
