using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace SuppressNotifications
{
    public static class Util
    {
        public static bool ShouldShowIcon(StatusItem item, GameObject go)
        {
            bool isShown = go.GetComponent<StatusItemsSuppressed>()?.IsShown(item) ?? true;
            return item.ShouldShowIcon() && isShown;
        }
    }
}
