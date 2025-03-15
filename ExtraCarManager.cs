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
        private static Dictionary<Car, (AssetBundle, string)> carBundles = new Dictionary<Car, (AssetBundle, string)>();

        /// <summary>This will inject a new car in the game, please call this after CarManager.Init or as close as possible.</summary>
        public static void InjectCar(Car car, ModEntry sourceMod, string assetBundlePath, string prefabPath)
        {
            if (injectedCars.Contains(car))
            {
                Main.Error("You're trying to inject a car that has already been injected (" + car.name + ")");
                return;
            }

            GetCarListFromClass(car.carClass)?.Add(car);

            // cache car and asset bundle
            string[] frags = assetBundlePath.Split(Path.DirectorySeparatorChar);
            string assetBundleName = frags[frags.Length - 1];
            AssetBundle selectedBundle = null;

            foreach (KeyValuePair<Car, (AssetBundle, string)> pair in carBundles)
            {
                if (pair.Value.Item1.name == assetBundleName)
                {
                    selectedBundle = pair.Value.Item1;
                    break;
                }
            }

            if (selectedBundle == null)
                selectedBundle = AssetBundle.LoadFromFile(Path.Combine(sourceMod.Path, assetBundlePath));

            carBundles.Add(car, (selectedBundle, prefabPath));
            injectedCars.Add(car);

            Main.Log("Injected car \"" + car.name + "\" in the game.");

            // debug
            foreach (Car carCar in CarManager.SixtiesCarList)
                Main.Log(carCar.name + " : " + (IsCarInjected(carCar) ? "Injected" : "OG"));
            // debug
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

        internal static GameObject LoadBundleCar(Car car)
        {
            if (!injectedCars.Contains(car))
            {
                Main.Error("The car you're trying to load isn't part of the injected cars (are you hijacking an original \"Resources.Load\" call ?)");
                return null;
            }

            (AssetBundle bundle, string prefabPath) bundleInfo = carBundles[car];
            return bundleInfo.bundle.LoadAsset<GameObject>(bundleInfo.prefabPath);
        }
    }
}
