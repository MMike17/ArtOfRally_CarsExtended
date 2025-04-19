using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

using static GhostManager;

// TODO : We'll finish this later

namespace CarsExtended
{
    //[HarmonyPatch(typeof(GhostManager), nameof(GhostManager.InitializeGhost))]
    static class GhostManager_Patch
    {
        private static GameObject carPrefab;
        private static bool shouldReplace;

        //private static void Prefix(GhostManager __instance)
        //{
        //    GhostData currentData = Main.GetField<GhostData, GhostManager>(__instance, "_currentData", BindingFlags.Instance);
        //    shouldReplace = ExtraCarManager.IsCarInjected(currentData._livery.CarPrefabName);

        //    if (!shouldReplace)
        //        return;

        //    carPrefab = ExtraCarManager.LoadBundleCar(currentData._livery.CarPrefabName);

        //    Main.Log("Overriding prefab loading for car \"" + currentData._livery.CarPrefabName + "\"");
        //}

        //private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        //{
        //    foreach (CodeInstruction instruction in instructions)
        //    {
        //        if (!shouldReplace)
        //        {
        //            yield return instruction;
        //            continue;
        //        }

        //        // loading prefab to spawn on stack
        //        if (instruction.opcode == OpCodes.Ldloc_S && (instruction.operand as LocalBuilder)?.LocalIndex == 6)
        //        {
        //            // load new prefab into stack
        //            yield return new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(GameObject), nameof(carPrefab)));
        //            Main.Log("Loading new prefab in memory");
        //        }
        //        else if (instruction.opcode == OpCodes.Call &&
        //            instruction.operand is MethodInfo method &&
        //            method.Name == "Instantiate" &&
        //            method.DeclaringType == typeof(Object)) // calling prefab spawn
        //        {
        //            yield return instruction; // call spawn method
        //            Main.Log("New car prefab spawned");

        //            // calls car configurator method
        //            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CarSpawner), nameof(CarSpawner.ConfigureCar)));

        //            // next instruction should be push to local variable
        //        }
        //        else
        //            yield return instruction;
        //    }
        //}
    }
}