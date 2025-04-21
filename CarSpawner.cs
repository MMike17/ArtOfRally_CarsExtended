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

            Transform lightsTransform = carObj.transform.Find("PrefabSpawns/Lights");
            Transform brakelightsTransform = lightsTransform.Find("Brakelights");
            GameObject sourceCarPrefab = Resources.Load<GameObject>(
                "Prefabs/Cars/" + carInfos.soundSourcePrefabName + "/" + carInfos.soundSourcePrefabName
            );

            carObj.AddComponent<AerodynamicResistance>();
            carObj.AddComponent<Arcader>();
            carObj.AddComponent<AxisCarController>();
            carObj.AddComponent<PlayerCollider>();
            carObj.AddComponent<ResetCar>();
            carObj.AddComponent<SkidmarksManager>();

            Axles axles = SetupAxles(carObj.AddComponent<Axles>(), carInfos);
            SetupCarDynamics(carObj.AddComponent<CarDynamics>(), brakelightsTransform);
            SetupCollider(carObj);
            SetupDrivetrain(carObj.AddComponent<Drivetrain>(), carInfos);
            SetupHeadlights(carObj, lightsTransform);
            SetupParticleManager(carObj, lightsTransform, brakelightsTransform);
            SetupPlayerVibrator(carObj.AddComponent<PlayerVibrator>(), axles);
            SetupRenderers(carObj, carInfos);
            SetupRigidbody(carObj.GetComponent<Rigidbody>() ?? carObj.AddComponent<Rigidbody>());
            SetupShadow(carObj, sourceCarPrefab);
            SetupSoundController(carObj.AddComponent<SoundController>(), sourceCarPrefab.GetComponent<SoundController>());
            SetupWings(carObj);

            Resources.UnloadAsset(sourceCarPrefab);
            carObj.SetActive(true);
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

        private static void SetupCarDynamics(CarDynamics carDynamics, Transform brakelightsTransform)
        {
            BrakeEffects brakeEffects = brakelightsTransform.gameObject.AddComponent<BrakeEffects>();

            brakeEffects.RightBrakeLightTransform = brakelightsTransform.Find("BreakLight R");
            brakeEffects.LeftBrakeLightTransform = brakelightsTransform.Find("BreakLight L");
        }

        private static void SetupCollider(GameObject carObj)
        {
            TouchReactCollider touchCollider = carObj.transform.Find("Collider").gameObject.AddComponent<TouchReactCollider>();
            touchCollider.AddChildColliders = false;
            touchCollider.ColliderScale = 1.6f;
        }

        private static void SetupDrivetrain(Drivetrain drivetrain, CarInfos carInfos)
        {
            drivetrain.transmissionForParticles = (TransmissionForParticles)Enum.Parse(
                typeof(TransmissionForParticles),
                carInfos.data.carStats.Transmission.ToString()
            );
            drivetrain.engineOrientation = carInfos.engineOrientation;
        }

        private static void SetupHeadlights(GameObject carObj, Transform lightsTransform)
        {
            HeadlightManager headlightManager = lightsTransform.Find("Headlights").gameObject.AddComponent<HeadlightManager>();
            headlightManager.HeadlightPosition = headlightManager.transform.Find("Headlights");
        }

        private static void SetupParticleManager(GameObject carObj, Transform lightsTransform, Transform brakelightsTransform)
        {
            ParticleManager particleManager = carObj.transform.Find("PrefabSpawns").gameObject.AddComponent<ParticleManager>();
            Transform particlesRoot = particleManager.transform.Find("Particles");

            particleManager.AmbientEffectsPosition = particlesRoot.Find("Ambient Effects");
            particleManager.Brakelight_Position_L = brakelightsTransform.Find("BrakeLight L");
            particleManager.Brakelight_Position_R = brakelightsTransform.Find("BrakeLight R");
            particleManager.DustPosition = particlesRoot.Find("Dust");
            particleManager.EngineSteamPosition = particlesRoot.Find("EngineSteam");
            particleManager.ExhaustBackfire_Position_1 = lightsTransform.Find("Backfire/Backfire L");
            particleManager.ExhaustBackfire_Position_2 = lightsTransform.Find("Backfire/Backfire R");
            particleManager.Exhaust1Position = particlesRoot.Find("Exhaust1");
            particleManager.Exhaust2Position = particlesRoot.Find("Exhaust2");
        }

        private static void SetupPlayerVibrator(PlayerVibrator playerVibrator, Axles axles) => playerVibrator.axles = axles;

        private static void SetupRenderers(GameObject carObj, CarInfos carInfos)
        {
            // TODO : How can I setup all the renderers ?
            // TODO : How does the game load liveries ?
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

        private static void SetupShadow(GameObject carObj, GameObject sourceCarPrefab)
        {
            Transform shadowTransform = carObj.transform.Find("Shadow");
            Renderer renderer = shadowTransform.GetComponent<Renderer>();
            renderer.sharedMaterial = sourceCarPrefab.transform.Find("Shadow").GetComponent<Renderer>().sharedMaterial;

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

        private static void SetupSoundController(SoundController soundController, SoundController sourceSoundController)
        {
            soundController.antiLagEnabled = sourceSoundController.antiLagEnabled;
            soundController.playExhaustParticles = sourceSoundController.playExhaustParticles;
            soundController.flatTireVolume = sourceSoundController.flatTireVolume;
            soundController.flatTire = sourceSoundController.flatTire;
            soundController.waterSplashNoise = sourceSoundController.waterSplashNoise;
            soundController.rollingNoiseOffroad = sourceSoundController.rollingNoiseOffroad;
            soundController.rollingNoiseSand = sourceSoundController.rollingNoiseSand;
            soundController.rollingNoiseGrass = sourceSoundController.rollingNoiseGrass;
            soundController.windVolume = sourceSoundController.windVolume;
            soundController.wind = sourceSoundController.wind;
            soundController.antilag1 = sourceSoundController.antilag1;
            soundController.backfire3 = sourceSoundController.backfire3;
            soundController.backfire2 = sourceSoundController.backfire2;
            soundController.backfire1 = sourceSoundController.backfire1;
            soundController.turboWasteGate3 = sourceSoundController.turboWasteGate3;
            soundController.turboWasteGate2 = sourceSoundController.turboWasteGate2;
            soundController.turboWasteGate1 = sourceSoundController.turboWasteGate1;
            soundController.shiftTriggerVolume = sourceSoundController.shiftTriggerVolume;
            soundController.shiftTrigger = sourceSoundController.shiftTrigger;
            soundController.scrapeNoiseVolume = sourceSoundController.scrapeNoiseVolume;
            soundController.scrapeNoise = sourceSoundController.scrapeNoise;
            soundController.crashLowVolume = sourceSoundController.crashLowVolume;
            soundController.crashLowSpeed = sourceSoundController.crashLowSpeed;
            soundController.crashHighVolume = sourceSoundController.crashHighVolume;
            soundController.crashHiSpeed = sourceSoundController.crashHiSpeed;
            soundController.skidPitchFactor = sourceSoundController.skidPitchFactor;
            soundController.gravelSkid = sourceSoundController.gravelSkid;
            soundController.tarmacWetSkid = sourceSoundController.tarmacWetSkid;
            soundController.tarmacDrySkid = sourceSoundController.tarmacDrySkid;
            soundController.brakeNoiseVolume = sourceSoundController.brakeNoiseVolume;
            soundController.brakeNoise = sourceSoundController.brakeNoise;
            soundController.transmissionSourcePitch = sourceSoundController.transmissionSourcePitch;
            soundController.transmissionVolumeReverse = sourceSoundController.transmissionVolumeReverse;
            soundController.transmissionVolume = sourceSoundController.transmissionVolume;
            soundController.transmission = sourceSoundController.transmission;
            soundController.startEnginePitch = sourceSoundController.startEnginePitch;
            soundController.startEngineVolume = sourceSoundController.startEngineVolume;
            soundController.startEngine = sourceSoundController.startEngine;
            soundController.engineNoThrottlePitchFactor = sourceSoundController.engineNoThrottlePitchFactor;
            soundController.engineNoThrottleVolume = sourceSoundController.engineNoThrottleVolume;
            soundController.engineNoThrottle = sourceSoundController.engineNoThrottle;
            soundController.engineThrottlePitchFactor = sourceSoundController.engineThrottlePitchFactor;
            soundController.engineThrottleVolume = sourceSoundController.engineThrottleVolume;
            soundController.engineThrottle = sourceSoundController.engineThrottle;
        }

        private static void SetupWings(GameObject carObj)
        {
            carObj.transform.Find("Wing_Front").gameObject.AddComponent<Wing>().WingPosition = Wing.WING_POSITION.FRONT;
            carObj.transform.Find("Wing_Back").gameObject.AddComponent<Wing>().WingPosition = Wing.WING_POSITION.FRONT;
        }

        // Custom/CarShader1 <= shader used for cars
        // Main texture ?
        // Normal ?
        // Colors ?
        // TODO : Map materials from CarShader
        // TODO : Map which shaders are used where on the car
    }
}