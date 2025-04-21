using UnityEngine;

namespace CarsExtended
{
    /// <summary>Holds all information needed to inject cars</summary>
    public class CarInfos
    {
        public enum EngineOrientation
        {
            Transverse,
            Longitudinal
        }

        public enum SourceCar
        {
            // TODO : Make enum with all default cars (used for sound copying)
        }

        public Car data;

        public Vector3 engineOrientation; // not sure I really need this...
        public float wheelWidth = 0.26f;
        public float wheelRadius = 0.34f;
        public string soundSourcePrefabName;

        // TODO : Should I make a guide about settings ? (sizes examples and how it touches behaviours)

        public CarInfos(Car car, EngineOrientation engineOrientation, float wheelWidth, float wheelRadius, SourceCar soundSource)
        {
            data = car;

            if (engineOrientation == EngineOrientation.Longitudinal)
                this.engineOrientation = Vector3.forward;
            else if (engineOrientation == EngineOrientation.Transverse)
                this.engineOrientation = Vector3.right;

            this.wheelWidth = wheelWidth;
            this.wheelRadius = wheelRadius;
            soundSourcePrefabName = soundSource.ToString();
        }
    }
}
