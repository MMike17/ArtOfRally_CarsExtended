using System.Collections.Generic;
using HarmonyLib;

namespace CarsExtended
{
    [HarmonyPatch(typeof(GhostManager), nameof(GhostManager.InitializeGhost))]
    internal static class GhostManager_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // cancel Resources.Load call and replace with CarSpawner.SpawnCar()
        }
    }
}