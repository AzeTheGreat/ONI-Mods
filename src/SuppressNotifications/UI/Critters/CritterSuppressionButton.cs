using UnityEngine;

namespace SuppressNotifications
{
    class CritterSuppressionButton : EntitySuppressionButton
    {
        public override void OnPrefabInit()
        {
            base.OnPrefabInit();

            Subscribe((int)GameHashes.SpawnedFrom, (object data) => OnSpawnedFrom(data));
            Subscribe((int)GameHashes.LayEgg, (object data) => OnLayEgg(data));
        }

        private void OnSpawnedFrom(object baby)
        {
            gameObject.Trigger((int)GameHashes.CopySettings, baby);
        }

        private void OnLayEgg(object egg)
        {
            var noteComp = (egg as GameObject).AddOrGet<NotificationsSuppressedComp>();
            var statComp = (egg as GameObject).AddOrGet<StatusItemsSuppressedComp>();

            noteComp.Init();
            statComp.Init();
            noteComp.OnCopySettings(gameObject);
            statComp.OnCopySettings(gameObject);
        }
    }
}
