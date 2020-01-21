using AzeLib;
using AzeLib.Extensions;
using Harmony;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BetterLogicOverlay.LogicSettingDisplay.ModCompatability
{
    class AlarmNotificationTrigger : LogicSettingDispComp
    {
        public override string GetSetting()
        {
            return gameObject.name.Truncate(5);
        }

        private class Add : PostLoad
        {
            protected override IEnumerable<MethodBase> PostLoadTargetMethods()
            {
                var notificationTrigger = AccessTools.Method("NotificationTriggerConfig:DoPostConfigureComplete");
                if (notificationTrigger != null) yield return notificationTrigger;
            }

            public static void Postfix(GameObject go) => go.AddComponent<AlarmNotificationTrigger>();
        }
    }
}
