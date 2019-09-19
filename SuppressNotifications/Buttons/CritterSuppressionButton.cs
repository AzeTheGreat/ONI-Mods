using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SuppressNotifications
{
    class CritterSuppressionButton : SuppressionButton
    {
        [MyCmpAdd]
        private CopyCritterSettings copyCritterSettings;

        private static readonly EventSystem.IntraObjectHandler<CritterSuppressionButton> OnSpawnedFromDelegate =
            new EventSystem.IntraObjectHandler<CritterSuppressionButton>(OnSpawnedFrom);

        private static readonly EventSystem.IntraObjectHandler<CritterSuppressionButton> OnLayEggDelegate =
            new EventSystem.IntraObjectHandler<CritterSuppressionButton>(OnLayEgg);

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            Subscribe((int)GameHashes.SpawnedFrom, OnSpawnedFromDelegate);
            Subscribe((int)GameHashes.LayEgg, OnLayEggDelegate);
        }

        private static void OnSpawnedFrom(CritterSuppressionButton adult, object baby)
        {
            adult.gameObject.Trigger((int)GameHashes.CopySettings, baby);
        }

        private static void OnLayEgg(CritterSuppressionButton critter, object egg)
        {
            var noteComp = (egg as GameObject).AddOrGet<NotificationsSuppressedComp>();
            var statComp = (egg as GameObject).AddOrGet<StatusItemsSuppressedComp>();

            noteComp.Init();
            statComp.Init();
            noteComp.OnCopySettings(critter.gameObject);
            statComp.OnCopySettings(critter.gameObject);
        }
    }
}
