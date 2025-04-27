using System.Reflection;
using HarmonyLib;
using UnityEngine;

// TODO : Change class name for something more descriptive
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
            carPrefabName = "Car_Test";
            //return true;
            // TEST

            // This is a modified copy of the OG code
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

                Main.SetField(__instance, "prefabSpawnPos", BindingFlags.Instance, position);
                Main.SetField(__instance, "prefabSpawnRotation", BindingFlags.Instance, rotation);
            }
            else
                Main.Error("No spawn position found");

            (CarInfos infos, GameObject carPrefab) carData = ExtraCarManager.LoadBundleCar(carPrefabName);
            __result = UnityEngine.Object.Instantiate(carData.carPrefab, position, rotation);
            CarSpawner.ConfigureCar(__result, carData.infos);

            return false;
        }
    }
}