﻿using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;

namespace AzeLib.Extensions
{
    public static class GameObjectExt
    {
        public static Component GetReflectionComp(this GameObject go, string typeString)
        {
            var type = AccessTools.TypeByName(typeString);
            if (type == null)
                return null;
            else
                return go.GetComponent(type);
        }

        public static StateMachine.BaseDef GetDef(this GameObject go, Type type)
        {
            var smc = go.GetComponent<StateMachineController>();
            if (smc == null)
                return null;

            return smc.cmpdef.defs.FirstOrDefault(x => x.GetType() == type);
        }
    }
}
