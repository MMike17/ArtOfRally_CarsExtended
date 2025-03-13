using UnityEngine;

namespace CarsExtended
{
    internal static class CarSpawner
    {
        // TODO : Check that the car mode spawning works when we try to select a car (might have to spawn cars there)

        // where do we spawn cars ?

        // GhostManager.(public void InitializeGhost())
        // PlayerManager.(public GameObject CreateCar(bool isForwardStage, string carPrefabName, Transform optionalSpawnPosition))

        // TODO : What infos do I need to lift from the og ?
        public static void SpawnCar(Transform parent, Vector3 position, Quaternion rotation, bool isLocalPos, bool isLocalRot)
        {
            //
        }
    }
}