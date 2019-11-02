using STRINGS;
using System.Collections.Generic;
using UnityEngine;

namespace RebalancedTiles
{
    public class Combustible : GameStateMachine<Combustible, Combustible.Instance>
    {
        public State Unburned;
        public State Burned;

        public override void InitializeStates(out BaseState defaultState)
        {
            defaultState = Unburned;

            Unburned
                .Transition(Burned, (Instance smi) => smi.IsTemp(smi), UpdateRate.SIM_4000ms);
            Burned
                .Transition(Unburned, (Instance smi) => !smi.IsTemp(smi), UpdateRate.SIM_4000ms)
                .Update(delegate (Instance smi, float dt) { smi.TryDoOverheatDamage(); }, UpdateRate.SIM_4000ms);
        }

        public new class Instance : GameInstance
        {
            public Instance(IStateMachineTarget master) : base(master) { }

            public bool IsTemp(Instance smi)
            {
                float temp = Grid.Temperature[Grid.PosToCell(smi)];
                if (temp > 300)
                    return true;
                else
                    return false;
            }

            public void LogTest(Instance smi)
            {
                Util.KDestroyGameObject(smi.gameObject);
            }

            private float lastOverheatDamageTime;

            public void TryDoOverheatDamage()
            {
                if (Time.time - lastOverheatDamageTime < 7.5f)
                    return;

                lastOverheatDamageTime += 7.5f;
                master.Trigger(-794517298, new BuildingHP.DamageSourceInfo
                {
                    damage = 10,
                    source = BUILDINGS.DAMAGESOURCES.BUILDING_OVERHEATED,
                    popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.OVERHEAT,
                    fullDamageEffectName = "smoke_damage_kanim"
                });

                if (gameObject.GetComponent<BuildingHP>().IsBroken)
                {
                    var comp = gameObject.AddComponent<DestroyToTile>();
                    comp.def = Assets.GetBuildingDef(TileConfig.ID);
                    comp.cell = Grid.PosToCell(gameObject);
                    comp.orientation = Orientation.Neutral;
                    comp.resource_storage = null;
                    comp.selected_elements = new List<Tag> { Grid.Element[Grid.PosToCell(gameObject)].tag };
                    comp.temp = Grid.Temperature[Grid.PosToCell(gameObject)];
                    comp.playSound = false; 
                }
            }
        }
    }

    public class DestroyToTile : KMonoBehaviour
    {
        public BuildingDef def;
        public int cell;
        public Orientation orientation;
        public Storage resource_storage;
        public IList<Tag> selected_elements;
        public float temp;
        public bool playSound = true;
        public float timeBuilt = -1;

        new void OnDestroy()
        {
            def.Build(cell, orientation, resource_storage, selected_elements, temp, playSound, timeBuilt);
        }
    }
}
