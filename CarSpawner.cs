using System;
using System.Reflection;
using UnityEngine;

using static Car;
using static Drivetrain;

namespace CarsExtended
{
    internal static class CarSpawner
    {
        // TODO : How do I add cars to the car selector ?

        public static void ConfigureCar(GameObject carObj, Car car)
        {
            // TODO : why the hell is the car spawning looping ?

            Main.Log("Starting car configuration");
            carObj.SetActive(false);

            Setup setup = carObj.AddComponent<Setup>();
            setup.carClass = car.carClass;

            // /!\ You can check out the "loaded" object and the "spawned" object /!\

            SetupRigidbody(carObj.GetComponent<Rigidbody>() ?? carObj.AddComponent<Rigidbody>());

            // TODO : Setup drivetrain
            Drivetrain drivetrain = carObj.AddComponent<Drivetrain>();
            drivetrain.transmissionForParticles = (TransmissionForParticles)Enum.Parse(
                typeof(TransmissionForParticles),
                car.carStats.Transmission.ToString()
            );

            // TODO : Check this in Editor !
            //drivetrain.engineOrientation = ; (Vector3)

            // TODO : Setup carDynamics 
            CarDynamics carDynamics = carObj.AddComponent<CarDynamics>();
            //carDynamics.skidmarks = ;
            //carDynamics.enableForceFeedback = ;
            //carDynamics.frontRearHandBrakeBalance = ;
            //carDynamics.frontRearBrakeBalance = ;
            //carDynamics.frontRearWeightRepartition = ; // set by setup
            //carDynamics.inertiaFactor = ;
            //carDynamics.dampAbsRoadVelo = ;
            //carDynamics.centerOfMass = ;
            //carDynamics.axles = ;
            //carDynamics.brakeEffects = ;
            //carDynamics.controller = ;

            // TODO : Setup arcader 
            Arcader arcader = carObj.AddComponent<Arcader>();

            // TODO : Setup aerodynamicResistance 
            AerodynamicResistance aerodynamicResistance = carObj.AddComponent<AerodynamicResistance>();

            // TODO : Setup axles 
            Axles axles = carObj.AddComponent<Axles>();

            // TODO : Setup axisCarController 
            AxisCarController axisCarController = carObj.AddComponent<AxisCarController>();

            // TODO : How do I manage that ?
            //car = GetComponentsInChildren<Wing>();

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

        // CAR PREFAB REQUIREMENTS

        // Main object must have a Rigidbody
    }
}