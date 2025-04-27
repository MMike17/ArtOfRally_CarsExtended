using System;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using I2.Loc;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO : How do I remove injected cars when I turn the mod off ?
// Remove cars from selection menu / remove cars from game car list (doesn't that break selection indexes ?)

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
}
