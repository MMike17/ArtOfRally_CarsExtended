using UnityEngine;

namespace CarsExtended
{
    /// <summary>Holds all information needed to inject cars</summary>
    public class CarInfos
    {
        // TODO : Add more settings as needed

        public enum EngineOrientation
        {
            Transverse,
            Longitudinal
        }

        public Car data;
        public Vector3 engineOrientation; // not sure I really need this...

        public CarInfos(Car car, EngineOrientation engineOrientation)
        {
            this.data = car;

            if (engineOrientation == EngineOrientation.Longitudinal)
                this.engineOrientation = Vector3.forward;
            else if (engineOrientation == EngineOrientation.Transverse)
                this.engineOrientation = Vector3.right;
        }
    }
}
