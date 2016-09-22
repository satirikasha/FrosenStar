using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public static class WidgetResourcesCache {

    private const string WidgetPath = "Prefabs/UI/Widgets";

    private static UIWidget[] Widgets;

    public static UIWidget GetWidget(Type type) {
        PrepareWidgets();
        return Widgets.FirstOrDefault(_ => _.GetType() == type);
    }

    public static T GetWidget<T>() where T : UIWidget {
        PrepareWidgets();
        return Widgets.OfType<T>().FirstOrDefault();
    }

    private static void PrepareWidgets() {
        if (Widgets == null)
            Widgets = Resources.LoadAll<UIWidget>(WidgetPath);
    }
}

