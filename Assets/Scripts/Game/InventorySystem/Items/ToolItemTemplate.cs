using UnityEngine;
using System.Collections;
using System;

public abstract class ToolItemTemplate<T> : SlotItemTemplate<T> where T : ToolItem {}

public abstract class ToolItem : SlotItem {}

