using System.Reflection;
using HarmonyLib;
using UnityEngine;

// TODO : Change class name for something more descriptive

// Here we're overriding the car spawning when spawning the player's car
namespace CarsExtended
{
    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.CreateCar))]
    static class PlayerManager_Patch
    {
        private static bool Prefix(PlayerManager __instance, bool isForwardStage, string carPrefabName, Transform optionalSpawnPosition, ref GameObject __result)
        {
            // skip if not an injected car
            //if (!ExtraCarManager.IsCarInjected(carPrefabName))
            //    return true;

            // TEST
            Main.Log(
                "__instance : " + (__instance != null) + " / " +
                "isForwardStage : " + isForwardStage + " / " +
                "carPrefabName : " + carPrefabName + " / " +
                "optionalSpawnPosition : " + (optionalSpawnPosition != null) +
                "__result : " + (__result != null)
            );

            carPrefabName = "TestPrefab";
            //return true;
            // TEST

            Main.Log("Overriding prefab loading to spawn car \"" + carPrefabName + "\"");

            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            GameObject spawnPoint;

            if (optionalSpawnPosition != null)
                spawnPoint = optionalSpawnPosition.gameObject;
            else if (isForwardStage)
                spawnPoint = GameObject.Find("SpawnPositionForward");
            else
                spawnPoint = GameObject.Find("SpawnPositionReverse");

            if (spawnPoint != null)
            {
                position = spawnPoint.transform.position + Vector3.up;
                rotation = spawnPoint.transform.rotation;

                Main.SetField<PlayerManager>(__instance, "prefabSpawnPos", BindingFlags.Instance, position);
                Main.SetField<PlayerManager>(__instance, "prefabSpawnRotation", BindingFlags.Instance, rotation);
            }
            else
                Main.Error("No spawn position found");

            (CarInfos infos, GameObject carPrefab) carData = ExtraCarManager.LoadBundleCar(carPrefabName);
            __result = Object.Instantiate(carData.carPrefab, position, rotation) as GameObject;
            CarSpawner.ConfigureCar(__result, carData.infos);

            Main.Log("Car \"" + carPrefabName + "\" is ready for play");
            return false;
        }
    }
}