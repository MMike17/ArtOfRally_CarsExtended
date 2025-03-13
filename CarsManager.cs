namespace CarsExtended
{
    /// <summary>This class manages any cars added by other mods. Use this class to inject new cars into the game.</summary>
    public static class CarsManager
    {
        /// <summary>This will inject a new car in the game, please call this after CarManager.Init or as close as possible.</summary>
        public static void InjectCar(Car car)
        {
            // Get car list from CarManager
            // add new car to game list
            // add new car to internal list (used to override spawn)
        }

        public static bool IsCarInjected(Car car)
        {
            // TODO : Check if the car is an injected one or not
            return false;
        }
    }
}
