using System.Collections;
using AwesomeTechnologies.TouchReact;
using I2.Loc;
using UnityEngine;

namespace CarsExtended
{
    internal static class CarSpawner
    {
        const float SHADOW_EXPANSION = 2;

        internal static CarInfos carInfos;

        internal static Transform lightsTransform { get; private set; }
        internal static Transform brakelightsTransform { get; private set; }
        internal static GameObject sourceCarPrefab { get; private set; }

        // TODO : How do I add cars to the car selection menu ?

        internal static void ConfigureCar(GameObject carObj, CarInfos carInfos)
        {
            Main.Log("Started configuring \"" + carInfos.data.name + "\"");

            carObj.SetActive(false);
            carObj.layer = LayerMask.NameToLayer(Layers.CAR);
            carObj.tag = Tags.CAR;

            // caching for patch configuration
            CarSpawner.carInfos = carInfos;
            lightsTransform = carObj.transform.Find("PrefabSpawns/Lights");
            brakelightsTransform = lightsTransform.Find("Brakelights");
            sourceCarPrefab = Resources.Load<GameObject>("Prefabs/Cars/" + carInfos.soundSourcePrefabName + "/" + carInfos.soundSourcePrefabName);

            // Add Setup required components
            CheckAddComponent<Drivetrain>(carObj);
            SetupRigidbody(CheckAddComponent<Rigidbody>(carObj));
            CheckAddComponent<AxisCarController>(carObj);
            CheckAddComponent<Axles>(carObj);
            CheckAddComponent<BrakeEffects>(brakelightsTransform.gameObject);
            CheckAddComponent<CarDynamics>(carObj);
            CheckAddComponent<SoundController>(carObj);
            CheckAddComponent<Arcader>(carObj);
            CheckAddComponent<AerodynamicResistance>(carObj);
            CheckAddComponent<Wing>(carObj.transform.Find("Wing_Front").gameObject);
            CheckAddComponent<Wing>(carObj.transform.Find("Wing_Back").gameObject);

            // Add additional components
            CheckAddComponent<PlayerCollider>(carObj);
            CheckAddComponent<ResetCar>(carObj);
            CheckAddComponent<SkidmarksManager>(carObj);
            CheckAddComponent<PlayerVibrator>(carObj);
            CheckAddComponent<TouchReactCollider>(carObj.transform.Find("Collider").gameObject);
            CheckAddComponent<HeadlightManager>(lightsTransform.Find("Headlights").gameObject);
            CheckAddComponent<ParticleManager>(carObj.transform.Find("PrefabSpawns").gameObject);

            SetupSetup(CheckAddComponent<Setup>(carObj));
            SetupRenderers(carObj, carInfos);

            carObj.SetActive(true);
        }

        private static T CheckAddComponent<T>(GameObject obj) where T : Component => obj.GetComponent<T>() ?? obj.AddComponent<T>();

        private static void SetupSetup(Setup setup)
        {
            setup.filePath = "This codebase is a joke";
            setup.carClass = carInfos.data.carClass;

            // Force redo setup because WHY DOESN'T IT WORK ON THE FIRST PASS ?!
            CoroutineManager.Start(ForceDelayedActivation(setup));
        }

        private static IEnumerator ForceDelayedActivation(Setup setup)
        {
            yield return new WaitForSeconds(1f);
            setup.LoadSetup();
        }

        private static void SetupRenderers(GameObject carObj, CarInfos carInfos)
        {
            // TODO : How does the game load liveries and setup cars ?
            // TODO : How can I setup all the renderers ?

            // TODO : Material with "BrakeLightEmissive" in name is required for brake lights (check in scene)
        }

        // force setup rigidbody because I don't trust humans
        private static void SetupRigidbody(Rigidbody rigidbody)
        {
            rigidbody.drag = 0;
            rigidbody.angularDrag = 0;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = true;
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.constraints = RigidbodyConstraints.None;
        }

        // This is called once wheels are setup
        internal static void SetupShadow(Component carObj)
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

        // Custom/CarShader1 <= shader used for cars
        // Main texture ?
        // Normal ?
        // Colors ?
        // TODO : Map materials from CarShader
        // TODO : Map which shaders are used where on the car
    }
}