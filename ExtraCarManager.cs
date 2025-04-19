using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using static UnityModManagerNet.UnityModManager;
using static Car;

namespace CarsExtended
{
    /// <summary>This class manages any cars added by other mods. Use this class to inject new cars into the game.</summary>
    public static class ExtraCarManager
    {
        private static List<Car> injectedCars = new List<Car>();
        private static Dictionary<Car, AssetBundle> carBundles = new Dictionary<Car, AssetBundle>();

        /// <summary>This will inject a new car in the game, please call this after CarManager.Init or as close as possible.</summary>
        public static void InjectCar(Car car, ModEntry sourceMod, string assetBundleName)
        {
            if (injectedCars.Contains(car))
            {
                Main.Error("You're trying to inject a car that has already been injected (" + car.name + ")");
                return;
            }

            GetCarListFromClass(car.carClass)?.Add(car);

            // cache car and asset bundle
            AssetBundle selectedBundle = null;

            foreach (KeyValuePair<Car, AssetBundle> pair in carBundles)
            {
                if (pair.Value.name == assetBundleName)
                {
                    selectedBundle = pair.Value;
                    break;
                }
            }

            if (selectedBundle == null)
                selectedBundle = AssetBundle.LoadFromFile(Path.Combine(sourceMod.Path, assetBundleName));

            carBundles.Add(car, selectedBundle);
            injectedCars.Add(car);

            Main.Log("Injected car \"" + car.name + "\" in the game.");
        }

        private static List<Car> GetCarListFromClass(CarClass carClass)
        {
            switch (carClass)
            {
                case CarClass.GROUP_2:
                    return CarManager.SixtiesCarList;

                case CarClass.GROUP_3:
                    return CarManager.SeventiesCarList;

                case CarClass.GROUP_4:
                    return CarManager.EightiesCarList;

                case CarClass.GROUP_B:
                    return CarManager.GroupBCarList;

                case CarClass.GROUP_S:
                    return CarManager.GroupSCarList;

                case CarClass.GROUP_A:
                    return CarManager.GroupACarList;

                case CarClass.BONUS_VANS:
                    return CarManager.BonusVanCarList;

                case CarClass.BONUS_PIAGGIO:
                    return CarManager.BonusPiaggioCarList;

                case CarClass.BONUS_DAKAR:
                    return CarManager.BonusDakarCarList;

                case CarClass.BONUS_LOGGING:
                    return CarManager.BonusLoggingCarList;
            }

            Main.Error("Couldn't find car list for class \"" + carClass + "\"");
            return null;
        }

        internal static bool IsCarInjected(Car car) => injectedCars.Contains(car);

        internal static bool IsCarInjected(string carPrefabName) => injectedCars.Find(item => item.prefabName == carPrefabName) != null;

        internal static GameObject LoadBundleCar(Car car)
        {
            if (!injectedCars.Contains(car))
            {
                Main.Error("The car you're trying to load isn't part of the injected cars (are you hijacking an original \"Resources.Load\" call ?)");
                return null;
            }

            return carBundles[car].LoadAsset<GameObject>(car.prefabName);
        }

        internal static GameObject LoadBundleCar(string carPrefabName)
        {
            Car selectedCar = injectedCars.Find(item => item.prefabName == carPrefabName);
            return LoadBundleCar(selectedCar);
        }
    }
}
