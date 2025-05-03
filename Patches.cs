using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AwesomeTechnologies.TouchReact;
using CarsExtended.Patches;
using HarmonyLib;
using I2.Loc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using static Drivetrain;

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

    // 	static void Postfix()
    // 	{
    // 		//
    // 	}
    // }

    //[HarmonyPatch(typeof(CarMaterialManager))]
    //static class CarMaterialManager_Init_Patch
    //{
    //    [HarmonyPatch(nameof(CarMaterialManager.Init))]
    //    [HarmonyPrefix]
    //    static void Init_Prefix()
    //    {
    //        Main.Log("CarMaterialManager.Init Prefix");
    //    }

    //    [HarmonyPatch(nameof(CarMaterialManager.Init))]
    //    [HarmonyPostfix]
    //    static void Init_Postfix()
    //    {
    //        Main.Log("CarMaterialManager.Init Postfix");
    //    }

    //    [HarmonyPatch(nameof(CarMaterialManager.LoadLiveryAndDirtTextures))]
    //    [HarmonyPrefix]
    //    static bool LoadLiveryAndDirtTextures_Prefix()
    //    {
    //        Main.Log("CarMaterialManager.LoadLiveryAndDirtTextures Prefix");

    //        //return false; // TEST
    //        return true;
    //    }

    //    [HarmonyPatch(nameof(CarMaterialManager.LoadLiveryAndDirtTextures))]
    //    [HarmonyPostfix]
    //    static void LoadLiveryAndDirtTextures_Postfix()
    //    {
    //        Main.Log("CarMaterialManager.LoadLiveryAndDirtTextures Postfix");
    //    }
    //}

    //[HarmonyPatch(typeof(StageSceneManager), "StartEvent")]
    //static class StageSceneManager_StartEvent_Patch
    //{
    //    static void Prefix(bool doKinematicDelay)
    //    {
    //        Main.Log("StageSceneManager.StartEvent Prefix");
    //        Main.Log("doKinematicDelay : " + doKinematicDelay);
    //        Main.Log("Has stage timer manager : " + (GameEntryPoint.EventManager.stageTimerManager != null));
    //    }

    //    static void Postfix()
    //    {
    //        Main.Log("StageSceneManager.StartEvent Postfix");
    //        Main.Log("Audio sources : " + Main.GetField<Dictionary<int, bool>, PlayerManager>(__instance, "_wheelRaycastLookup", System.Reflection.BindingFlags.Instance));
    //    }
    //}

    //[HarmonyPatch(typeof(RallyManager), "OnStageStart")]
    //static class RallyManager_OnStageStart_Patch
    //{
    //    static void Prefix()
    //    {
    //        Main.Log("RallyManager.OnStageStart Prefix");
    //        Main.Log("Has stage timer manager : " + (GameEntryPoint.EventManager.stageTimerManager != null));
    //    }

    //    static void Postfix()
    //    {
    //        Main.Log("RallyManager.OnStageStart Postfix");
    //        Main.Log("Audio sources : " + Main.GetField<Dictionary<int, bool>, PlayerManager>(__instance, "_wheelRaycastLookup", System.Reflection.BindingFlags.Instance));
    //    }
    //}

    // CarCameras

    //[HarmonyPatch(typeof(CameraManager), "Update")]
    //static class CameraManager_Update
    //{
    //    static void Prefix()
    //    {
    //        Main.Log("CameraManager.Update Prefix");
    //    }

    //    static void Postfix()
    //    {
    //        Main.Log("CameraManager.Update Postfix");
    //    }
    //}

    //[HarmonyPatch(typeof(CarDynamics))]
    //static class CarDynamics_Patch
    //{
    //    [HarmonyPatch("SetBrakes")]
    //    [HarmonyPostfix]
    //    static void Postfix(CarDynamics __instance)
    //    {
    //        // TEST
    //        SetWheel(__instance.axles.frontAxle.rightWheel);
    //        SetWheel(__instance.axles.frontAxle.leftWheel);
    //        SetWheel(__instance.axles.rearAxle.rightWheel);
    //        SetWheel(__instance.axles.rearAxle.leftWheel);

    //        Main.Try(() =>
    //        {
    //            if (__instance.axles.frontAxle.rightWheel != null)
    //            {
    //                FieldInfo[] fields = typeof(Wheel).GetFields(BindingFlags.Instance | BindingFlags.Public);
    //                string debug = "";

    //                foreach (FieldInfo field in fields)
    //                    debug += field.Name + " : " + field.GetValue(__instance.axles.frontAxle.rightWheel) + "\n";

    //                Main.Log(debug.TrimEnd('\n'));
    //            }
    //        });
    //        // TEST
    //    }

    //    static void SetWheel(Wheel wheel)
    //    {
    //        wheel.suspensionTravel = 0.3f;
    //        wheel.suspensionRate = 14000;
    //        wheel.bumpRate = 2500;
    //        wheel.reboundRate = 3500;
    //        wheel.fastBumpFactor = 0.95f;
    //        wheel.fastReboundFactor = 0.95f;
    //        wheel.brakeFrictionTorque = 900;
    //        wheel.sidewaysGripFactor = 0.95f;
    //        wheel.forwardGripFactor = 0.95f;
    //        wheel.maxSteeringAngle = 30;
    //    }
    //}

    //[HarmonyPatch(typeof(Drivetrain))]
    //static class Drivetrain_Patch
    //{
    //    [HarmonyPatch("Awake")]
    //    [HarmonyPostfix]
    //    static void Awake_Postfix()
    //    {
    //        Main.Log("Drivetrain.Awake");
    //    }

    //    [HarmonyPatch("Init")]
    //    [HarmonyPostfix]
    //    static void Init_Postfix()
    //    {
    //        Main.Log("Drivetrain.Init");
    //    }
    //}

    //[HarmonyPatch(typeof(AxisCarController))]
    //static class AxisCarController_Patch
    //{
    //    [HarmonyPatch("Awake")]
    //    [HarmonyPostfix]
    //    static void Awake_Postfix()
    //    {
    //        Main.Log("AxisCarController.Awake");
    //    }
    //}

    //[HarmonyPatch(typeof(Axles))]
    //static class Axles_Patch
    //{
    //    [HarmonyPatch("Awake")]
    //    [HarmonyPostfix]
    //    static void Awake_Postfix()
    //    {
    //        Main.Log("Axles.Awake");
    //    }

    //    [HarmonyPatch("Start")]
    //    [HarmonyPostfix]
    //    static void Start_Postfix()
    //    {
    //        Main.Log("Axles.Start");
    //    }
    //}

    //[HarmonyPatch(typeof(BrakeEffects))]
    //static class BrakeEffects_Patch
    //{
    //    [HarmonyPatch("Awake")]
    //    [HarmonyPrefix]
    //    static void Awake_Prefix()
    //    {
    //        Main.Log("BrakeEffects.Awake Prefix");
    //    }
    //}

    //[HarmonyPatch(typeof(SoundController))]
    //static class SoundController_Patch
    //{
    //    [HarmonyPatch("Init")]
    //    [HarmonyPostfix]
    //    static void Init_Postfix()
    //    {
    //        Main.Log("SoundController.Init");
    //    }
    //}

    //[HarmonyPatch(typeof(Arcader))]
    //static class Arcader_Patch
    //{
    //    [HarmonyPatch("Awake")]
    //    [HarmonyPostfix]
    //    static void Awake_Postfix()
    //    {
    //        Main.Log("Arcader.Awake");
    //    }

    //    [HarmonyPatch("Start")]
    //    [HarmonyPostfix]
    //    static void Start_Postfix()
    //    {
    //        Main.Log("Arcader.Start");
    //    }
    //}

    //[HarmonyPatch(typeof(AerodynamicResistance))]
    //static class AerodynamicResistance_Patch
    //{
    //    [HarmonyPatch("Start")]
    //    [HarmonyPostfix]
    //    static void Start_Postfix()
    //    {
    //        Main.Log("AerodynamicResistance.Start");
    //    }
    //}

    //[HarmonyPatch(typeof(Wing))]
    //static class Wing_Patch
    //{
    //    [HarmonyPatch("Init")]
    //    [HarmonyPostfix]
    //    static void Init_Postfix()
    //    {
    //        Main.Log("Wing.Init");
    //    }
    //}

    //[HarmonyPatch(typeof(PlayerCollider))]
    //static class PlayerCollider_Patch
    //{
    //    [HarmonyPatch("Start")]
    //    [HarmonyPostfix]
    //    static void Start_Postfix()
    //    {
    //        Main.Log("PlayerCollider.Start");
    //    }
    //}

    //[HarmonyPatch(typeof(ResetCar))]
    //static class ResetCar_Patch
    //{
    //    [HarmonyPatch("Start")]
    //    [HarmonyPostfix]
    //    static void Start_Postfix()
    //    {
    //        Main.Log("ResetCar.Start");
    //    }
    //}

    //[HarmonyPatch(typeof(SkidmarksManager))]
    //static class SkidmarksManager_Patch
    //{
    //    [HarmonyPatch("Awake")]
    //    [HarmonyPostfix]
    //    static void Awake_Postfix()
    //    {
    //        Main.Log("SkidmarksManager.Awake");
    //    }
    //}

    //[HarmonyPatch(typeof(PlayerVibrator))]
    //static class PlayerVibrator_Patch
    //{
    //    [HarmonyPatch("Start")]
    //    [HarmonyPostfix]
    //    static void Start_Postfix()
    //    {
    //        Main.Log("PlayerVibrator.Start");
    //    }
    //}

    //[HarmonyPatch(typeof(TouchReactCollider))]
    //static class TouchReactCollider_Patch
    //{
    //    [HarmonyPatch("Awake")]
    //    [HarmonyPostfix]
    //    static void Awake_Postfix()
    //    {
    //        Main.Log("TouchReactCollider.Awake");
    //    }

    //    [HarmonyPatch("Start")]
    //    [HarmonyPostfix]
    //    static void Start_Postfix()
    //    {
    //        Main.Log("TouchReactCollider.Start");
    //    }
    //}

    //[HarmonyPatch(typeof(HeadlightManager))]
    //static class HeadlightManager_Patch
    //{
    //    [HarmonyPatch("Init")]
    //    [HarmonyPostfix]
    //    static void Init_Postfix()
    //    {
    //        Main.Log("HeadlightManager.Init");
    //    }
    //}

    //[HarmonyPatch(typeof(ParticleManager))]
    //static class ParticleManager_Patch
    //{
    //    [HarmonyPatch("Init")]
    //    [HarmonyPostfix]
    //    static void Init_Postfix()
    //    {
    //        Main.Log("ParticleManager.Init");
    //    }
    //}

    //[HarmonyPatch(typeof(Setup))]
    //static class Setup_Patch
    //{
    //    [HarmonyPatch("Awake")]
    //    [HarmonyPostfix]
    //    static void Awake_Postfix()
    //    {
    //        Main.Log("Setup.Awake");
    //    }

    //    [HarmonyPatch("LoadSetup")]
    //    [HarmonyPostfix]
    //    static void LoadSetup_Postfix(Setup __instance)
    //    {
    //        Main.Log("Setup.LoadSetup");

    //        string text = Main.GetField<string, Setup>(__instance, "setupFileText", BindingFlags.Instance);

    //        foreach (Wheel wheel in __instance.GetComponent<Axles>().allWheels)
    //        {
    //            bool front = __instance.GetComponent<Axles>().frontAxle.wheels.Contains(wheel);

    //            ForceSetupWheel(wheel, front);
    //            string dump = DumpWheel(wheel, front);
    //            Main.Log(dump);

    //            Main.Try(() =>
    //            {
    //                string path = "C:\\Users\\Mike\\Downloads";
    //                string fullPath = Path.Combine(path, __instance.gameObject.name + "_" + wheel.gameObject.name) + ".txt";
    //                File.WriteAllText(fullPath, dump);
    //                Main.Log("Print to file");
    //            });
    //        }
    //    }

    //    static string DumpWheel(Wheel wheel, bool front)
    //    {
    //        string result = "";

    //        // suspensions
    //        result += "suspensionTravel : " + wheel.suspensionTravel + " / 0.3\n"; // x
    //        result += "suspensionRate : " + wheel.suspensionRate + " / 14000\n"; // x
    //        result += "bumpRate : " + wheel.bumpRate + " / 2500\n"; // x
    //        result += "reboundRate : " + wheel.reboundRate + " / 3500\n"; // x
    //        result += "fastBumpFactor : " + wheel.fastBumpFactor + " / 0.95\n"; // x
    //        result += "fastReboundFactor : " + wheel.fastReboundFactor + " / 0.95\n"; // x
    //        result += "camber : " + wheel.camber + " / 0\n";
    //        result += "maxSteeringAngle : " + wheel.maxSteeringAngle + " / " + (front ? 30 : 0) + "\n\n"; // x

    //        // brake
    //        result += "brakeFrictionTorque : " + wheel.brakeFrictionTorque + " / 900 ?\n"; // x
    //        result += "handbrakeFrictionTorque : " + wheel.handbrakeFrictionTorque + " / " + (front ? 0 : 900) + "\n\n";

    //        // tires
    //        result += "forwardGripFactor  : " + wheel.forwardGripFactor + " / 0.95\n"; // x
    //        result += "sidewaysGripFactor  : " + wheel.sidewaysGripFactor + " / 0.95\n"; // x
    //        result += "pressure  : " + wheel.pressure + " / 200\n";
    //        result += "optimalPressure  : " + wheel.optimalPressure + " / 200\n\n";

    //        // wheel
    //        result += "mass : " + wheel.mass + " / 40\n"; // x
    //        result += "radius : " + wheel.radius + " / 0.26\n"; // x
    //        result += "width : " + wheel.width + " / 0.285"; // x

    //        return result;
    //    }

    //    static void ForceSetupWheel(Wheel wheel, bool front)
    //    {
    //        // suspensions
    //        wheel.suspensionTravel = 0.2f;
    //        wheel.suspensionRate = 20000;
    //        wheel.bumpRate = 4000;
    //        wheel.reboundRate = 4000;
    //        wheel.fastBumpFactor = 0.3f;
    //        wheel.fastReboundFactor = 0.3f;
    //        wheel.camber = 0;
    //        wheel.maxSteeringAngle = front ? 33 : 0;

    //        // brake
    //        wheel.brakeFrictionTorque = 800;
    //        wheel.handbrakeFrictionTorque = front ? 0 : 700;

    //        // tires
    //        wheel.forwardGripFactor = 1;
    //        wheel.sidewaysGripFactor = 1;
    //        wheel.pressure = 200;
    //        wheel.optimalPressure = 200;

    //        // wheel
    //        wheel.mass = 50;
    //        wheel.radius = 0.35f;
    //        wheel.width = 0.2f;
    //    }
    //}
}