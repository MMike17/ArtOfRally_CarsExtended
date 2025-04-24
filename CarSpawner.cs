using AwesomeTechnologies.TouchReact;
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
            Main.Log("Configuring \"" + carInfos.data.name + "\"");

            // TODO : why the hell is the car spawning looping ?
            // Now it crashes...much better
            // Loops on this "Initing StageManager"

            carObj.SetActive(false);
            carObj.layer = LayerMask.NameToLayer(Layers.CAR);
            carObj.tag = Tags.CAR;

            // caching for patch configuration
            CarSpawner.carInfos = carInfos;
            lightsTransform = carObj.transform.Find("PrefabSpawns/Lights");
            brakelightsTransform = lightsTransform.Find("Brakelights");
            sourceCarPrefab = Resources.Load<GameObject>("Prefabs/Cars/" + carInfos.soundSourcePrefabName + "/" + carInfos.soundSourcePrefabName);

            // Add components
            carObj.AddComponent<Setup>();
            carObj.AddComponent<AerodynamicResistance>();
            carObj.AddComponent<Arcader>();
            carObj.AddComponent<AxisCarController>();
            carObj.AddComponent<PlayerCollider>();
            carObj.AddComponent<ResetCar>();
            carObj.AddComponent<SkidmarksManager>();
            carObj.AddComponent<Axles>();
            carObj.AddComponent<CarDynamics>();
            carObj.AddComponent<Drivetrain>();

            carObj.transform.Find("Collider").gameObject.AddComponent<TouchReactCollider>();
            lightsTransform.Find("Headlights").gameObject.AddComponent<HeadlightManager>();
            carObj.transform.Find("PrefabSpawns").gameObject.AddComponent<ParticleManager>();
            carObj.transform.Find("Wing_Front").gameObject.AddComponent<Wing>();
            carObj.transform.Find("Wing_Back").gameObject.AddComponent<Wing>();

            // unique setups
            SetupRenderers(carObj, carInfos);
            SetupRigidbody(carObj.GetComponent<Rigidbody>() ?? carObj.AddComponent<Rigidbody>());
            SetupShadow(carObj, sourceCarPrefab);

            carObj.SetActive(true);
        }

        private static void SetupRenderers(GameObject carObj, CarInfos carInfos)
        {
            // TODO : How can I setup all the renderers ?
            // TODO : How does the game load liveries and setup cars ?
            // Maybe that's what crashing the game...

            // TODO : Material with "BrakeLightEmissive" in name is required for brake lights (check in scene)
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

        // Custom/CarShader1 <= shader used for cars
        // Main texture ?
        // Normal ?
        // Colors ?
        // TODO : Map materials from CarShader
        // TODO : Map which shaders are used where on the car
    }
}