using System;
using AwesomeTechnologies.TouchReact;
using UnityEngine;

using static Drivetrain;

namespace CarsExtended
{
    internal static class CarSpawner
    {
        const float SHADOW_EXPANSION = 2;

        // TODO : How do I add cars to the car selection menu ?

        public static void ConfigureCar(GameObject carObj, CarInfos carInfos)
        {
            // TODO : why the hell is the car spawning looping ?

            Main.Log("Starting car configuration");
            carObj.SetActive(false);

            Setup setup = carObj.AddComponent<Setup>();
            setup.carClass = carInfos.data.carClass;

            // TODO : Check if I have any race conditions

            SetupRigidbody(carObj.GetComponent<Rigidbody>() ?? carObj.AddComponent<Rigidbody>());
            SetupDrivetrain(carObj.AddComponent<Drivetrain>(), carInfos);
            SetupCarDynamics(carObj.AddComponent<CarDynamics>());
            carObj.AddComponent<Arcader>();
            carObj.AddComponent<AerodynamicResistance>();
            Axles axles = SetupAxles(carObj.AddComponent<Axles>(), carInfos);
            carObj.AddComponent<AxisCarController>();
            SetupWings(carObj);
            SetupSoundController(carObj.AddComponent<SoundController>(), carInfos);
            carObj.AddComponent<SkidmarksManager>();
            carObj.AddComponent<PlayerCollider>();
            carObj.AddComponent<ResetCar>();
            SetupPlayerVibrator(carObj.AddComponent<PlayerVibrator>(), axles);
            SetupCollider(carObj);
            SetupParticleManager(carObj);
            SetupHeadlights(carObj);
            SetupShadow(carObj, carInfos);
            SetupRenderers(carObj, carInfos);

            carObj.SetActive(true);
        }

        // force setup rigidbody because I don't trust humans
        private static void SetupRigidbody(Rigidbody rigidbody)
        {
            rigidbody.drag = 0;
            rigidbody.angularDrag = 0;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.constraints = RigidbodyConstraints.None;
        }

        private static void SetupDrivetrain(Drivetrain drivetrain, CarInfos carInfos)
        {
            drivetrain.transmissionForParticles = (TransmissionForParticles)Enum.Parse(
                typeof(TransmissionForParticles),
                carInfos.data.carStats.Transmission.ToString()
            );
            drivetrain.engineOrientation = carInfos.engineOrientation;
        }

        private static void SetupCarDynamics(CarDynamics carDynamics)
        {
            Transform brakeTransform = carDynamics.transform.Find("PrefabSpawns/Lights/Brakelights");
            BrakeEffects brakeEffects = brakeTransform.gameObject.AddComponent<BrakeEffects>();

            brakeEffects.RightBrakeLightTransform = brakeTransform.Find("BreakLight R");
            brakeEffects.LeftBrakeLightTransform = brakeTransform.Find("BreakLight L");
        }

        private static Axles SetupAxles(Axles axles, CarInfos carInfos)
        {
            Transform wheelsRoot = axles.transform.Find("Wheels");

            axles.frontAxle = new Axle()
            {
                rightWheel = SetupWheel(wheelsRoot.Find("WheelFR").gameObject.AddComponent<Wheel>(), carInfos),
                leftWheel = SetupWheel(wheelsRoot.Find("WheelFL").gameObject.AddComponent<Wheel>(), carInfos)
            };
            axles.rearAxle = new Axle()
            {
                rightWheel = SetupWheel(wheelsRoot.Find("WheelRR").gameObject.AddComponent<Wheel>(), carInfos),
                leftWheel = SetupWheel(wheelsRoot.Find("WheelRL").gameObject.AddComponent<Wheel>(), carInfos)
            };

            if (wheelsRoot.Find("WheelRRL") != null)
            {
                axles.otherAxles = new Axle[1]
                {
                    new Axle()
                    {
                        rightWheel = SetupWheel(wheelsRoot.Find("WheelRRR").gameObject.AddComponent<Wheel>(), carInfos),
                        leftWheel = SetupWheel(wheelsRoot.Find("WheelRRL").gameObject.AddComponent<Wheel>(), carInfos)
                    }
                };
            }

            return axles;
        }

        private static Wheel SetupWheel(Wheel wheel, CarInfos carInfos)
        {
            wheel.width = carInfos.wheelWidth;
            wheel.radius = carInfos.wheelRadius;
            wheel.model = wheel.transform.Find(wheel.name).gameObject;

            return wheel;
        }

        private static void SetupWings(GameObject carObj)
        {
            carObj.transform.Find("Wing_Front").gameObject.AddComponent<Wing>().WingPosition = Wing.WING_POSITION.FRONT;
            carObj.transform.Find("Wing_Back").gameObject.AddComponent<Wing>().WingPosition = Wing.WING_POSITION.FRONT;
        }

        private static void SetupSoundController(SoundController soundController, CarInfos carInfos)
        {
            GameObject sourcePrefab = Resources.Load<GameObject>(
                "Prefabs/Cars/" + carInfos.soundSourcePrefabName + "/" + carInfos.soundSourcePrefabName
            );
            SoundController source = sourcePrefab.GetComponent<SoundController>();

            soundController.antiLagEnabled = source.antiLagEnabled;
            soundController.playExhaustParticles = source.playExhaustParticles;
            soundController.flatTireVolume = source.flatTireVolume;
            soundController.flatTire = source.flatTire;
            soundController.waterSplashNoise = source.waterSplashNoise;
            soundController.rollingNoiseOffroad = source.rollingNoiseOffroad;
            soundController.rollingNoiseSand = source.rollingNoiseSand;
            soundController.rollingNoiseGrass = source.rollingNoiseGrass;
            soundController.windVolume = source.windVolume;
            soundController.wind = source.wind;
            soundController.antilag1 = source.antilag1;
            soundController.backfire3 = source.backfire3;
            soundController.backfire2 = source.backfire2;
            soundController.backfire1 = source.backfire1;
            soundController.turboWasteGate3 = source.turboWasteGate3;
            soundController.turboWasteGate2 = source.turboWasteGate2;
            soundController.turboWasteGate1 = source.turboWasteGate1;
            soundController.shiftTriggerVolume = source.shiftTriggerVolume;
            soundController.shiftTrigger = source.shiftTrigger;
            soundController.scrapeNoiseVolume = source.scrapeNoiseVolume;
            soundController.scrapeNoise = source.scrapeNoise;
            soundController.crashLowVolume = source.crashLowVolume;
            soundController.crashLowSpeed = source.crashLowSpeed;
            soundController.crashHighVolume = source.crashHighVolume;
            soundController.crashHiSpeed = source.crashHiSpeed;
            soundController.skidPitchFactor = source.skidPitchFactor;
            soundController.gravelSkid = source.gravelSkid;
            soundController.tarmacWetSkid = source.tarmacWetSkid;
            soundController.tarmacDrySkid = source.tarmacDrySkid;
            soundController.brakeNoiseVolume = source.brakeNoiseVolume;
            soundController.brakeNoise = source.brakeNoise;
            soundController.transmissionSourcePitch = source.transmissionSourcePitch;
            soundController.transmissionVolumeReverse = source.transmissionVolumeReverse;
            soundController.transmissionVolume = source.transmissionVolume;
            soundController.transmission = source.transmission;
            soundController.startEnginePitch = source.startEnginePitch;
            soundController.startEngineVolume = source.startEngineVolume;
            soundController.startEngine = source.startEngine;
            soundController.engineNoThrottlePitchFactor = source.engineNoThrottlePitchFactor;
            soundController.engineNoThrottleVolume = source.engineNoThrottleVolume;
            soundController.engineNoThrottle = source.engineNoThrottle;
            soundController.engineThrottlePitchFactor = source.engineThrottlePitchFactor;
            soundController.engineThrottleVolume = source.engineThrottleVolume;
            soundController.engineThrottle = source.engineThrottle;

            Resources.UnloadAsset(sourcePrefab);
        }

        private static void SetupPlayerVibrator(PlayerVibrator playerVibrator, Axles axles)
        {
            playerVibrator.axles = axles;
        }

        private static void SetupCollider(GameObject carObj)
        {
            TouchReactCollider touchCollider = carObj.transform.Find("Collider").gameObject.AddComponent<TouchReactCollider>();
            touchCollider.AddChildColliders = false;
            touchCollider.ColliderScale = 1.6f;
        }

        private static void SetupParticleManager(GameObject carObj)
        {
            ParticleManager particleManager = carObj.transform.Find("PrefabSpawns").gameObject.AddComponent<ParticleManager>();
            Transform particlesRoot = particleManager.transform.Find("Particles");
            Transform lightsRoot = particleManager.transform.Find("Lights");

            particleManager.AmbientEffectsPosition = particlesRoot.Find("Ambient Effects");
            particleManager.Brakelight_Position_L = lightsRoot.Find("Brakelights/BrakeLight L");
            particleManager.Brakelight_Position_R = lightsRoot.Find("Brakelights/BrakeLight R");
            particleManager.DustPosition = particlesRoot.Find("Dust");
            particleManager.EngineSteamPosition = particlesRoot.Find("EngineSteam");
            particleManager.ExhaustBackfire_Position_1 = lightsRoot.Find("Backfire/Backfire L");
            particleManager.ExhaustBackfire_Position_2 = lightsRoot.Find("Backfire/Backfire R");
            particleManager.Exhaust1Position = particlesRoot.Find("Exhaust1");
            particleManager.Exhaust2Position = particlesRoot.Find("Exhaust2");
        }

        private static void SetupHeadlights(GameObject carObj)
        {
            HeadlightManager headlightManager = carObj.transform.Find("PrefabSpawns/Lights/Headlights").gameObject.AddComponent<HeadlightManager>();
            headlightManager.HeadlightPosition = headlightManager.transform.Find("Headlights");
        }

        private static void SetupShadow(GameObject carObj, CarInfos carInfos)
        {
            GameObject sourcePrefab = Resources.Load<GameObject>(
                "Prefabs/Cars/" + carInfos.soundSourcePrefabName + "/" + carInfos.soundSourcePrefabName
            );

            Transform colliderTransform = carObj.transform.Find("Collider");
            Transform shadowTransform = carObj.transform.Find("Shadow");
            Renderer renderer = shadowTransform.GetComponent<Renderer>();
            renderer.sharedMaterial = sourcePrefab.transform.Find("Shadow").GetComponent<Renderer>().sharedMaterial;

            Vector3 groundPos = Vector3.zero;
            Wheel[] wheels = carObj.transform.GetComponentsInChildren<Wheel>();

            groundPos = Vector3.Lerp(
                Vector3.Lerp(wheels[0].hitDown.point, wheels[1].hitDown.point, 0.5f),
                Vector3.Lerp(wheels[2].hitDown.point, wheels[3].hitDown.point, 0.5f),
                0.5f
            );
            shadowTransform.localPosition = new Vector3(0, carObj.transform.position.y - groundPos.y, 0);
            shadowTransform.localScale = carObj.GetComponentInChildren<TouchReactCollider>()
                .GetComponent<Collider>().bounds.size + new Vector3(1, 0, 1) * SHADOW_EXPANSION;
        }

        private static void SetupRenderers(GameObject carObj, CarInfos carInfos)
        {
            // TODO : How can I setup all the renderers ?
            // TODO : How does the game load liveries ?
        }

        // Custom/CarShader1 <= shader used for cars
    }
}