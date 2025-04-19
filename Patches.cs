using HarmonyLib;

namespace CarsExtended
{
    // Patch model
    // [HarmonyPatch(typeof(), nameof())]
    // [HarmonyPatch(typeof(), MethodType.)]
    // static class type_method_Patch
    // {
    // 	static void Prefix()
    // 	{
    // 		//
    // 	}

    //	this will negate the method
    //  	static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    //  	{
    //      	foreach (var instruction in instructions)
    //          	yield return new CodeInstruction(OpCodes.Ret);
    //  	}

    // 	static void Postfix()
    // 	{
    // 		//
    // 	}
    // }

    // TEST
    [HarmonyPatch(typeof(Setup), "Awake")]
    static class Setup_Awake_Patch
    {
        static void Prefix()
        {
            Main.Log("Setup Awake");
        }
    }

    [HarmonyPatch(typeof(Setup), "Start")]
    static class Setup_Start_Patch
    {
        static void Prefix()
        {
            Main.Log("Setup Start");
        }
    }
    // TEST
}
