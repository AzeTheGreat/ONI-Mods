using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuppressNotifications
{
    class CopyEntitySettingsTool : DragTool
    {
        public static CopyEntitySettingsTool instance;

        private List<int> cells = new();
        private GameObject sourceGameObject;

        public void SetSourceObject(GameObject go) => sourceGameObject = go;
        public void Activate() => PlayerController.Instance.ActivateTool(this);

        public override void OnPrefabInit()
        {
            // Initialize
            base.OnPrefabInit();
            instance = this;

            // Set the cursor
            boxCursor = CopySettingsTool.Instance.boxCursor;

            // Set the area visualizer
            var avTemplate = CopySettingsTool.Instance.areaVisualizer;
            var areaVisualizer = Util.KInstantiate(avTemplate, gameObject, nameof(CopyEntitySettingsTool) + "AreaVisualizer");
            areaVisualizer.SetActive(false);
            areaVisualizerSpriteRenderer = areaVisualizer.GetComponent<SpriteRenderer>();

            this.areaVisualizer = areaVisualizer;
            areaVisualizerTextPrefab = CopySettingsTool.Instance.areaVisualizerTextPrefab;

            // Set the visualizer
            visualizer = CopySettingsTool.Instance.visualizer;

            // Set the hover card
            var hoverConfig = gameObject.AddComponent<HoverTextConfiguration>();
            var hoverTemplate = CopySettingsTool.Instance.gameObject.GetComponent<HoverTextConfiguration>();
            hoverConfig.ToolNameStringKey = hoverTemplate.ToolNameStringKey;
            hoverConfig.ActionStringKey = hoverTemplate.ActionStringKey;
            hoverConfig.ActionName = hoverTemplate.ActionName;
            hoverConfig.ToolName = hoverTemplate.ToolName;
        }

        public override void OnDragTool(int cell, int distFromOrigin)
        {
            if (sourceGameObject == null)
                return;

            if (Grid.IsValidCell(cell) && !cells.Contains(cell))
                cells.Add(cell);
        }

        public override void OnDragComplete(Vector3 cursorDown, Vector3 cursorUp)
        {
            if (sourceGameObject.GetComponent<CritterSuppressionButton>() != null)
                CopyCritterSettings();
            if (sourceGameObject.GetComponent<CropSuppressionButton>() != null)
                CopyCropSettings();
            if (sourceGameObject.GetComponent<MinionSuppressionButton>() != null)
                CopyMinionSettings();
        }

        public override void OnLeftClickDown(Vector3 cursor_pos)
        {
            base.OnLeftClickDown(cursor_pos);
            cells.Clear();
        }

        public override void OnDeactivateTool(InterfaceTool new_tool)
        {
            base.OnDeactivateTool(new_tool);
            sourceGameObject = null;
        }

        private void CopyCritterSettings() => CopySettings<CreatureBrain>(Components.Brains, kmb => kmb.isSpawned && !kmb.HasTag(GameTags.Dead));
        private void CopyCropSettings() => CopySettings<Crop>(Components.Crops);
        private void CopyMinionSettings() => CopySettings<MinionIdentity>(Components.MinionIdentities);

        private void CopySettings<T>(IEnumerable cmps, Func<KMonoBehaviour, bool> predicate = null) where T : KMonoBehaviour
        {
            foreach (var cmp in cmps)
            {
                var kmb = cmp as T;
                if (kmb != null && (predicate?.Invoke(kmb) ?? true) && cells.Contains(Grid.PosToCell(kmb)))
                    CopyTo(kmb.gameObject);
            }

            void CopyTo(GameObject go)
            {
                go.Trigger((int)GameHashes.CopySettings, sourceGameObject);
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.COPIED_SETTINGS, go.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
            }
        }
    }
}
