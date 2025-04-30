using System;
using System.Collections;
using System.Reflection;
using AwesomeTechnologies.TouchReact;
using HarmonyLib;
using I2.Loc;
using UnityEngine;

using static Drivetrain;

namespace CarsExtended.Patches
{
    // TODO : Change patch class names to make more sense

    internal static class CarPatcher
    {
        internal static bool CheckCarInjected(Component obj)
        {
            bool check = ChecKName(obj);

            // check parent
            if (!check)
                check = ChecKName(obj.GetComponentInParent<Setup>());

            if (check)
                Main.Log("Configuring " + obj.GetType().Name + " component");

            return check;

            bool ChecKName(Component component)
            {
                string carPrefabName = component.name.Replace("(Clone)", "");
                return ExtraCarManager.IsCarInjected(carPrefabName);
            }
        }
    }

    [HarmonyPatch(typeof(Setup), nameof(Setup.LoadSetup))]
    static class Setup_Patch
    {
        private static void Prefix(Setup __instance)
        {
            if (!CarPatcher.CheckCarInjected(__instance))
                return;

            __instance.carClass = CarSpawner.carInfos.data.carClass;
        }
    }

    [HarmonyPatch(typeof(Axles), "Awake")]
    static class Axles_Patch
    {
        private static void Prefix(Axles __instance)
        {
            if (!CarPatcher.CheckCarInjected(__instance))
                return;

            CarInfos carInfos = CarSpawner.carInfos;
            Transform wheelsRoot = __instance.transform.Find("Wheels");

            __instance.frontAxle.rightWheel = SetupWheel(wheelsRoot.Find("WheelFR").gameObject.AddComponent<Wheel>(), carInfos);
            __instance.frontAxle.leftWheel = SetupWheel(wheelsRoot.Find("WheelFL").gameObject.AddComponent<Wheel>(), carInfos);

            __instance.rearAxle.rightWheel = SetupWheel(wheelsRoot.Find("WheelRR").gameObject.AddComponent<Wheel>(), carInfos);
            __instance.rearAxle.leftWheel = SetupWheel(wheelsRoot.Find("WheelRL").gameObject.AddComponent<Wheel>(), carInfos);

            if (wheelsRoot.Find("WheelRRL") != null)
            {
                __instance.otherAxles = new Axle[1]
                {
                    new Axle()
                    {
                        rightWheel = SetupWheel(wheelsRoot.Find("WheelRRR").gameObject.AddComponent<Wheel>(), carInfos),
                        leftWheel = SetupWheel(wheelsRoot.Find("WheelRRL").gameObject.AddComponent<Wheel>(), carInfos)
                    }
                };
            }

            CarSpawner.SetupShadow(__instance);
        }

        private static Wheel SetupWheel(Wheel wheel, CarInfos carInfos)
        {
            wheel.width = carInfos.wheelWidth;
            wheel.radius = carInfos.wheelRadius;
            wheel.model = wheel.transform.Find(wheel.name).gameObject;

            return wheel;
        }
    }

    [HarmonyPatch(typeof(CarDynamics), "Awake")]
    static class CarDynamics_Patch
    {
        private static void Prefix(CarDynamics __instance)
        {
            if (!CarPatcher.CheckCarInjected(__instance))
                return;

            BrakeEffects brakeEffects = CarSpawner.brakelightsTransform.GetComponent<BrakeEffects>();

            brakeEffects.RightBrakeLightTransform = CarSpawner.brakelightsTransform.Find("BrakeLight R");
            brakeEffects.LeftBrakeLightTransform = CarSpawner.brakelightsTransform.Find("BrakeLight L");

            // WHY THE HELL DO I EVEN NEED THIS ?!
            CoroutineManager.Start(ForceSetActive());

            IEnumerator ForceSetActive()
            {
                float time = Time.time;

                while (true)
                {
                    if (!brakeEffects.enabled)
                        brakeEffects.enabled = true;
                    else
                    {
                        Main.InvokeMethod(brakeEffects, "Awake", BindingFlags.Instance, null);
                        yield break;
                    }

                    yield return null;
                }
            }
        }
    }

    [HarmonyPatch(typeof(TouchReactCollider), "Start")]
    static class TouchReactCollider_Patch
    {
        private static void Prefix(TouchReactCollider __instance)
        {
            if (!CarPatcher.CheckCarInjected(__instance))
                return;

            __instance.AddChildColliders = false;
            __instance.ColliderScale = 1.6f;
            __instance.tag = Tags.CAR;
        }
    }

    [HarmonyPatch(typeof(Drivetrain), "Awake")]
    static class Drivetrain_Patch
    {
        private static void Prefix(Drivetrain __instance)
        {
            if (!CarPatcher.CheckCarInjected(__instance))
                return;

            __instance.transmissionForParticles = (TransmissionForParticles)Enum.Parse(
                typeof(TransmissionForParticles),
                CarSpawner.carInfos.data.carStats.Transmission.ToString()
            );
            __instance.engineOrientation = CarSpawner.carInfos.engineOrientation;
        }
    }

    [HarmonyPatch(typeof(HeadlightManager), nameof(HeadlightManager.Init))]
    static class HeadlightManager_Patch
    {
        private static void Prefix(HeadlightManager __instance)
        {
            if (!CarPatcher.CheckCarInjected(__instance))
                return;

            __instance.HeadlightPosition = __instance.transform.Find("Headlights");
            Transform auxLightsRoot = __instance.transform.Find("AuxHeadlights");

            for (int i = 0; i < auxLightsRoot.childCount - 2; i++)
                auxLightsRoot.Find("Light" + (i + 1)).tag = Tags.CAR_BREAKABLE_OBJECT;
        }
    }

    [HarmonyPatch(typeof(ParticleManager), "Init")]
    static class ParticleManager_Patch
    {
        private static void Prefix(ParticleManager __instance)
        {
            if (!CarPatcher.CheckCarInjected(__instance))
                return;

            Transform particlesRoot = __instance.transform.Find("Particles");

            __instance.AmbientEffectsPosition = particlesRoot.Find("Ambient Effects");
            __instance.Brakelight_Position_L = CarSpawner.brakelightsTransform.Find("BrakeLight L");
            __instance.Brakelight_Position_R = CarSpawner.brakelightsTransform.Find("BrakeLight R");
            __instance.DustPosition = particlesRoot.Find("Dust");
            __instance.EngineSteamPosition = particlesRoot.Find("EngineSteam");
            __instance.ExhaustBackfire_Position_1 = CarSpawner.lightsTransform.Find("Backfire/Backfire L");
            __instance.ExhaustBackfire_Position_2 = CarSpawner.lightsTransform.Find("Backfire/Backfire R");
            __instance.Exhaust1Position = particlesRoot.Find("Exhaust1");
            __instance.Exhaust2Position = particlesRoot.Find("Exhaust2");
        }
    }

    [HarmonyPatch(typeof(PlayerVibrator), "Start")]
    static class PlayerVibrator_Patch
    {
        private static void Prefix(PlayerVibrator __instance)
        {
            if (!CarPatcher.CheckCarInjected(__instance))
                return;

            __instance.axles = __instance.GetComponent<Axles>();
        }
    }

    [HarmonyPatch(typeof(SoundController), nameof(SoundController.Init))]
    static class SoundController_Patch
    {
        private static void Prefix(SoundController __instance)
        {
            if (!CarPatcher.CheckCarInjected(__instance))
                return;

            SoundController sourceSoundController = CarSpawner.sourceCarPrefab.GetComponent<SoundController>();

            __instance.antiLagEnabled = sourceSoundController.antiLagEnabled;
            __instance.playExhaustParticles = sourceSoundController.playExhaustParticles;
            __instance.flatTireVolume = sourceSoundController.flatTireVolume;
            __instance.flatTire = sourceSoundController.flatTire;
            __instance.waterSplashNoise = sourceSoundController.waterSplashNoise;
            __instance.rollingNoiseOffroad = sourceSoundController.rollingNoiseOffroad;
            __instance.rollingNoiseSand = sourceSoundController.rollingNoiseSand;
            __instance.rollingNoiseGrass = sourceSoundController.rollingNoiseGrass;
            __instance.windVolume = sourceSoundController.windVolume;
            __instance.wind = sourceSoundController.wind;
            __instance.antilag1 = sourceSoundController.antilag1;
            __instance.backfire3 = sourceSoundController.backfire3;
            __instance.backfire2 = sourceSoundController.backfire2;
            __instance.backfire1 = sourceSoundController.backfire1;
            __instance.turboWasteGate3 = sourceSoundController.turboWasteGate3;
            __instance.turboWasteGate2 = sourceSoundController.turboWasteGate2;
            __instance.turboWasteGate1 = sourceSoundController.turboWasteGate1;
            __instance.shiftTriggerVolume = sourceSoundController.shiftTriggerVolume;
            __instance.shiftTrigger = sourceSoundController.shiftTrigger;
            __instance.scrapeNoiseVolume = sourceSoundController.scrapeNoiseVolume;
            __instance.scrapeNoise = sourceSoundController.scrapeNoise;
            __instance.crashLowVolume = sourceSoundController.crashLowVolume;
            __instance.crashLowSpeed = sourceSoundController.crashLowSpeed;
            __instance.crashHighVolume = sourceSoundController.crashHighVolume;
            __instance.crashHiSpeed = sourceSoundController.crashHiSpeed;
            __instance.skidPitchFactor = sourceSoundController.skidPitchFactor;
            __instance.gravelSkid = sourceSoundController.gravelSkid;
            __instance.tarmacWetSkid = sourceSoundController.tarmacWetSkid;
            __instance.tarmacDrySkid = sourceSoundController.tarmacDrySkid;
            __instance.brakeNoiseVolume = sourceSoundController.brakeNoiseVolume;
            __instance.brakeNoise = sourceSoundController.brakeNoise;
            __instance.transmissionSourcePitch = sourceSoundController.transmissionSourcePitch;
            __instance.transmissionVolumeReverse = sourceSoundController.transmissionVolumeReverse;
            __instance.transmissionVolume = sourceSoundController.transmissionVolume;
            __instance.transmission = sourceSoundController.transmission;
            __instance.startEnginePitch = sourceSoundController.startEnginePitch;
            __instance.startEngineVolume = sourceSoundController.startEngineVolume;
            __instance.startEngine = sourceSoundController.startEngine;
            __instance.engineNoThrottlePitchFactor = sourceSoundController.engineNoThrottlePitchFactor;
            __instance.engineNoThrottleVolume = sourceSoundController.engineNoThrottleVolume;
            __instance.engineNoThrottle = sourceSoundController.engineNoThrottle;
            __instance.engineThrottlePitchFactor = sourceSoundController.engineThrottlePitchFactor;
            __instance.engineThrottleVolume = sourceSoundController.engineThrottleVolume;
            __instance.engineThrottle = sourceSoundController.engineThrottle;
        }
    }

    [HarmonyPatch(typeof(Wing), nameof(Wing.Init))]
    static class Wing_Patch
    {
        private static void Prefix(Wing __instance)
        {
            if (!CarPatcher.CheckCarInjected(__instance))
                return;

            if (__instance.name == "Wing_Front")
                __instance.WingPosition = Wing.WING_POSITION.FRONT;

            if (__instance.name == "Wing_Back")
                __instance.WingPosition = Wing.WING_POSITION.REAR;
        }
    }

    [HarmonyPatch(typeof(SkidmarksManager), "Awake")]
    static class SkidmarksManager_Patch
    {
        private static void Prefix(SkidmarksManager __instance)
        {
            __instance.skidmarks = new GameObject("This will die IMMEDIATELY").AddComponent<Skidmarks>();
        }
    }
}
